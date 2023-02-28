using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Image HealthBarFill;

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
        ChangeHealth(value);
        var dir = transform.position - camera.position;
        look = Quaternion.LookRotation(dir, Vector3.up);
    }

    public virtual void Update()
    {


        if (!initialized) return;
        UILook();


    }



    #region  Health Bar

    public void MakeHealthBarFull()
    {

        HealthBarFill.fillAmount = 1;


    }
 
    public void ChangeHealth(int health)
    {
        healthText.text = health.ToString();
        HealthBarFill.fillAmount = health / 100f;
    }
    #endregion

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
