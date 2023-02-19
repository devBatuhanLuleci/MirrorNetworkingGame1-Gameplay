using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class Throwable : NetworkBehaviour
{
    public float speed= .2f;

    private Coroutine throwingCoroutine;
    protected float movemenTime = 0;


    public string OwnerName = "";
    public uint OwnerNetId = 0;
    public uint RootNetId = 0;

    #region new

    public enum ProjectileType { Linear, Parabolic }
    public ProjectileType projectileType;


    #endregion


    float height;
    float v0;
    float angle;
    float time;


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
  
    public virtual void OnObjectSpawn()
    {
        
        //Inherited.
    }
   

    public void Init(string ownerName, uint ownerNetId, uint RootId = 0, bool isRooted =false  )
    {
        OwnerName = ownerName;
        OwnerNetId = ownerNetId;


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


    public void Throw(Vector3 dir, float Range, float offSetZValue =0, float offSetYValue = 0, float radialOffSet=0f)
    {




        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);
        //float dist = Mathf.Abs(/*playerController.BulletSpawnPoints[2].spawnPoint.z */-0.4f  - /*radialOffset*/0.6f);

        var targetPos = new Vector3(groundDir.magnitude * (Range)+ offSetZValue, /*dir.y*/ -offSetYValue, 0);
     
      //  var targetPos = new Vector3(dir.magnitude * Range + (offSetZValue),  -transform.position.y, 0);
        
       
        CalculateProjectile(targetPos);
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);




        throwingCoroutine = StartCoroutine(Coroutine_Movement(groundDir.normalized, v0, angle, time, speed,  radialOffSet, offSetYValue));


    }



    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity, float radialOffSet, float offSetYValue)
    {

      //  Debug.Log("targetPos: " + direction);

      
        //switch (projectileType)
        //{
        //    case ProjectileType.Linear:
        //        yOffSet = 0f;
        //        break;
        //    case ProjectileType.Parabolic:
        //        yOffSet = bullet[0].spawnPoint.y;
        //        break;
        //    default:
        //        break;
        //}

        var startPos = transform.position + new Vector3(0, 0, 0) /*+ StartPosOffSet2(direction,radialOffSet)*/;


        float startTime = 0;
        startTime = Time.time;
        float t = Time.time - startTime;



        //  #region SpawnObject
          float x1 = v0 * t * Mathf.Cos(angle);
        //// Debug.Log("direction:" + direction);
          float y1 = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
          y1 = (float)Math.Round(y1, 4);
          var upValue2 = projectileType == ProjectileType.Parabolic ? (Vector3.up * y1) : Vector3.zero;

        //  GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //  //Debug.Log("1: " + direction * x1);
        //  //Debug.Log("2: " + upValue2);
      //  Debug.Log("distance: " + Vector3.Distance(startPos, startPos + direction * x1 + upValue2));

        //  go.transform.position = startPos + direction * x1 + upValue2;
        //  go.transform.localScale = Vector3.one * .4f;
        //  go.transform.GetComponent<Renderer>().material.color = Color.red;
        //  #endregion

   //    gameObject.CreatePrimitiveObject(startPos + direction * x1 + upValue2, .4f);



        while (t < time)
        {

            t = (Time.time - startTime)* initialVelocity;
            
            t = Mathf.Min(t, time) ;

            float x = v0 * t * Mathf.Cos(angle);
   
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            y = (float)Math.Round(y, 4);
            var upValue = projectileType == ProjectileType.Parabolic ? (Vector3.up * y) : Vector3.zero;

            /*go.*/transform.position = startPos + direction * x + upValue;



         


            yield return null;

        }
     //   gameObject.CreatePrimitiveObject(startPos + direction * x1 + upValue2, .4f);
//
        //Destroy(go, 1f);
        //burası hedefe vardığında bir kez çalışır.
        OnArrived();

    }
    public Vector3 StartPosOffSet2(Vector3 dir, float radialOffSet)
    {

        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition =  direction  * (-radialOffSet);


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

        //  var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);
        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);

        height = projectileType == ProjectileType.Parabolic ? (0 + new Vector3(dir.x, 0, dir.z).magnitude / 2f) : 0;

        height = Mathf.Max(0.01f, height);

        var dist = new Vector3(dir.x, 0, dir.z);


        //if (targetPos.x < 0.02f)
        //{

        //}
        //else
        //{

        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //go.transform.position = targetPos;
        //go.transform.localScale = Vector3.one * .4f;
        //go.transform.GetComponent<Renderer>().material.color = Color.red;


        //CalculatePathWithHeight(dir.normalized * targetPos.magnitude , height, out v0, out angle, out time);

        CalculatePathWithHeight(dir.normalized * targetPos.magnitude, height, out v0, out angle, out time);
          
          
      //  }




    }

    #endregion



}
