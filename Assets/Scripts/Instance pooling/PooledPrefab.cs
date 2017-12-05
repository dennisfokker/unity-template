using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledPrefab
{
    public GameObject Prefab;
    public int PoolSize = 10;
    public bool ConfirmedNotPoolable = false;
}
