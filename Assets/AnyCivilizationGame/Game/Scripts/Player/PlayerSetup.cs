using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{

    #region Public Fields
    public string SelectedCharacter = "Character1";
    #endregion

    #region Private Fields
    private Health health;
    private PlayerUIHandler playerUIHandler;
    private PlayerMovement playerMovement;
    private GameObject characterMesh;

    private PlayerController playerController;

    private NetworkIdentity NetworkIdentity;

    #endregion


    private void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();

        playerController = GetComponent<PlayerController>();

        health = GetComponent<Health>();
        playerUIHandler = GetComponent<PlayerUIHandler>();
        playerMovement = GetComponent<PlayerMovement>();

    }

    public void Start()
    {
        // Do anything on all client but not server
        if (!NetworkIdentity.isServer)
        {
            characterMesh = CreateCharacterMesh();

            playerController.PlayerAnimatorController = characterMesh.GetComponent<Animator>();

            health.ResetValues(100);
            playerUIHandler.Initialize(health.MaxHealth);
        }
        else // Do anything on server
        {
            health.ResetValues(100);
            playerUIHandler.enabled = false;

        }

        // Do anything on only local client
        if (NetworkIdentity.isLocalPlayer && !NetworkIdentity.isServer)
        {
            InitLocalPlayer();
        }

    }

    private void InitLocalPlayer()
    {
        var playerController = GetComponent<PlayerController>();

        CameraController.Instance.Initialize(transform);
        InputHandler.Instance.Init(playerController);
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
