using Mirror;
using System.Collections;
using UnityEngine;

public class Throwable : NetworkBehaviour
{
    protected float timeOld;

    private Coroutine throwingCoroutine;
    protected float movemenTime = 0;



    #region new

    public enum ProjectileType { Bullet, Bomb }
    public ProjectileType projectileType;


    #endregion

    float height;
    float v0;
    float angle;
    float timeNew;
  
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


    public void Throw(Vector3 dir)
    {
      

        CalculateProjectile(dir);
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);

        // throwingCoroutine= StartCoroutine(Coroutine_Movement(this.gameObject, groundDirection.normalized, v0, angle, timeNew));
        throwingCoroutine = StartCoroutine(Coroutine_Movement(dir,v0,angle,timeNew,.1f));


    }



    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float initialVelocity)
    {

        var FirePoint = transform.position;

        float t = 0;
        // Debug.Log(time / (initialVelocity ));
        // Debug.Log(BulletObj.transform.name);
        while (t < time)
        {

            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);

            var upValue = projectileType == ProjectileType.Bomb ? (Vector3.up * y) : Vector3.zero;

            //BulletObj.transform.position = FirePoint + direction * x + upValue;
            transform.position = FirePoint + direction * x + upValue;



            t += Time.deltaTime * (initialVelocity);

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

         height = projectileType == ProjectileType.Bomb ? (dir.y + dir.magnitude / 2f) : 0;
        height = Mathf.Max(0.01f, height);
    
       var  targetPos = new Vector3(dir.magnitude, dir.y, 0);

        // DrawPath(groundDirection.normalized, v0, angle, timeNew, _step);


       // Debug.Log(dir);
        CalculatePathWithHeight(targetPos*5, height, out v0, out angle, out timeNew);





    }
    #endregion



}
