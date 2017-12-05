using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(MeshRenderer))]
public class TextMeshOutline : MonoBehaviour
{
    public float pixelSize = 2;
    public Color outlineColor = Color.black;
    public bool resolutionDependant = false;
    public int doubleResolution = 1024;

    private TextMesh textMesh;
    private MeshRenderer meshRenderer;
    private Vector3 previousPosition;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        meshRenderer = GetComponent<MeshRenderer>();

        for (int i = 0; i < 8; i++)
        {
            GameObject outline = new GameObject("outline", typeof(TextMesh));
            outline.transform.parent = transform;
            outline.transform.localScale = Vector3.one;
            outline.transform.localEulerAngles = Vector3.zero;

            MeshRenderer otherMeshRenderer = outline.GetComponent<MeshRenderer>();
            otherMeshRenderer.material = new Material(meshRenderer.material);
            otherMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            otherMeshRenderer.receiveShadows = false;
            otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
            otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
            otherMeshRenderer.sortingOrder = meshRenderer.sortingOrder - 1;
        }

        previousPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        outlineColor.a = textMesh.color.a * textMesh.color.a;

        // copy attributes
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (i == 0 && CheckIfAllSame(child))
                break;

            TextMesh other = child.GetComponent<TextMesh>();
            other.color = outlineColor;
            other.text = textMesh.text;
            other.alignment = textMesh.alignment;
            other.anchor = textMesh.anchor;
            other.characterSize = textMesh.characterSize;
            other.font = textMesh.font;
            other.fontSize = textMesh.fontSize;
            other.fontStyle = textMesh.fontStyle;
            other.richText = textMesh.richText;
            other.tabSize = textMesh.tabSize;
            other.lineSpacing = textMesh.lineSpacing;
            other.offsetZ = textMesh.offsetZ;
            
            bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
            Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * pixelSize : pixelSize);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint + pixelOffset);
            other.transform.position = worldPoint;

            MeshRenderer otherMeshRenderer = child.GetComponent<MeshRenderer>();
            otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
            otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
        }

        previousPosition = transform.position;
    }

    private Vector3 GetOffset(int i)
    {
        switch (i % 8)
        {
            case 0: return new Vector3(0, 1, 0);
            case 1: return new Vector3(1, 1, 0);
            case 2: return new Vector3(1, 0, 0);
            case 3: return new Vector3(1, -1, 0);
            case 4: return new Vector3(0, -1, 0);
            case 5: return new Vector3(-1, -1, 0);
            case 6: return new Vector3(-1, 0, 0);
            case 7: return new Vector3(-1, 1, 0);
            default: return Vector3.zero;
        }
    }

    private bool CheckIfAllSame(Transform child)
    {
        TextMesh other = child.GetComponent<TextMesh>();

        if (other.color != outlineColor)
            return false;
        if (other.text != textMesh.text)
            return false;
        if (other.alignment != textMesh.alignment)
            return false;
        if (other.anchor != textMesh.anchor)
            return false;
        if (other.characterSize != textMesh.characterSize)
            return false;
        if (other.font != textMesh.font)
            return false;
        if (other.fontSize != textMesh.fontSize)
            return false;
        if (other.fontStyle != textMesh.fontStyle)
            return false;
        if (other.richText != textMesh.richText)
            return false;
        if (other.tabSize != textMesh.tabSize)
            return false;
        if (other.lineSpacing != textMesh.lineSpacing)
            return false;
        if (other.offsetZ != textMesh.offsetZ)
            return false;

        if (transform.position != previousPosition)
            return false;

        MeshRenderer otherMeshRenderer = child.GetComponent<MeshRenderer>();
        if (otherMeshRenderer.sortingLayerID != meshRenderer.sortingLayerID)
            return false;
        if (otherMeshRenderer.sortingLayerName != meshRenderer.sortingLayerName)
            return false;

        return true;
    }
}