
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : ObjectSetup
{

    #region Public Fields
    public string SelectedCharacter = "Character1";
    #endregion

    #region Private Fields
  
    private Energy energy;
    private PlayerMovement playerMovement;
    private GameObject characterMesh;

    private PlayerController playerController;

  

    #endregion


    public override void Awake()
    {
        base.Awake();

        playerController = GetComponent<PlayerController>();

       
        energy = GetComponent<Energy>();
    
        playerMovement = GetComponent<PlayerMovement>();

       
    }

    public override  void Start()
    {
        // Do anything on all client but not server
        base.Start();
        if (!NetworkIdentity.isServer)
        {


            characterMesh = CreateCharacterMesh();

            playerController.PlayerAnimatorController = characterMesh.GetComponent<Animator>();
            GetSpine(characterMesh.transform);

          
           

        }
        else // Do anything on server
        {
            energy.MakeEnergyBarsFull();
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


    private void InitLocalPlayer()
    {
        var playerController = GetComponent<PlayerController>();

        CameraController.Instance.Initialize(transform);
        InputHandler.Instance.Init(playerController);
    }
    private void InitOtherPlayers()
    {
        var playerController = GetComponent<PlayerController>();
        objectUIHandler.DisablePanel();


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
