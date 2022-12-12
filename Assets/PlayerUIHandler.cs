using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class PlayerUIHandler : MonoBehaviour
{
    #region UI fields
    [Header("Setup")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Slider healthSlider;


    [SerializeField]
    private TMP_Text healthText;

    [SerializeField]
    private GameObject EnergyBarGeneral;

    [SerializeField]
    private Image EnergyBarGray;

    #endregion
    [SerializeField]
    private Image EnergyBarOrange;

    [SerializeField]
    private Image EnergyBarRed;

    #region ShakeParameters
    public bool isShaking = false;
    public float duration = 1f;
    public Vector3 strength=new Vector3(50,0,0);
    public int vibrato = 8;
    public float randomness = 0;



    [SerializeField]
    private TMP_Text nameText;
    #endregion

    #region Shoot projectile fields
    [Space]
    [Header("Projectile")]

    [SerializeField]
    private PlayerController playerController;

    public enum ProjectileType { Bullet, Bomb }
    public ProjectileType projectileType;



    float _step;



    private bool throwable = false;


    [HideInInspector]
    public float height;

    [HideInInspector]
    public float angle;
    [HideInInspector]
    public float v0;
    [HideInInspector]
    public float timeNew;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    public Vector3 groundDirection;



    //[HideInInspector]
    //public Vector3 targetPos;
    #endregion


    #region  Private Fields 
    private int maxHelath = 100;
    private bool initialized = false;

    #region  Components 
    private Transform camera;
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
    public void DisablePanel()
    {
        EnergyBarGeneral.SetActive(false);
    }

    Quaternion look;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            MakeEnergyBarsFull();


        }

        if (!initialized) return;
        UILook();
        // CalculateProjectile();



    }

    private void UILook()
    {
        if (canvas != null && camera != null)
        {
            canvas.transform.rotation = look;
        }
    }


    #region  Health Bar

    public void ChangeHealth(int health)
    {
        healthText.text = health.ToString();
        healthSlider.value = (float)maxHelath / (float)health;
    }
    #endregion


    #region Energy Bar


    public void MakeEnergyBarsFull()
    {

        EnergyBarOrange.fillAmount = 1;
        EnergyBarGray.fillAmount = 1;


    }
    public void ChangeEnergy(float fillAmount)
    {

        float perBarAmount = 0.333f;

        int value = Mathf.FloorToInt(fillAmount / perBarAmount);

        EnergyBarGray.fillAmount = fillAmount;

        if (fillAmount > perBarAmount * value)
            EnergyBarOrange.fillAmount = perBarAmount * value;




    }
    public void ShakeEnergyBar()
    {

        if (!isShaking)
        {
            EnergyBarGeneral.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true)
                .SetRelative()
                .SetEase(Ease.OutQuad)
                .OnStart(() => { isShaking = true; })
                .OnComplete(() => { isShaking = false; });

            //EnergyBarGeneral.transform.DOPunchScale(Vector3.one, duration, vibrato, 1);
            EnergyBarGeneral.transform.DOScale(0.2f,duration/2f).SetRelative().SetLoops(2, LoopType.Yoyo);

            EnergyBarRed.DOColor(new Color(255,0,0,25/255f), duration/2f).SetLoops(2,LoopType.Yoyo).SetEase(Ease.Linear);



        }



    }







    #endregion  
    #region  Projectile
    public void CalculateProjectile(Vector3 dir)
    {

        height = projectileType == ProjectileType.Bomb ? (dir.y + dir.magnitude / 2f) : 0;
        height = Mathf.Max(0.01f, height);
        var targetPos = new Vector3(dir.magnitude, dir.y, 0);

        // DrawPath(groundDirection.normalized, v0, angle, timeNew, _step);


        CalculatePathWithHeight(targetPos, height, out v0, out angle, out timeNew);




    }

    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        // step = Mathf.Max(0.01f, step);

        // line.positionCount = (int)(time / step) + 2;

        // int count = 0;

        // for (float i = 0; i < time; i += step)
        // {
        //     float x = v0 * i * Mathf.Cos(angle);
        //     float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);

        //     var FirstUpValue = projectileType == ProjectileType.Bomb ? (Vector3.up * y) : Vector3.zero;

        //    // line.SetPosition(count, firePoint.position + direction * x + FirstUpValue);

        //     count++;

        // }

        // float xFinal = v0 * time * Mathf.Cos(angle);
        //// float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        // float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

        // var upValue = projectileType == ProjectileType.Bomb ? (Vector3.up * yFinal) : Vector3.zero;
        // line.SetPosition(count, firePoint.position + direction * xFinal + upValue);


    }

    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }
    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = (tplus > tmin) ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);


        v0 = b / Mathf.Sin(angle);



    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));




    }
    #endregion

}
