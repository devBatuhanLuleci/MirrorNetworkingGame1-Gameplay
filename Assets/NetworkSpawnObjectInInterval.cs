using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using static UnityEditor.PlayerSettings;

public class NetworkSpawnObjectInInterval : NetworkBehaviour
{
    public Transform spawnPoint;
    public GameObject prefabToSpawn;
    public float maxDistance = 5f;
    public float spawnInterval = 1f;
    public bool StopSpawningOnGameFinish = true;
    private float lastSpawnTime = 0f;
    private NetworkIdentity networkIdentity;
    Coroutine spawningCoroutine;
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
        spawningCoroutine = StartCoroutine(SpawnLoop());
    }
    private void OnEnable()
    {
        NetworkedTimer.OnTimeFinished += OnGameFinished;
    }
    private void OnDisable()
    {
        NetworkedTimer.OnTimeFinished -= OnGameFinished;

    }

    private void OnGameFinished()
    {
        if (StopSpawningOnGameFinish)
        {

            if (spawningCoroutine != null)
            {
                StopCoroutine(spawningCoroutine);
                Debug.Log("spawning stoped");
            }
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnObjectWithDiffrentForce(transform.position);
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
    public void SpawnObjectWithDiffrentForce(Vector3 spawnPos)
    {


        var name = prefabToSpawn.transform.name;
        Vector3 spawnPosition = spawnPos;
        var spawnedBullet = ObjectPooler.Instance.Get(name, spawnPosition, Quaternion.identity).GetComponent<Throwable>();
        Vector3 randomDir = Vector3.zero.RandomDirection();
        float randomRange = 0;
        randomRange= randomRange.RandomRange(1,2);
        Debug.Log($" dir: {randomDir}  range: {randomRange}");
        spawnedBullet.OnObjectSpawn();
        spawnedBullet.Throw(randomDir, randomRange);
        spawnedBullet.InitInfo(randomDir);
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
