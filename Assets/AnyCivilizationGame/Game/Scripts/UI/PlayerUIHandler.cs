using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class PlayerUIHandler : ObjectUIHandler
{


    #region Energy
    [SerializeField]
    private GameObject EnergyBarGeneral;

    [SerializeField]
    private Image EnergyBarGray;

    [SerializeField]
    private Image EnergyBarOrange;

    [SerializeField]
    private Image EnergyBarRed;


    #endregion


    #region Ulti
    [SerializeField]
    private Image ULtimateFillImage;

    #endregion


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

    public enum ProjectileType { Lineer, Parabolic }
    public ProjectileType projectileType;


    public SpriteRenderer BombIndicator;
    public LineRenderer AttackBasicIndicator;
    public LineRenderer AttackBasicIndicator2;
    public LayerMask LayerMask;



    #region  Private Fields 

    public float bulletSpeed = 0.1f;
    public float radialOffset = .75f;
    public float minAttackLimit = 0.1f;

    float height;
    float height2;
    float v0;
    float angle;
    float timeNew;
    float step = .1f;

    float v02;
    float angle2;
    float timeNew2;
    float step2 = .1f;

    private Coroutine throwingCoroutine;
    Vector3 tempDir;

    #endregion



    #region  Components 
    private Transform camera;
    #endregion
    #endregion

    public override void DisablePanel()
    {
        base.DisablePanel();
        EnergyBarGeneral.SetActive(false);
    }

    Quaternion look;
    public override void Update()
    {
        base.Update();



        if (Input.GetKeyDown(KeyCode.Space))
        {

            Throw(tempDir);

        }

    }



  


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
    public void ChangeUltimateFillRate(float fillAmount)
    {

        //  float perBarAmount = 0.333f;

        // int value = Mathf.FloorToInt(fillAmount / perBarAmount);

        ULtimateFillImage.fillAmount = (fillAmount / 100f);

        //  if (fillAmount > perBarAmount * value)
        //    EnergyBarOrange.fillAmount = perBarAmount * value;





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


        #region new



        Vector3 dir = new Vector3(lookPos.x, 0, lookPos.y);


        Vector3 targetPos = Vector3.zero;
        Vector3 targetPos2 = Vector3.zero;


        var hitOffSet = (Vector3.up * 0.5f);

        if (Physics.Raycast(player.transform.position + hitOffSet, new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y), out hit, playerController.Range, LayerMask))
        {

            var dist = (hit.point - (player.transform.position + StartPosOffSet(dir.normalized))).magnitude;
            //  Debug.DrawRay(player.transform.position + hitOffSet + StartPosOffSet(dir.normalized), new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * dist, Color.green, .1f);
            var bombPos = Vector3.zero;

            switch (projectileType)
            {
                case ProjectileType.Lineer:
                    //  targetPos = new Vector3(dir.normalized.magnitude* new Vector3(hit.point.x, 0, hit.point.z).magnitude, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);
                    //new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * dist
                    targetPos = new Vector3(new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y).magnitude * dist, -playerController.BulletSpawnPoints[0].spawnPoint.y / 2f, 0);

                    break;

                case ProjectileType.Parabolic:

                    float dist2 = Mathf.Abs(playerController.BulletSpawnPoints[2].spawnPoint.z - radialOffset);

                    targetPos = new Vector3(dir.magnitude * playerController.Range, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);
                    targetPos2 = new Vector3(dir.magnitude * (playerController.Range) + (dist2), -playerController.BulletSpawnPoints[2].spawnPoint.y, 0);

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

            // Debug.DrawRay(player.transform.position + hitOffSet +StartPosOffSet(dir.normalized), new Vector3(lookPos.normalized.x, 0, lookPos.normalized.y) * playerController.Range, Color.red, .1f);

            switch (projectileType)
            {
                case ProjectileType.Lineer:
                    //  Debug.Log(dir.normalized);
                    targetPos = new Vector3(dir.normalized.magnitude * playerController.Range, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);

                    break;
                case ProjectileType.Parabolic:


                    float dist = Mathf.Abs(playerController.BulletSpawnPoints[2].spawnPoint.z - radialOffset);
                    targetPos = new Vector3(dir.magnitude * playerController.Range, -playerController.BulletSpawnPoints[0].spawnPoint.y, 0);
                    targetPos2 = new Vector3(dir.magnitude * (playerController.Range) + (dist), -playerController.BulletSpawnPoints[2].spawnPoint.y, 0);



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


        CalculateProjectile2(targetPos2);
        // Debug.Log("targetPos: " + dir.normalized);

        tempDir = dir.normalized;
        //  Debug.Log("PlayerUI dir normalized: " + dir.normalized);

        DrawPath(dir.normalized, player, v0, angle, timeNew, step);





        DrawPath2(dir.normalized, player, v02, angle2, timeNew2, step2);




        #endregion






    }


    public void Throw(Vector3 dir)
    {



        //TODO : Burada kaldın space 'e basınca topun hareketine bak.
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);





        //  throwingCoroutine = StartCoroutine(Coroutine_Movement(dir, v0, angle, timeNew, bulletSpeed));
        throwingCoroutine = StartCoroutine(Coroutine_Movement2(dir, v02, angle2, timeNew2, bulletSpeed));


    }
    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity)
    {

        var yOffSet = 0f;
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = Vector3.one * .5f;
        go.GetComponent<Renderer>().material.color = Color.red;

        Vector3 startPos;
        switch (projectileType)
        {
            case ProjectileType.Lineer:
                yOffSet = 0f;
                break;
            case ProjectileType.Parabolic:
                yOffSet = playerController.BulletSpawnPoints[0].spawnPoint.y;
                break;
            default:
                break;
        }
        //ar startPos = transform.position + new Vector3(0,/* yOffSet*/0, 0) + StartPosOffSet2(direction);

        startPos = transform.position + new Vector3(0, yOffSet, 0) + StartPosOffSet(direction);
        //  var FirePoint = transform.position + StartPosOffSet(direction);

        float t = 0;
        float a = 0f;
        // Debug.Log(time / (initialVelocity ));
        // Debug.Log(BulletObj.transform.name);
        while (t < time)
        {

            float x = v0 * t * Mathf.Cos(angle);
            a = x;
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);

            var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            go.transform.position = startPos + direction * x + upValue;



            t += Time.fixedDeltaTime * (initialVelocity);



            yield return null;

        }
        //Debug.Log("height2:" + height2);
        //Debug.Log("VO2:" + v02);
        //Debug.Log("angle2:" + angle2);
        //Debug.Log("timeNew2:" + timeNew2);
        //  Debug.Log("posy: " + (v0 * t));

        //  Destroy(go, 1f);
        //burası hedefe vardığında bir kez çalışır.
        //   OnArrived();
    }


    IEnumerator Coroutine_Movement2(Vector3 direction, float v0, float angle, float time, float initialVelocity)
    {

        //    Debug.Log("bana gelen : " + direction);


        var yOffSet = 0f;
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = Vector3.one * .5f;
        go.GetComponent<Renderer>().material.color = Color.red;

        switch (projectileType)
        {
            case ProjectileType.Lineer:
                yOffSet = 0f;
                break;
            case ProjectileType.Parabolic:
                yOffSet = playerController.BulletSpawnPoints[2].spawnPoint.y;

                break;
            default:
                break;
        }
        var startPos = transform.position + new Vector3(0, yOffSet, 0) + StartPosOffSet2(direction);
        // Debug.Log("bana gelen : " + (transform.position + new Vector3(0, yOffSet, 0)));
        Debug.Log("bana gelen : " + StartPosOffSet2(direction));


        //  var FirePoint = transform.position + StartPosOffSet(direction);

        float t = 0;
        float a = 0f;
        // Debug.Log(time / (initialVelocity ));
        // Debug.Log(BulletObj.transform.name);
        while (t < time)
        {

            float x = v0 * t * Mathf.Cos(angle);
            a = x;
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);

            var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            go.transform.position = startPos + direction * x + upValue;



            t += Time.fixedDeltaTime * (initialVelocity);



            yield return null;

        }
        //Debug.Log("height2:" + height2);
        //Debug.Log("VO2:" + v02);
        //Debug.Log("angle2:" + angle2);
        //Debug.Log("timeNew2:" + timeNew2);
        //  Debug.Log("posy: " + (v0 * t));

        //  Destroy(go, 1f);
        //burası hedefe vardığında bir kez çalışır.
        //   OnArrived();
    }

    private void DrawPath(Vector3 direction, Transform player, float v0, float angle, float time, float step)
    {
        var yOffSet = 0f;
        //var startPos = playerController.BulletSpawnPoints[0].spawnPoint + StartPosOffSet(direction);
        switch (projectileType)
        {
            case ProjectileType.Lineer:
                yOffSet = 0f;
                break;
            case ProjectileType.Parabolic:
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

            var FirstUpValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            AttackBasicIndicator.SetPosition(count, startPos + direction * x + FirstUpValue);

            count++;

        }

        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

        var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * yFinal) : Vector3.zero;
        BombIndicator.transform.position = startPos + (direction * xFinal + upValue);

        AttackBasicIndicator.SetPosition(count, startPos + (direction * xFinal + upValue));


    }
    private void DrawPath2(Vector3 direction, Transform player, float v0, float angle, float time, float step)
    {
        var yOffSet = 0f;

        //var startPos = playerController.BulletSpawnPoints[0].spawnPoint + StartPosOffSet(direction);


        switch (projectileType)
        {
            case ProjectileType.Lineer:
                yOffSet = 0f;
                break;
            case ProjectileType.Parabolic:
                yOffSet = playerController.BulletSpawnPoints[2].spawnPoint.y;

                break;
            default:
                break;
        }
        var startPos = player.transform.position + new Vector3(0, yOffSet, 0) + StartPosOffSet2(direction);
        step = Mathf.Max(0.01f, step);


        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //go.transform.localScale = Vector3.one * .3f;
        //go.GetComponent<Renderer>().material.color = Color.blue;
        //go.layer = 12;
        //float xFinal2 = v0 * time * Mathf.Cos(angle);
        //float yFinal2 = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        //var upValue2 = projectileType == ProjectileType.Parabolic ? (Vector3.up * yFinal2) : Vector3.zero;

        //go.transform.position = startPos + (direction * xFinal2 + upValue2);


        AttackBasicIndicator2.positionCount = (int)(time / step) + 2;

        int count = 0;

        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);

            var FirstUpValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            AttackBasicIndicator2.SetPosition(count, startPos + direction * x + FirstUpValue);

            count++;

        }

        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

        var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * yFinal) : Vector3.zero;

        AttackBasicIndicator2.SetPosition(count, startPos + (direction * xFinal + upValue));


    }

    public void ResetProjector()
    {
        AttackBasicIndicator.positionCount = 0;
        AttackBasicIndicator2.positionCount = 0;
        BombIndicator.gameObject.SetActive(false);

    }


    public void CalculateProjectile(Vector3 dir)
    {

        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);

        height = projectileType == ProjectileType.Parabolic ? (dir.y + dir.magnitude / 2f) : 0;
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
    public void CalculateProjectile2(Vector3 dir)
    {

        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);
        height2 = projectileType == ProjectileType.Parabolic ? (0 + new Vector3(dir.x, 0, dir.z).magnitude / 2f) : 0;



        //height2 = projectileType == ProjectileType.Parabolic ? (dir.y + dir.magnitude / 2f) : 0;

        height2 = Mathf.Max(0.01f, height2);





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


            CalculatePathWithHeight(dir.normalized * targetPos.magnitude /*- StartPosOffSet2(targetPos)*4*/, height2, out v02, out angle2, out timeNew2);

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
    public Vector3 StartPosOffSet2(Vector3 dir)
    {

        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition = /*+offsetVector * localHorizontalOffset*/  direction * playerController.BulletSpawnPoints[2].spawnPoint.z;


        return startPosition;

    }

    #endregion

}
