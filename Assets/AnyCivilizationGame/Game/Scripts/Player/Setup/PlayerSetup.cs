
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSetup : ObjectSetup
{

    #region Public Fields
    public string SelectedCharacter = "Character1";

    [SyncVar]
    public NetworkedGameManager.TeamTypes Team;
    #endregion


    #region Private Fields
  
    private Energy energy;
    private PlayerMovement playerMovement;
    private GameObject characterMesh;

    private PlayerController playerController;
  //  private CharacterSpecificStats characterSpecificStats;

    //private PlayerUIHandler objectUIHandler;


    #endregion


    public override void Awake()
    {
        base.Awake();

        playerController = GetComponent<PlayerController>();

       
        energy = GetComponent<Energy>();
    
        playerMovement = GetComponent<PlayerMovement>();
    }

    public override void Start()
    {
        // Do anything on all client but not server
        base.Start();
        if (!NetworkIdentity.isServer)
        {
            characterMesh = CreateCharacterMesh();

            playerController.PlayerAnimatorController = characterMesh.GetComponent<Animator>();
            playerController.CharacterSpecificStats = characterMesh.GetComponent<CharacterSpecificStats>();
           
            GetSpine(characterMesh.transform);  
            SetPlayerDataForAllClient();
        }
        else // Do anything on server
        {
            SetObjectDataForServer();
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
    public void InitilizeTeamOfThisPlayer(NetworkedGameManager.TeamTypes team)
    {

        this.Team=team;

    }


    private void SetPlayerDataForAllClient()
    {
        objectUIHandler.Initialize();

    }

    public override void SetObjectDataForServer()
    {
        base.SetObjectDataForServer();
        var data = ACGDataManager.Instance.GetCharacterData().Attributes;
        try
        {
            if (data.TryGetValue("Health", out var healthAttribute))
            {
                health.ResetValues((int)healthAttribute.Value);
                objectUIHandler.enabled = false;
            }
            if (data.TryGetValue("Energy", out var energyAttribute))
            {
                energy.MakeEnergyBarsFull((int)energyAttribute.Value);
            }
        }
        catch (Exception ex) { Debug.LogError(ex.Message); }
    }
 

    private void InitLocalPlayer()
    {
        var playerController = GetComponent<PlayerController>();

        CameraController.Instance.Initialize(transform);
        InputHandler.Instance.Init(playerController);
        playerController.playerUIHandler.Change_TeamIndicator_Color("Me");
        playerController.playerUIHandler.Change_TeamHealthBar_Color("Me");
    }
    private void InitOtherPlayers()
    {
        var playerController = GetComponent<PlayerController>();
        base.objectUIHandler.DisablePanel();

        //TODO :  SetTeamColor delaylı olarak çalışıyor, bu methodu direk çalıştırdığımızda referans hatası alıyoruz .
        //  Invoke("SetTeamColor", 3);
        StartCoroutine(SetTeamColorLocal(netIdentity.netId));
    }
    public override IEnumerator SetTeamColorLocal(uint netId)
    {

        // base.SetTeamColor();
        //    Debug.Log("othernetId:" + netIdentity.netId);
          yield return new WaitForSeconds(3);

        if (NetworkedGameManager.Instance.IsInMyTeam(netId))
        {
            playerController.playerUIHandler.Change_TeamIndicator_Color("Ally");
           playerController.playerUIHandler.Change_TeamHealthBar_Color("Ally");

        }
        else
        {
            playerController.playerUIHandler.Change_TeamIndicator_Color("Enemy");
           playerController.playerUIHandler.Change_TeamHealthBar_Color("Enemy");

        }
      //  yield return new WaitForSeconds(3);
        yield return null;
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
