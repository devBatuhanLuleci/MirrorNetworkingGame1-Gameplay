using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ObjectUIHandler : MonoBehaviour
{
    #region UI fields
    [SerializeField]
    private Canvas canvas;

    //[SerializeField]
    //private Slider healthSlider;

    #region Health

    public enum ObjectBehaviourStates { Me, Ally, Enemy }
    [SerializeField]
    protected ObjectBehaviourStates _state; //this holds the actual value 
    public ObjectBehaviourStates State
    { //this is public and accessible, and should be used to change "State"
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            OnPlayerBehaviourStateChanged();
            Debug.Log("Enum just got changed to: " + _state);
        }
    }
    [SerializeField]
    protected Sprite Me_HealthBar_BG;

    [SerializeField]
    protected Sprite Me_HealthBar_Fill;

    [SerializeField]
    protected Sprite Enemy_Or_Ally_HealthBar_BG;

    [SerializeField]
    protected Sprite Enemy_Or_Ally_HealthBar_Fill;


    [SerializeField]
    protected Color HealthBarFill_Front_My_Color;

    [SerializeField]
    protected Color HealthBarFill_Front_Ally_Color;

    [SerializeField]
    protected Color HealthBarFill_Front_Enemy_Color;


    [Space(5)]

    [SerializeField]
    protected Color HealthBarFill_Back_My_Color;

    [SerializeField]
    protected Color HealthBarFill_Back_Ally_Color;

    [SerializeField]
    protected Color HealthBarFill_Back_Enemy_Color;



    [SerializeField]
    private Transform HealthBar;

    [SerializeField]
    private Image HealthBarBackground;


    [SerializeField]
    protected Image HealthBarFill_Front;



    [SerializeField]
    private Image HealthBarFill_Back;


   
    #endregion

    [SerializeField]
    private TMP_Text healthText;
    #endregion

    #region  Private Fields 
    private Tween tween;


    private bool initialized = false;

    #region  Components 
    private Transform camera;


    Vector3 cameraBackRotation;
    Vector3 cameraDownRotation;


    #endregion
    #endregion


    #region Indicator

    [SerializeField]
    private TeamIndicatorHandler teamIndicatorHandler;


    #endregion

    public void Initialize()
    {
        camera = Camera.main.transform;
        initialized = true;
        //MakeHealthBarFull(maxHelath);
        //  ChangeHealth(value);

        cameraBackRotation = camera.transform.rotation * -Vector3.back;
        cameraDownRotation = camera.transform.rotation * -Vector3.down;
    }
    public void Change_TeamHealthBar_Color(string teamType)
    {

        ObjectBehaviourStates currentState = (ObjectBehaviourStates)Enum.Parse(typeof(ObjectBehaviourStates), teamType);
        State = currentState;

    }
    public void Change_TeamIndicator_Color(string teamType)
    {

        teamIndicatorHandler.ChangeTeamIndicatorType(teamType);

    }

    public virtual void OnPlayerBehaviourStateChanged()
    {
        switch (_state)
        {
            case ObjectBehaviourStates.Me:

                HealthBarBackground.sprite = Me_HealthBar_BG;
                HealthBarFill_Front.sprite = Me_HealthBar_Fill;
                HealthBarFill_Back.sprite = Me_HealthBar_Fill;

                HealthBarFill_Front.color = HealthBarFill_Front_My_Color;
                HealthBarFill_Back.color = HealthBarFill_Back_My_Color;


                break;
            case ObjectBehaviourStates.Ally:

                HealthBarBackground.sprite = Enemy_Or_Ally_HealthBar_BG;
                HealthBarFill_Front.sprite = Enemy_Or_Ally_HealthBar_Fill;
                HealthBarFill_Back.sprite = Enemy_Or_Ally_HealthBar_Fill;

                HealthBarFill_Front.color = HealthBarFill_Front_Ally_Color;
                HealthBarFill_Back.color = HealthBarFill_Back_Ally_Color;


                break;
            case ObjectBehaviourStates.Enemy:

                HealthBarBackground.sprite = Enemy_Or_Ally_HealthBar_BG;
                HealthBarFill_Front.sprite = Enemy_Or_Ally_HealthBar_Fill;
                HealthBarFill_Back.sprite = Enemy_Or_Ally_HealthBar_Fill;


                HealthBarFill_Front.color = HealthBarFill_Front_Enemy_Color;
                HealthBarFill_Back.color = HealthBarFill_Back_Enemy_Color;


                break;
            default:
                break;
        }
        
        HealthBarBackground.SetNativeSize();
        HealthBarFill_Front.SetNativeSize();
        HealthBarFill_Back.SetNativeSize();
    }

    public virtual void Awake()
    {

    }
    public virtual void LateUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    maxHelath -= 10;
        //    ChangeHealth(maxHelath);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartCoroutine(change());

        //}
        if (!initialized) return;
        UILook();



    }
    //IEnumerator change()
    //{


    //    while (maxHelath < 100)
    //    {
    //        maxHelath += 20;
    //        yield return new WaitForSeconds(0.5f);


    //        if (maxHelath > 100)
    //        {
    //            maxHelath = 100;
    //        }
    //        ChangeHealth(maxHelath, true);
    //    }

    //    yield return null;
    //}



    #region  Health Bar

   

    public void ChangeHealth(int health)
    {

       
        healthText.text = health.ToString();
      

       

     
    }
    public void ChangeHealthRate( float healthRate, bool isIncreasing = false)
    {


        HealthBarFill_Front.fillAmount = healthRate;
        //HealthBarFill_Green.color = HealthBarFill_Gradient.Evaluate(health / 100f);
        // HealthBarFill_Green.DOGradientColor(HealthBarFill_Gradient2, .5f).SetLoops(2,LoopType.Yoyo);


        Animate_HealthBarFill_Red(healthRate, isIncreasing);

        //    Debug.Log("isinitlized: " + IsInitilized);




    }
    #endregion


    public void Animate_HealthBarFill_Red(float value, bool isIncreasing = false)
    {
        if (tween != null)
        {
            tween.Kill();

        }


        if (isIncreasing)
        {

            HealthBarFill_Back.fillAmount = value;


        }
        else
        {

            float duration = 1f;
            float angle = HealthBarFill_Back.fillAmount;
            tween = DOTween.To(() => angle, x => angle = x, value, duration).SetEase(Ease.InSine)
                .OnUpdate(() =>
                {

                    HealthBarFill_Back.fillAmount = angle;

                }
                )

                ;
        }

        // HealthBarFill_Red.fillAmount

    }
    public void Animate_HealthBar_On_Hit()
    {

        //  float duration = 1f;
        //HealthBarFill_Blur_Effect.DOFade(0f, .5f).SetLoops(1, LoopType.Yoyo).From(.2f);
        HealthBar.DOScale(1f, .5f).SetLoops(1, LoopType.Yoyo).From(1.25f);
        // HealthBarFill_Red.fillAmount

    }
  
    private void UILook()
    {
        if (canvas != null && camera != null)
        {
     //       canvas.transform.rotation = look;
            canvas.transform.LookAt(canvas.transform.position + cameraBackRotation, cameraDownRotation);
        }
      

    }
    
    public virtual void DisablePanel()
    {


    }
}
