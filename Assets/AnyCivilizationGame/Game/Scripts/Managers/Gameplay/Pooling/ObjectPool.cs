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

    private void RegisterPools()
    {
        NetworkClient.RegisterPrefab(pool.prefab, SpawnHandler, UnspawnHandler);
    }
    // used by NetworkClient.RegisterPrefab
    GameObject SpawnHandler(SpawnMessage msg) => Get(msg.position, msg.rotation);

    // used by NetworkClient.RegisterPrefab
    void UnspawnHandler(GameObject spawned) => Return(spawned);


    public GameObject Get(Vector3 position, Quaternion rotation)
    {


        GameObject objectToSpawn = pools.Get();    

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;

    }



    // Used to put object back into pool so they can b
    // Should be used on server after unspawning an object
    // Used on client by NetworkClient to unspawn objects
    public void Return(GameObject spawned)
    {
        // disable object
        spawned.SetActive(false);

        // add back to pool
        pools.Return(spawned);
    }
}
