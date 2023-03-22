using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEditor.PlayerSettings;

public class NetworkSpawnObjectInInterval : NetworkBehaviour
{
    public Transform spawnPoint;
    public GameObject prefabToSpawn;
    public float maxDistance = 5f;
    public float spawnInterval = 1f;
    private float lastSpawnTime = 0f;
    private NetworkIdentity networkIdentity;
    private void Awake()
    {
        networkIdentity=GetComponent<NetworkIdentity>();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && networkIdentity.isServer)
        //{
        //    StartSpawnLoop();
        //}
    }
    public void StartSpawnLoop()
    {
        // Start an infinite loop that spawns objects with the specified interval time
        StartCoroutine(SpawnLoop());
    }

    private System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnObjectInNear();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnObjectInNear()
    {

        
        var name = prefabToSpawn.transform.name;
        Vector3 spawnPosition = GetRandomPointNearTransform(spawnPoint.position, maxDistance);
        var spawnedBullet = ObjectPooler.Instance.Get(name, spawnPosition, Quaternion.identity).GetComponent<Throwable>();
        
    NetworkServer.Spawn(spawnedBullet.gameObject);


        //  Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomPointNearTransform(Vector3 center, float maxDistance)
    {
        Vector2 randomCircle = Random.insideUnitCircle * maxDistance;
        Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);
        return center + randomPoint;
    }

}
