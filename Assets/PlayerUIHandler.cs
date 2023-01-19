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
    public Vector3 strength = new Vector3(50, 0, 0);
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

    public enum ProjectileType { StaticBullet, Bomb }
    public ProjectileType projectileType;


    public SpriteRenderer BombIndicator;
    public LineRenderer AttackBasicIndicator;
    public LayerMask LayerMask;




    public float bulletSpeed = 0.1f;
    public float radialOffset = .75f;
    public float minAttackLimit = 0.1f;

    float height;
    float v0;
    float angle;
    float timeNew;
    float step = .1f;






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

            EnergyBarGeneral.transform.DOScale(0.2f, duration / 2f).SetRelative().SetLoops(2, LoopType.Yoyo);

            EnergyBarRed.DOColor(new Color(255, 0, 0, 25 / 255f), duration / 2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);



        }



    }

    public void RotateProjector(Transform player, Vector2 lookPos, Transform target, RaycastHit hit, float trailDistance)
    {
        #region Old
        //AttackBasicIndicator.SetPosition(0, player.transform.position + (lookPos.normalized) / 4f);
        //var hitOffSet = (Vector3.up * 0.5f);

        //if (Physics.Raycast(player.transform.position + hitOffSet, (lookPos.normalized), out hit, trailDistance, LayerMask))
        //{
        //    AttackBasicIndicator.SetPosition(1, hit.point - hitOffSet);
        //}
        //else
        //{
        //    AttackBasicIndicator.SetPosition(1, player.transform.position + ((lookPos.normalized) * trailDistance));
        //}
        #endregion

        #region new


        Vector3 dir = new Vector3(lookPos.x, 0, lookPos.y);


        Vector3 targetPos = Vector3.zero;


        var hitOffSet = (Vector3.up * 0.5f);
        //   Debug.DrawRay(player.transform.position + hitOffSet, new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * Range, Color.green, .1f);

        if (Physics.Raycast(player.transform.position + hitOffSet, new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y), out hit,playerController.Range, LayerMask))
        {

            var dist = (hit.point - (player.transform.position + StartPosOffSet(dir.normalized))).magnitude;
            Debug.DrawRay(player.transform.position + hitOffSet + StartPosOffSet(dir.normalized), new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * dist, Color.green, .1f);
            var bombPos = Vector3.zero;
         
            switch (projectileType)
            {
                case ProjectileType.StaticBullet:
                    //  targetPos = new Vector3(dir.normalized.magnitude* new Vector3(hit.point.x, 0, hit.point.z).magnitude, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);
                    //new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * dist
                    targetPos = new Vector3(new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y).magnitude * dist, -playerController.BulletSpawnPoints[0].spawnPoint.y / 2f, 0);

                    break;

                case ProjectileType.Bomb:


                    targetPos = new Vector3(dir.magnitude * playerController.Range, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);

                    // targetPos = new Vector3(dir.normalized.magnitude * new Vector3(hit.point.x, 0, hit.point.z).magnitude, -playerController.BulletSpawnPoints[0].spawnPoint.y / 2f, 0);
                    if (!BombIndicator.gameObject.activeSelf)
                    {
                        BombIndicator.gameObject.SetActive(true);
                    }


                        BombIndicator.transform.position = hit.point;
                    break;
                default:
                    break;
            }
            //  Debug.DrawRay(player.transform.position + hitOffSet, new Vector3(hit.point.x, 0, hit.point.z), Color.green, .1f);
        }

        else
        {
           
            Debug.DrawRay(player.transform.position + hitOffSet +StartPosOffSet(dir.normalized), new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * playerController.Range, Color.red, .1f);

            switch (projectileType)
            {
                case ProjectileType.StaticBullet:
                  //  Debug.Log(dir.normalized);
                    targetPos = new Vector3(dir.normalized.magnitude * playerController.Range, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);

                    break;
                case ProjectileType.Bomb:

                   

                    targetPos = new Vector3(dir.magnitude * playerController.Range, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);
                    if (!BombIndicator.gameObject.activeSelf)
                    {
                        BombIndicator.gameObject.SetActive(true);

                    }

                    break;

                default:
                    break;
            }

        }

       

        CalculateProjectile(targetPos);
        DrawPath(dir.normalized, player, v0, angle, timeNew, step);




        #endregion






    }


    private void DrawPath(Vector3 direction, Transform player, float v0, float angle, float time, float step)
    {
        var yOffSet = 0f;
        //var startPos = playerController.BulletSpawnPoints[0].spawnPoint + StartPosOffSet(direction);
        switch (projectileType)
        {
            case ProjectileType.StaticBullet:
                yOffSet = 0f;
                break;
            case ProjectileType.Bomb:
                yOffSet = playerController.BulletSpawnPoints[0].spawnPoint.y;
                break;
            default:
                break;
        } 
        var startPos = player.transform.position + new Vector3(0, yOffSet, 0) + StartPosOffSet(direction);
        step = Mathf.Max(0.01f, step);

        AttackBasicIndicator.positionCount = (int)(time / step) + 2;

        int count = 0;

        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);

            var FirstUpValue = projectileType == ProjectileType.Bomb ? (Vector3.up * y) : Vector3.zero;

            AttackBasicIndicator.SetPosition(count, startPos + direction * x + FirstUpValue);

            count++;

        }

        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

        var upValue = projectileType == ProjectileType.Bomb ? (Vector3.up * yFinal) : Vector3.zero;
        BombIndicator.transform.position = startPos + (direction * xFinal + upValue);

        AttackBasicIndicator.SetPosition(count, startPos + (direction * xFinal + upValue));


    }
    public void ResetProjector()
    {
        AttackBasicIndicator.positionCount = 0;
        BombIndicator.gameObject.SetActive(false);

    }


    public void CalculateProjectile(Vector3 dir)
    {

        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);

        height = projectileType == ProjectileType.Bomb ? (dir.y + dir.magnitude / 2f) : 0;
        height = Mathf.Max(0.01f, height);

        var dist = new Vector3(dir.x, 0, dir.z).magnitude;


        if (targetPos.x < playerController.ClampedAttackJoystickOffset)
        {
            //   AttackBasicIndicator.enabled = false;


        }
        else
        {
            //  AttackBasicIndicator.enabled = true;

            //if (dist <= playerController.Range)
            //{
                //   Debug.Log("lineRange: " + (dir.normalized * targetPos.magnitude /*- StartPosOffSet(targetPos)*/));
               
                CalculatePathWithHeight(dir.normalized * targetPos.magnitude /*- StartPosOffSet(targetPos)*/, height, out v0, out angle, out timeNew);

            //}

        }




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
    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    public Vector3 StartPosOffSet(Vector3 dir)
    {

        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition = /*+offsetVector * localHorizontalOffset*/  direction * radialOffset;


        return startPosition;

    }

    #endregion

}
