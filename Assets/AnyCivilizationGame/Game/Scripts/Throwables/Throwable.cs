using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class Throwable : NetworkBehaviour
{
    public float speed = .2f;

    private Coroutine throwingCoroutine;
    protected float movemenTime = 0;


    public string OwnerName = "";
    public uint OwnerNetId = 0;
    public uint RootNetId = 0;
    public int OwnerConnectionId = 0;

    #region new

    public enum ProjectileType { Linear, Parabolic }
    public ProjectileType projectileType;


    #endregion


    float height;
    float v0;
    float angle;
    float time;
    protected float currentThrowRateValue;




    public virtual void OnArrived()
    {
        //Inherited.

    }

    public virtual void OnObjectSpawn()
    {

        //Inherited.
    }


    public void Init(string ownerName, uint ownerNetId, int ownerConnectionId, uint RootId = 0, bool isRooted = false)
    {
        OwnerName = ownerName;
        OwnerNetId = ownerNetId;
        OwnerConnectionId = ownerConnectionId;

        //TODO :  RootNetId  o objeyi oluşturan root player'in netID sini getirir.  Takım olayları devreye girdiğinde bunu kaldırabiliriz. 
        if (isRooted)
        {
            RootNetId = ownerNetId;


        }
        else
        {
            RootNetId = RootId;
        }

    }
    public virtual void InitInfo(Vector3 dir)
    {


    }



    private void OnDestroy()
    {
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
    }


    public void Throw(Vector3 dir, float Range, float offSetZValue = 0, float offSetYValue = 0, float radialOffSet = 0f)
    {



        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);

        currentThrowRateValue = groundDir.magnitude;

        var targetPos = new Vector3(groundDir.magnitude * (Range) + offSetZValue, /*dir.y*/ -offSetYValue, 0);


        CalculateProjectile(targetPos);
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);




        throwingCoroutine = StartCoroutine(Coroutine_Movement(groundDir.normalized, v0, angle, time, speed, radialOffSet, offSetYValue));


    }



    public  IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity, float radialOffSet, float offSetYValue)
    {



        var startPos = transform.position + new Vector3(0, 0, 0);


        float startTime = 0;
        startTime = Time.time;
        float t = Time.time - startTime;


        var TotalXPos = (v0 * time * Mathf.Cos(angle));

        while (t < time)
        {

            t = (Time.time - startTime) * initialVelocity;

            t = Mathf.Min(t, time);

            float x = v0 * t * Mathf.Cos(angle);

            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            y = (float)Math.Round(y, 4);
            var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            transform.position = startPos + direction * x + upValue;


            yield return null;

        }

        //burası hedefe vardığında bir kez çalışır.
        OnArrived();

    }
    public Vector3 StartPosOffSet2(Vector3 dir, float radialOffSet)
    {

        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition = direction * (-radialOffSet);


        return startPosition;


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



    public void CalculateProjectile(Vector3 dir)
    {

        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);

        height = projectileType == ProjectileType.Parabolic ? (0 + new Vector3(dir.x, 0, dir.z).magnitude / 2f) : 0;

        height = Mathf.Max(0.01f, height);

        var dist = new Vector3(dir.x, 0, dir.z);


        CalculatePathWithHeight(dir.normalized * targetPos.magnitude, height, out v0, out angle, out time);



    }





}
