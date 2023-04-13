using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class CrystalModeUIOpeningHandler : NetworkBehaviour
{

    private  GemModeAnimateFloatOnCrystalInfoRect gemModeAnimateFloatOnCrystalInfoRect;



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

    }

    private void Update()
    {
        if (!isServer) { return; }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(gemModeAnimateFloatOnCrystalInfoRect.AnimateCoroutine());
        }


      
    }
    public void CreateCrystalModeCanvas()
    {
    

        var prefab = Resources.Load<CrystalModeGamePlayCanvasUIController>("CrystalModeGameplayCanvas"/*nameof(CrystalModeGamePlayCanvasUIController*/);
        var crystalModeCanvas = Instantiate(prefab);

        InitCrystalCanvas(crystalModeCanvas);
        Debug.LogError("CrystalModeGameplayCanvas spawned.");

    }
    public void InitCrystalCanvas(CrystalModeGamePlayCanvasUIController crystalModeCanvas)
    {

        GameplayPanelUIManager.Instance.Init_CrystalModeGameplayCanvas(crystalModeCanvas);


    }
}
