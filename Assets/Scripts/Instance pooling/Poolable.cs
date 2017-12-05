using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    /// <summary>
    /// Initialize the pooled object.
    /// </summary>
    /// <param name="args">Arguments</param>
    public virtual void OnSpawn(params object[] args)
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deinitialize the pooled object.
    /// </summary>
    /// <param name="args">Arguments</param>
    public virtual void OnDespawn(params object[] args)
    {
        gameObject.SetActive(false);
    }
}
