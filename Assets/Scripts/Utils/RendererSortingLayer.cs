using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class RendererSortingLayer : MonoBehaviour
{
    public string sortingLayer;
    public int orderInLayer;

    private Renderer Renderer;

    void Update()
    {
        Renderer = GetComponent<Renderer>();
        SetSortingLayer();
    }

    private void SetSortingLayer()
    {
        Renderer.sortingLayerName = sortingLayer;
        Renderer.sortingOrder = orderInLayer;
    }
}