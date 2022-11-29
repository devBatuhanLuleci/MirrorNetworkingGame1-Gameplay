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
    private GameObject characterMesh;
    private PlayerController playerController;
    #endregion


    private void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (!NetworkIdentity.isServer)
        {
            characterMesh = CreateCharacterMesh();
            playerController.PlayerAnimatorController = characterMesh.GetComponent<Animator>();
        }


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
