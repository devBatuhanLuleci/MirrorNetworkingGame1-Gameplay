using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{

    #region Public Fields
    public string SelectedCharacter = "Character1";
    #endregion

    #region Private Fields
    private NetworkIdentity NetworkIdentity;
    #endregion


    private void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
    }

    private void Start()
    {
        if (NetworkIdentity.isLocalPlayer && !NetworkIdentity.isServer)
        {
            InitLocalPlayer();

        }
        else if (NetworkIdentity.isServer)
        {
            GetComponent<Health>().ResetValues();
        }
    }

    private void InitLocalPlayer()
    {
        var characterMesh = CreateCharacterMesh();

        var playerController = GetComponent<PlayerController>();
        var playerMovement = GetComponent<PlayerMovement>();

        CameraController.Instance.Initialize(transform);
        InputHandler.Instance.Init(playerController);
        playerMovement.PlayerAnimatorController = characterMesh.GetComponent<Animator>();
    }

    private GameObject CreateCharacterMesh()
    {
        var path = "Characters/" + SelectedCharacter;
        var mesh = Resources.Load<GameObject>(path);
        if (mesh == null)
        {
            Debug.LogError(SelectedCharacter + " is not found in " + path);
            return null;
        }
        return Instantiate(mesh, transform);
    }
}
