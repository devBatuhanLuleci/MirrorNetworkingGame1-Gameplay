using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ObjectUIHandler : MonoBehaviour
{
    #region UI fields
    [SerializeField]
    private Canvas canvas;

    //[SerializeField]
    //private Slider healthSlider;

    #region Health




    [SerializeField]
    protected Color targetColor;


    [SerializeField]
    private Transform HealthBar;

    [SerializeField]
    private Image HealthBarBackground;


    [SerializeField]
    protected Image HealthBarFill_Green;



    [SerializeField]
    private Image HealthBarFill_Red;


   
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


    public void Initialize()
    {
        camera = Camera.main.transform;
        initialized = true;
        //MakeHealthBarFull(maxHelath);
        //  ChangeHealth(value);

        cameraBackRotation = camera.transform.rotation * -Vector3.back;
        cameraDownRotation = camera.transform.rotation * -Vector3.down;
    }

    private void Awake()
    {

        //TODO : Remove this Awake
        // MakeHealthBarFull();
    }
    public virtual void Update()
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


        HealthBarFill_Green.fillAmount = healthRate;
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

            HealthBarFill_Red.fillAmount = value;


        }
        else
        {

            float duration = 1f;
            float angle = HealthBarFill_Red.fillAmount;
            tween = DOTween.To(() => angle, x => angle = x, value, duration).SetEase(Ease.InSine)
                .OnUpdate(() =>
                {

                    HealthBarFill_Red.fillAmount = angle;

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
