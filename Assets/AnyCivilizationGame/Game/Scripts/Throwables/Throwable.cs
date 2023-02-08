using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class Throwable : NetworkBehaviour
{
    protected float speed;

    private Coroutine throwingCoroutine;
    protected float movemenTime = 0;



    #region new

    public enum ProjectileType { Linear, Parabolic }
    public ProjectileType projectileType;


    #endregion

    float height;
    float v0;
    float angle;
    float timeNew;


    float height2;
    float v02;
    float angle2;
    float timeNew2;


    //public void Throw(Vector3[] path)
    //{
    //    throwingCoroutine = StartCoroutine(Coroutine_Movement(path));
    //}

    //private void Update()
    //{
    //    if (movemenTime > 0)
    //    {
    //        movemenTime += Time.deltaTime;
    //    }
    //}
    public virtual void OnArrived()
    {
        gameObject.SetActive(false);

        //  Debug.Log("we arrived.");
    }

    //IEnumerator Coroutine_Movement(Vector3[] path)
    //{
    //    movemenTime += Time.deltaTime;
    //    var flow = movemenTime / timeOld;
    //    var index = 0; // current positionIndex
    //    var lastIndex = path.Length - 1; // last index of positions
    //    var currentPosition = path[index];


    //    while (index <= lastIndex)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, currentPosition, flow);
    //        if (Vector3.Distance(transform.position, currentPosition) <= 0.01f)
    //        {
    //            index++;
    //            index = Mathf.Min(index, lastIndex);
    //            currentPosition = path[index];
    //        }
    //        yield return null;
    //    }
    //    movemenTime = 0;
    //    OnArrived();
    //}

    private void OnDestroy()
    {
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
    }





    #region new


    public void Throw(Vector3 dir, float Range)
    {

        float dist = Mathf.Abs(/*playerController.BulletSpawnPoints[2].spawnPoint.z */-0.4f  - /*radialOffset*/0.6f);

        var targetPos2 = new Vector3(dir.magnitude * Range + (dist), /*-playerController.BulletSpawnPoints[0].spawnPoint.y*//*-1f*/ -transform.position.y, 0);
      //  Debug.Log("Throwable targetPos2: " + targetPos2);


        //CalculateProjectile(dir, Range);
        CalculateProjectile2(targetPos2, Range);
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);

        // throwingCoroutine= StartCoroutine(Coroutine_Movement(this.gameObject, groundDirection.normalized, v0, angle, timeNew));
        //throwingCoroutine = StartCoroutine(Coroutine_Movement(dir, v0, angle, timeNew, .1f));

        //  Debug.Log("Throwable dir normalized: " + dir.normalized);
       
        throwingCoroutine = StartCoroutine(Coroutine_Movement(dir.normalized, v02, angle2, timeNew2, speed));
        //Debug.Log("height2:" + height2);
        //Debug.Log("VO2:" + v02);
        //Debug.Log("angle2:" + angle2);
        //Debug.Log("timeNew2:" + timeNew2);

    }



    //IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity)
    //{

    //    var FirePoint = transform.position;

    //    float t = 0;
    //    // Debug.Log(time / (initialVelocity ));
    //    // Debug.Log(BulletObj.transform.name);
    //    while (t < time)
    //    {

    //        float x = v0 * t * Mathf.Cos(angle);
    //        float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);

    //        var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

    //        //BulletObj.transform.position = FirePoint + direction * x + upValue;
    //        transform.position = FirePoint + direction * x + upValue;



    //        t += Time.deltaTime * (initialVelocity);

    //        yield return null;

    //    }

    //    //burası hedefe vardığında bir kez çalışır.
    //    OnArrived();
    //}
    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity)
    {
        var yOffSet = 0f;
        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Destroy(go.GetComponent<Collider>());
        //go.transform.localScale = Vector3.one * .5f;
        //go.GetComponent<Renderer>().material.color = Color.red;

        yOffSet = /*playerController.BulletSpawnPoints[2].spawnPoint.y*/ -0.5f;

        var startPos = transform.position /*+ new Vector3(0, yOffSet, 0) + StartPosOffSet2(direction)*/;

        //  var FirePoint = transform.position + StartPosOffSet(direction);

        // Debug.Log(time / (initialVelocity ));
        // Debug.Log(BulletObj.transform.name);

        float startTime = 0;
        startTime = Time.time;
        float t = Time.time - startTime;


        while (t < time)
        {

            t = Time.time - startTime;
            t = Mathf.Min(t, time);

            float x = v0 * t * Mathf.Cos(angle);
   
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            y = (float)Math.Round(y, 4);
            var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            /*go.*/transform.position = startPos + direction * x + upValue;



         


            yield return null;

        }
      
        //Destroy(go, 1f);
        //burası hedefe vardığında bir kez çalışır.
          // OnArrived();

    }
    public Vector3 StartPosOffSet2(Vector3 dir)
    {

        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition =  direction /** playerController.BulletSpawnPoints[2].spawnPoint.z*/ * (-0.4f);


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


    public void CalculateProjectile(Vector3 dir, float Range)
    {

        height = projectileType == ProjectileType.Parabolic ? (dir.y + dir.magnitude / 2f) : 0;
        height = Mathf.Max(0.01f, height);

        var targetPos = new Vector3(dir.magnitude, dir.y, 0);

        // DrawPath(groundDirection.normalized, v0, angle, timeNew, _step);

      // Debug.Log("TargetPosRange: " + (targetPos * Range));
        // Debug.Log(dir);
        CalculatePathWithHeight(targetPos * Range, height, out v0, out angle, out timeNew);





    }
    public void CalculateProjectile2(Vector3 dir, float Range)
    {

    
        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);

        height2 = projectileType == ProjectileType.Parabolic ? (dir.y + dir.magnitude / 2f) : 0;
        height2 = Mathf.Max(0.01f, height2);

        var dist = new Vector3(dir.x, 0, dir.z).magnitude;


        if (targetPos.x < 0.02f)
        {
            //   AttackBasicIndicator.enabled = false;


        }
        else
        {
            //  AttackBasicIndicator.enabled = true;

            //if (dist <= playerController.Range)
            //{
            //   Debug.Log("lineRange: " + (dir.normalized * targetPos.magnitude /*- StartPosOffSet(targetPos)*/));

            CalculatePathWithHeight(dir.normalized * targetPos.magnitude /** Range *//*- StartPosOffSet2(targetPos)*4*/, height2, out v02, out angle2, out timeNew2);
          
            // CalculatePathWithHeight(targetPos * Range /*- StartPosOffSet2(targetPos)*4*/, height2, out v02, out angle2, out timeNew2);

            //}

        }




    }

    #endregion



}
