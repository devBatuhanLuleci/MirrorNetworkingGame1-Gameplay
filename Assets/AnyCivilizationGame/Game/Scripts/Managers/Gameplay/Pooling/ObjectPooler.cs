using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;


    }
    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance;


    private void Awake()
    {
        Instance = this;
    }

    #endregion
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();


        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab,transform);
                obj.name = pool.prefab.name + i;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
          
            poolDictionary.Add(pool.tag, objectPool);


        }

    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, PlayerAttack playerAttack, float rotAngle, float BulletTargetOffSetZ)
    {
       // Debug.Log(tag);
        if (!poolDictionary.ContainsKey(tag))
        {

            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectToSpawn.SetActive(true);
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {

            pooledObj.OnObjectSpawn(rotAngle);

        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;

    }



}
