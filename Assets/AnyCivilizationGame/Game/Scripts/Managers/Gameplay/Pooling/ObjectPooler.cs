using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<Pool> pools;

    public Dictionary<string, ObjectPool> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance;


    private void Awake()
    {
        Instance = this;
    }

    #endregion
    private void Start()
    {
        InitializePools();
    }

    /// <summary>
    /// Create all and register all pools.
    /// </summary>
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, ObjectPool>();
        foreach (Pool pool in pools)
        {
            var objectPool = new GameObject().AddComponent<ObjectPool>();
            objectPool.transform.SetParent(transform);
            objectPool.Initialize(pool);
            objectPool.name = pool.prefab.name + "_ObjectPool";
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    /// <summary>
    /// Get or create next pool object.
    /// </summary>
    public GameObject Get(string tag, Vector3 position, Quaternion rotation)
    {
        // Debug.Log(tag);
        if (poolDictionary.TryGetValue(tag, out var pool))
        {
            return pool.Get(position, rotation);
        }

        Debug.Log("Pool with tag " + tag + " doesn't exist.");
        return null;
    }



}
