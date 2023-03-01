using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{

    #region Public Fields
    public string SelectedCharacter = "Character1";
    #endregion

    #region Private Fields
    private Health health;
    private Energy energy;
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
        energy = GetComponent<Energy>();
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
            GetSpine(characterMesh.transform);
            SetPlayerDataForAllClient();
        }
        else // Do anything on server
        {
            SetPlayerDataForServer();
        }

        // Do anything on only local client
        if (NetworkIdentity.isLocalPlayer && !NetworkIdentity.isServer)
        {
            InitLocalPlayer();
        }

        // Do anything   on other clients and server
        if (!NetworkIdentity.isLocalPlayer && !NetworkIdentity.isServer)
        {
            InitOtherPlayers();
        }

    }

    private void SetPlayerDataForAllClient()
    {        
        playerUIHandler.Initialize();

    }

    private void SetPlayerDataForServer()
    {
        var data = ACGDataManager.Instance.GetCharacterData().Attributes;
        try
        {
            if (data.TryGetValue("Health", out var healthAttribute))
            {
                health.ResetValues((int)healthAttribute.Value);
                playerUIHandler.enabled = false;
            }
            if (data.TryGetValue("Energy", out var energyAttribute))
            {
                energy.MakeEnergyBarsFull((int)healthAttribute.Value);
            }
        }
        catch (Exception ex) { Debug.LogError(ex.Message); }
    }

    private void InitLocalPlayer()
    {
        var playerController = GetComponent<PlayerController>();

        CameraController.Instance.Initialize(transform);
        InputHandler.Instance.Init(playerController);
    }
    private void InitOtherPlayers()
    {
        var playerController = GetComponent<PlayerController>();
        playerUIHandler.DisablePanel();


    }
    public void GetSpine(Transform characterMesh)
    {
        var SpineObj = characterMesh.FindByName("Spine");

        GameObject obj = new GameObject();
        obj.transform.parent = SpineObj.parent;
        obj.transform.name = "Char_Rotator";
        obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        obj.transform.localScale = Vector3.one;
        SpineObj.SetParent(obj.transform);
        playerController.SetSpineRotator(obj.transform);


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
