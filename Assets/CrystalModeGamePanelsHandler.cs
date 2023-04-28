using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;

public class CrystalModeGamePanelsHandler : NetworkBehaviour
{

    private GemModeAnimateFloatOnCrystalInfoRect gemModeAnimateFloatOnCrystalInfoRect;
    private GemModeAnimateFloatOnCrystalStats gemModeAnimateFloatOnCrystalStats;
    private GemModeAnimateFloatOnCountDownTeamInfoPanel gemModeAnimateFloatOnCountDownTeamInfoPanel;
    private CrystalModeCountdown crystalModeCountDown;
    private CrystalModeStringSync crystalModeStringSync;
    [SyncVar(hook = nameof(HandleOpeningPanel))]
    public bool isOpeningPanelActive;

    [SyncVar(hook = nameof(HandleCountDownTeamInfoTextPanel))]
    public bool isCountDownTeamInfoTextPanelActive;


    [SyncVar(hook = nameof(HandleCountDownTextPanel))]
    public bool isCountDownTextPanelActive;


    [SyncVar(hook = nameof(HandleFinishPanel))]
    public bool isFinishPanelActive;

    public enum GamePanelStatus { None, CountDown }
    [SyncVar]
    public GamePanelStatus gamePanelStatus;

    private Coroutine CountDownPanelAnimationCoroutine;

    public UnityEvent onHandleOpeningPanelReadyToSpawnCrystalAction;
    public UnityEvent onHandleOpeningPanelReadyToPlayAction;
    //public UnityEvent onCountDownPanelActivation;



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
        if (TryGetComponent<GemModeAnimateFloatOnCountDownTeamInfoPanel>(out GemModeAnimateFloatOnCountDownTeamInfoPanel gemModeAnimateFloatOnCountDownTeamInfoPanel))
        {

            this.gemModeAnimateFloatOnCountDownTeamInfoPanel = gemModeAnimateFloatOnCountDownTeamInfoPanel;

        }
        if (TryGetComponent<CrystalModeCountdown>(out CrystalModeCountdown crystalModeCountDown))
        {

            this.crystalModeCountDown = crystalModeCountDown;

        }
        if (TryGetComponent<CrystalModeStringSync>(out CrystalModeStringSync crystalModeStringSync))
        {

            this.crystalModeStringSync = crystalModeStringSync;

        }



        this.gemModeAnimateFloatOnCountDownTeamInfoPanel.onCountdownFinishedAction.AddListener(StartToCountDown);
        this.gemModeAnimateFloatOnCountDownTeamInfoPanel.onAnimationFinishedBeforeExtraTimeAction.AddListener(CloseUpTeamCountDownPanel);


    }

    private void Update()
    {
        if (!isServer) { return; }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //    ActivateSequenceOnClients();
            StartCoroutine(DoOpeninPanelSequence());


        }
       


    }


    IEnumerator DoOpeninPanelSequence()
    {
        isOpeningPanelActive = true;
        yield return StartCoroutine(gemModeAnimateFloatOnCrystalInfoRect.AnimateCoroutine());
        isOpeningPanelActive = false;
        onHandleOpeningPanelReadyToSpawnCrystalAction.Invoke();
        yield return StartCoroutine(gemModeAnimateFloatOnCrystalStats.AnimateCoroutine());
        onHandleOpeningPanelReadyToPlayAction.Invoke();



    }


    public void StartToCountDownSequenceForATeam()
    {
        ChangeCurrentPanelStatusTo(GamePanelStatus.CountDown);
        isCountDownTeamInfoTextPanelActive = true;

        CountDownPanelAnimationCoroutine = StartCoroutine(gemModeAnimateFloatOnCountDownTeamInfoPanel.AnimateCoroutine());
    }

    public void CancelCountDownPanel()
    {
        if (CountDownPanelAnimationCoroutine != null)
        {
            ChangeCurrentPanelStatusTo(GamePanelStatus.None);
            isCountDownTeamInfoTextPanelActive = false;
            isCountDownTextPanelActive = false;
            StopCoroutine(CountDownPanelAnimationCoroutine);
            CountDownPanelAnimationCoroutine = null;
        }

    }


    public void StartToCountDown()
    {
        //  isCountDownTeamInfoTextPanelActive=false;
        isCountDownTextPanelActive = true;
        crystalModeCountDown.StartCountDown();
    }
    public void ChangeCurrentPanelStatusTo(GamePanelStatus panel)
    {
        gamePanelStatus = panel;
    }
    public void CloseUpTeamCountDownPanel()
    {
        isCountDownTeamInfoTextPanelActive = false;
        //isCountDownTextPanelActive = true;
        //crystalModeCountDown.StartCountDown();
    }
    public void HandleOpeningPanel(bool oldValue, bool newValue)
    {
        GameplayPanelUIManager.Instance.ActivateSequence(newValue);

    }
    public void HandleCountDownTeamInfoTextPanel(bool oldValue, bool newValue)
    {
        GameplayPanelUIManager.Instance.ActivateCountDownTeamInfoTextPanel(newValue);
    }
    public void HandleCountDownTextPanel(bool oldValue, bool newValue)
    {
        GameplayPanelUIManager.Instance.ActivateCountDownTextPanel(newValue);
    }
    public void HandleFinishPanel(bool oldValue, bool newValue)
    {
        GameplayPanelUIManager.Instance.ActivateFinishPanel(newValue);
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

    public void DoWinnableTeamCountDownText(string teamText)
    {
        crystalModeStringSync.ChangeWinnableTeamCountDownText(teamText);
    }
    public void DoWinnerTeamText(string teamText)
    {
        crystalModeStringSync.ChangeWinnerTeamText(teamText);
    }
}
