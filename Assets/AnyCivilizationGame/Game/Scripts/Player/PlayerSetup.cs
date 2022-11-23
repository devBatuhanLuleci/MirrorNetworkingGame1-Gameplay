using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private NetworkIdentity NetworkIdentity;

    private void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
    }

    private void Start()
    {
        if (NetworkIdentity.isLocalPlayer && !NetworkIdentity.isServer)
        {
            CameraController.Instance.Initialize(transform);
            var player = GetComponent<PlayerController>();
            InputHandler.Instance.Init(player);
        }else if (NetworkIdentity.isServer)
        {
            GetComponent<Health>().ResetValues();
        }
    }
}
