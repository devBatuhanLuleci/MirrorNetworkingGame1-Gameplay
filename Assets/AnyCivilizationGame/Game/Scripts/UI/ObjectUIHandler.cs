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
    private Image HealthBarBackground;


    [SerializeField]
    private Image HealthBarFill_Green;



    [SerializeField]
    private Image HealthBarFill_Red;


    #endregion

    [SerializeField]
    private TMP_Text healthText;
    #endregion

    #region  Private Fields 
    private int maxHelath = 100;
    private bool initialized = false;

    #region  Components 
    private Transform camera;


    Quaternion look;


    #endregion
    #endregion


    public void Initialize(int value)
    {
        camera = Camera.main.transform;
        initialized = true;
        maxHelath = value;
        MakeHealthBarFull(maxHelath);
        //ChangeHealth(value);
        var dir = transform.position - camera.position;
        look = Quaternion.LookRotation(dir, Vector3.up);
    }

    private void Awake()
    {

        //TODO : Remove this Awake
        MakeHealthBarFull();
    }
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            maxHelath -= 20;
            ChangeHealth(maxHelath);
        }

        if (!initialized) return;
        UILook();



    }
    public void GetDamage(int damage)
    {
        //TODO : Remove this GetDamage
        maxHelath -= damage;
    }



    #region  Health Bar

    public void MakeHealthBarFull(int value = 100)
    {

        HealthBarFill_Green.fillAmount = maxHelath / 100f;
        HealthBarFill_Red.fillAmount = maxHelath / 100f;


    }


    public void ChangeHealth(int health)
    {
        healthText.text = health.ToString();
        HealthBarFill_Green.fillAmount = health / 100f;

        Animate_HealthBarFill_Red(health / 100f);
    }

    #endregion
    public void Animate_HealthBarFill_Red(float value)
    {

        float duration = 1f;
        float angle = HealthBarFill_Red.fillAmount;
        DOTween.To(() => angle, x => angle = x, value, duration).SetEase(Ease.InSine)
            .OnUpdate(() =>
            {
                Debug.Log("angle: "+ angle);
                HealthBarFill_Red.fillAmount = angle;

            }
            )
            ;

      // HealthBarFill_Red.fillAmount

    }
    private void UILook()
    {
        if (canvas != null && camera != null)
        {
            canvas.transform.rotation = look;
        }


    }

    public virtual void DisablePanel()
    {


    }
}
