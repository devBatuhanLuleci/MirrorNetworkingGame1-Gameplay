using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class takes bullet spawn point arguments.
/// </summary>

[System.Serializable]
public class BulletSpawnPoints
{
    public string SpawnPointName;
    public Vector3 spawnPoint;
    [HideInInspector]
    public Vector3 BulletInitPos;
    [HideInInspector]
    public Vector3 BulletInitRot;
}
