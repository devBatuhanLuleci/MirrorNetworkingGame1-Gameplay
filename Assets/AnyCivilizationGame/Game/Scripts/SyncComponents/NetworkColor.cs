using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkColor : NetworkBehaviour
{
    [Header("Synchronization")]

    [Range(0f, 1f)]
    public float sendInterval;

    private float intervalTime = 0;
    private void Update()
    {
        if (!GetComponent<NetworkIdentity>().isServer) return;
        if (Time.time > intervalTime)
        {
            intervalTime = Time.time + sendInterval;
            RPCSyncData(GetComponentInChildren<MeshRenderer>().material.color);
        }
    }
    [ClientRpc]
    private void RPCSyncData(Color color)
    {
        GetComponentInChildren<MeshRenderer>().material.color = color;
    }
}
