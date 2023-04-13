using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class CrystalModeUIOpeningHandler : NetworkBehaviour
{

    private GemModeAnimateFloatOnCrystalInfoRect gemModeAnimateFloatOnCrystalInfoRect;
    private GemModeAnimateFloatOnCrystalStats gemModeAnimateFloatOnCrystalStats;

    [SyncVar(hook = nameof(HandleOpeningPanel))]
    public bool isPanelActive;




    private void Awake()
    {
        Init();

    }

    public void Init()
    {


        if (TryGetComponent<GemModeAnimateFloatOnCrystalInfoRect>(out GemModeAnimateFloatOnCrystalInfoRect gemModeAnimateFloatOnCrystalInfoRect))
        {

            this.gemModeAnimateFloatOnCrystalInfoRect = gemModeAnimateFloatOnCrystalInfoRect;

        }
        if (TryGetComponent<GemModeAnimateFloatOnCrystalStats>(out GemModeAnimateFloatOnCrystalStats gemModeAnimateFloatOnCrystalStats))
        {

            this.gemModeAnimateFloatOnCrystalStats = gemModeAnimateFloatOnCrystalStats;

        }
    }

    private void Update()
    {
        if (!isServer) { return; }


        if (Input.GetKeyDown(KeyCode.Space))
        {
        //    ActivateSequenceOnClients();
            StartCoroutine(sequence());


        }



    }

    IEnumerator sequence()
    {
        isPanelActive = true;
        yield return StartCoroutine(gemModeAnimateFloatOnCrystalInfoRect.AnimateCoroutine());
        yield return StartCoroutine(gemModeAnimateFloatOnCrystalStats.AnimateCoroutine());



    }
    public void HandleOpeningPanel(bool oldValue, bool newValue)
    {
        GameplayPanelUIManager.Instance.ActivateSequence(newValue);

    }
    //[ClientRpc]
    //public void ActivateSequenceOnClients()
    //{

    //    GameplayPanelUIManager.Instance.ActivateSequence();
    //}
    public void CreateCrystalModeCanvas()
    {


        var prefab = Resources.Load<CrystalModeGamePlayCanvasUIController>("CrystalModeGameplayCanvas"/*nameof(CrystalModeGamePlayCanvasUIController*/);
        var crystalModeCanvas = Instantiate(prefab);
        InitCrystalCanvas(crystalModeCanvas);
        Debug.LogError("CrystalModeGameplayCanvas spawned.");

    }
    public void InitCrystalCanvas(CrystalModeGamePlayCanvasUIController crystalModeCanvas)
    {
        crystalModeCanvas.Init();

        GameplayPanelUIManager.Instance.Init_CrystalModeGameplayCanvas(crystalModeCanvas);


    }
}
