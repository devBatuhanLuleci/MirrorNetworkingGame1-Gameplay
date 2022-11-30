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
        Debug.Log("we arrived.");
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

    private void Update()
    {
        //height = projectileType == ProjectileType.Bomb ? (targetPos.y + targetPos.magnitude / 2f) : 0;
        //height = Mathf.Max(0.01f, height);

        //CalculatePathWithHeight(targetPos, height, out v0, out angle, out timeNew);


        //DrawPath(groundDirection.normalized, v0, angle, timeNew, _step);

        //direction = targetPoint.position - firePoint.position;
        //groundDirection = new Vector3(direction.x, 0, direction.z);
        //targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);


    }

    public void Throw(Vector3 dir , float v , float Angle , float TimeNew, float initialVelocity)
    {

        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);

       // throwingCoroutine= StartCoroutine(Coroutine_Movement(this.gameObject, groundDirection.normalized, v0, angle, timeNew));
        throwingCoroutine = StartCoroutine(Coroutine_Movement(this.gameObject, dir, v, Angle, TimeNew,initialVelocity));


    }



    IEnumerator Coroutine_Movement(GameObject bulletObj, Vector3 direction, float v0, float angle, float time,float initialVelocity)
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
            bulletObj.transform.position = FirePoint + direction * x + upValue;



            t += Time.deltaTime * (initialVelocity);

            yield return null;

        }

        //burası hedefe vardığında bir kez çalışır.
        OnArrived();
        bulletObj.SetActive(false);
    }
    #endregion



}
