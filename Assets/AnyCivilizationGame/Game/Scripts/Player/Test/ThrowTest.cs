using SimpleInputNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTest : MonoBehaviour
{

    public LineRenderer line;
    public Transform firePoint;
    public Transform targetPoint;
    public Joystick attackJoystick;
    private Coroutine throwingCoroutine;
    public float Range = 5;
    public float bulletSpeed = 0.1f;
    public float radialOffset = .75f;
    public float minAttackLimit = 0.1f;
    public GameObject throwableObj;
    public enum ProjectileType { Bullet, Bomb }
    public ProjectileType projectileType;

    float height;
    float v0;
    float angle;
    float timeNew;
    float step = .1f;


    public enum Type { Type1, Type2 }
    public Type type;
    public virtual void OnArrived()
    {

        //  throwableObj.SetActive(false);

        //  Debug.Log("we arrived.");
    }



    private void OnDestroy()
    {
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
    }

    private void Update()
    {
        AttackProjectileHandler();





    }
    public void AttackProjectileHandler()
    {
        if (type == Type.Type1)
        {


            Vector3 dist = targetPoint.position - firePoint.position;


            Vector3 dir = new Vector3(dist.x, 0, dist.z);

            Vector3 targetPos = new Vector3(dir.magnitude, -firePoint.position.y, 0);


            CalculateProjectile(targetPos);
            DrawPath(dir.normalized, v0, angle, timeNew, step);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                throwableObj.SetActive(true);
                Throw(dir.normalized);

            }
        }
        else if (type == Type.Type2)
        {


            // Vector3 dist = targetPoint.position - firePoint.position;


            Vector3 dir = new Vector3(attackJoystick.Value.x, 0, attackJoystick.Value.y);

            Vector3 targetPos = new Vector3(dir.magnitude * Range, -firePoint.position.y, 0);


            CalculateProjectile(targetPos);
            DrawPath(dir.normalized, v0, angle, timeNew, step);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                throwableObj.SetActive(true);
               if(throwableObj.TryGetComponent(out Animator anim))
                {
                    Debug.Log("hmm");
                    anim.SetTrigger("Setup");

                }
                Throw(dir.normalized);

            }
        }
    }

    private void MainAttack()
    {
        var targetingDirection = attackJoystick.Value;
        var held = attackJoystick.joystickHeld;


    }

    //private void OnDrawGizmos()
    //{

    //    //dir = (targetPoint.position - transform.position).normalized;
    //    var targetingDirection = joystick.Value;
    //    dir = targetingDirection;
    //    var groundDir = new Vector3(dir.x, -1f, dir.y);
    //    CalculateProjectile(groundDir);
    //    DrawPath(groundDir, v0, angle, timeNew, step);
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Throw(groundDir);

    //    }

    //}



    public void Throw(Vector3 dir)
    {


        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);

        throwingCoroutine = StartCoroutine(Coroutine_Movement(dir, v0, angle, timeNew, bulletSpeed));


    }



    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity)
    {

        var FirePoint = firePoint.position + StartPosOffSet(direction);
        float startTime = 0;
        startTime = Time.time;
        float t = Time.time - startTime;

        //Debug.Log(time);
       
        while (t < time)
        {

            t = Time.time - startTime;
            t = Mathf.Min(t, time);
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (0.5f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            //  Debug.Log($" x{x}: , y {y}");
            y = (float)Math.Round(y, 4);
            var upValue = projectileType == ProjectileType.Bomb ? (Vector3.up * y) : Vector3.zero;

            throwableObj.transform.position = FirePoint + direction * x + upValue;
          
            //  t += Time.fixedDeltaTime * (initialVelocity);


            yield return null;

        }

        //burası hedefe vardığında bir kez çalışır.
        OnArrived();
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
        // Debug.Log("dir: " + dir);
        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);

        height = projectileType == ProjectileType.Bomb ? (dir.y + dir.magnitude / 2f) : 0;
        height = Mathf.Max(0.01f, height);

        var dist = new Vector3(dir.x, 0, dir.z).magnitude;
        Debug.Log("targetPos: " + targetPos);
        Debug.Log("pos: " + (dist - StartPosOffSet(targetPos).magnitude));


        if (targetPos.x < minAttackLimit)
        {
            line.enabled = false;


        }
        else
        {
            line.enabled = true;

            //  if (dist <= Range)
            CalculatePathWithHeight(dir.normalized * targetPos.magnitude /*- StartPosOffSet(targetPos)*/, height, out v0, out angle, out timeNew);

        }




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

    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        var startPos = firePoint.position + StartPosOffSet(direction);
        step = Mathf.Max(0.01f, step);

        line.positionCount = (int)(time / step) + 2;

        int count = 0;

        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);

            var FirstUpValue = projectileType == ProjectileType.Bomb ? (Vector3.up * y) : Vector3.zero;

            line.SetPosition(count, startPos + direction * x + FirstUpValue);

            count++;

        }

        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

        var upValue = projectileType == ProjectileType.Bomb ? (Vector3.up * yFinal) : Vector3.zero;

        line.SetPosition(count, startPos + (direction * xFinal + upValue));


    }




}
