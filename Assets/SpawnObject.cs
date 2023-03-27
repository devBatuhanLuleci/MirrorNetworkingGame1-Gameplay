using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnObject : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject prefabToSpawn;
    public float maxDistance = 5f;
    public float spawnInterval = 1f;
    private float lastSpawnTime = 0f;


    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpawnLoop();
        }
    }
    private void Update()
    {
        
    }
    private void StartSpawnLoop()
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
        Vector3 spawnPosition = GetRandomPointNearTransform(spawnPoint.position, maxDistance);
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
    public void SpawnObjectInNearWithTradictor()
    {
        Vector3 spawnPosition = GetRandomPointNearTransform(spawnPoint.position, maxDistance);
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomPointNearTransform(Vector3 center, float maxDistance)
    {
        Vector2 randomCircle = Random.insideUnitCircle * maxDistance;
        Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);
        return center + randomPoint;
    }
}
