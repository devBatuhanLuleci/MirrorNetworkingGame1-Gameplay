using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[Serializable]
public class ObjectPool : MonoBehaviour
{

    public Pool pool;
    public Pool<GameObject> pools;
    public void Initialize(Pool pool)
    {
        this.pool = pool;

        pools = new Pool<GameObject>(CreateObject, pool.size);
        RegisterPools();
    }


    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(pool.prefab, transform);
        obj.SetActive(false);
        return obj;
    }
    /// <summary>
    /// Reister pool prefab for network spawn
    /// </summary>
    private void RegisterPools()
    {
        NetworkClient.RegisterPrefab(pool.prefab, SpawnHandler, UnspawnHandler);
    }

    /// <summary>
    /// used by NetworkClient.RegisterPrefab
    /// </summary>
    GameObject SpawnHandler(SpawnMessage msg) => Get(msg.position, msg.rotation);

    // used by NetworkClient.RegisterPrefab
    void UnspawnHandler(GameObject spawned) => Return(spawned);

    /// <summary>
    /// Get next element of this pool
    /// </summary>
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject next = pools.Get();

        next.transform.position = position;
        next.transform.rotation = rotation;
        next.SetActive(true);

        return next;
    }




    /// <summary>
    /// Used to put object back into pool so they can b
    /// Should be used on server after unspawning an object
    ///  Used on client by NetworkClient to unspawn objects
    /// </summary>
    public void Return(GameObject spawned)
    {
        // disable object
        spawned.SetActive(false);

        // add back to pool
        pools.Return(spawned);
    }
}
