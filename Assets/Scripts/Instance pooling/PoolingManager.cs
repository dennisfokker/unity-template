using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public PooledPrefab[] Prefabs;

    private static PoolingManager _instance;
    private List<GameObject>[] pool;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create pool and loop over all prefabs.
        pool = new List<GameObject>[Prefabs.Length];
        for (int i = 0; i < Prefabs.Length; i++)
        {
            PooledPrefab pp = Prefabs[i];
            // If no poolable script found and prefab hasn't confirmed this fact: print an error.
            if (!pp.ConfirmedNotPoolable && pp.Prefab.GetComponent<Poolable>() == null)
            {
                Debug.LogError("Game object \"" + pp.Prefab.name + "\" isn't poolable.");
            }

            // Make sure the original object isn't active.
            pp.Prefab.SetActive(false);

            // Create a pool parent (to keep inspector clean)
            GameObject poolParent = new GameObject(pp.Prefab.name + " pool");

            pool[i] = new List<GameObject>();
            // Fill the prefab's pool with the assigned amount of instances and put in the pool parent.
            for (int j = 0; j < pp.PoolSize; j++)
            {
                GameObject go = Instantiate(pp.Prefab);
                go.transform.SetParent(poolParent.transform);
                go.name = pp.Prefab.name;
                pool[i].Add(go);
            }
        }
    }

    /// <summary>
    /// Spawn an available object and place at given position and rotation.
    /// </summary>
    /// <param name="prefabName">Name of instance to spawn/param>
    /// <param name="position">Position to spawn at</param>
    /// <param name="rotation">Rotation to spawn in.</param>
    /// <param name="extendPool">Enlarge pool if all instances already used.</param>
    /// <returns>Spawned object or null of none available.</returns>
    public GameObject Spawn(string prefabName, Vector3 position, Quaternion? rotation = null, bool extendPool = false, params object[] args)
    {
        // Loop over all pooled prefabs.
        for (int i = 0; i < Prefabs.Length; i++)
        {
            GameObject prefab = Prefabs[i].Prefab;

            // Check if desired prefab.
            if (prefab.name == prefabName)
            {
                GameObject pooledObject = null;
                // Check if still items left in pool.
                if (pool[i].Count > 0)
                {
                    pooledObject = pool[i][0];
                    pool[i].RemoveAt(0);
                }
                // If no items left, check if should expand.
                else if (extendPool)
                {
                    pooledObject = Instantiate(Prefabs[i].Prefab);
                    pooledObject.name = prefab.name;
                }
                // Don't expand: break;
                else
                {
                    break;
                }

                // Set rotation and position.
                pooledObject.transform.position = position;
                pooledObject.transform.rotation = rotation == null ? Prefabs[i].Prefab.transform.rotation : (Quaternion)rotation;

                // If it has a pool script, fire it's OnSpawn event.
                Poolable p = pooledObject.GetComponent<Poolable>();
                if (p != null)
                    p.OnSpawn(args);
                else
                    pooledObject.SetActive(true);

                return pooledObject;
            }
        }

        return null;
    }

    /// <summary>
    /// Despawn pooled game object.
    /// </summary>
    /// <param name="poolable">Pooled game object</param>
    public void Despawn(GameObject poolable, params object[] args)
    {
        // Loop over all pooled prefabs.
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // Check if current pool is same as current object.
            if (Prefabs[i].Prefab.name == poolable.name)
            {
                // Add to pool again.
                pool[i].Add(poolable);

                // If it has a pool script, fire it's OnDespawn event.
                Poolable p = poolable.GetComponent<Poolable>();
                if (p != null)
                    p.OnDespawn(args);
                else
                    poolable.SetActive(false);

                return;
            }
        }
    }
}
