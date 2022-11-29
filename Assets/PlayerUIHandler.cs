using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [HideInInspector]
    public Vector3 targetPos;
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


    Quaternion look;
    private void Update()
    {
        if (!initialized) return;
        UILook();
        CalculateProjectile();
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

    #region  Projectile
    public void CalculateProjectile()
    {
        height = projectileType == ProjectileType.Bomb ? (targetPos.y + targetPos.magnitude / 2f) : 0;
        height = Mathf.Max(0.01f, height);

        CalculatePathWithHeight(targetPos, height, out v0, out angle, out timeNew);


        DrawPath(groundDirection.normalized, v0, angle, timeNew, _step);


        direction = playerController.TargetPoint.position - playerController.FirePoint.position;
        groundDirection = new Vector3(direction.x, 0, direction.z);
        targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);



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
