using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTest : MonoBehaviour
{
    protected float timeOld;
    public LineRenderer line;
    public Vector3 dir;
    public Transform firePoint;
    public Transform targetPoint;
    private Coroutine throwingCoroutine;
    protected float movemenTime = 0;
    public float minProjectileLimit = 0.1f;
    public float step = .1f;
    public Joystick joystick;
    public float radialOffset = 2;
    public float localHorizontalOffset = 1;
    public float distance = 5;
    public enum ProjectileType { Bullet, Bomb }
    public ProjectileType projectileType;

    public float a;
    public float b;
    public float c;
    float height;
    float v0;
    float angle;
    float timeNew;

    public virtual void OnArrived()
    {
        gameObject.SetActive(false);

        //  Debug.Log("we arrived.");
    }



    private void OnDestroy()
    {
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
    }

    private void Update()
    {



        Vector3 dir = targetPoint.position - firePoint.position;
        //Vector3 pos = targetPoint.position - firePoint.position;
        //pos.y = 0;
        //Debug.Log(pos.normalized);
        Vector3 tmp = new Vector3(dir.x, -firePoint.position.y, dir.z);
       // tmp.Normalize();
        //float val = Mathf.Min(1, tmp.magnitude / distance);

        //Debug.Log(val > minProjectileLimit);

        //if (val < minProjectileLimit)
        //{
        //    line.enabled = false;


        //}
        //else
        //{
        //    line.enabled = true;

        //}
        
        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);

        Vector3 targetPos = new Vector3(groundDir.magnitude, dir.y, 0);
        //   Debug.Log(( (groundDir* distance).magnitude)/*>0.3f*/);
        //  Debug.Log(((targetPos * distance)- (OffSetHandler(groundDir)*distance)).magnitude)/*>0.3f*/;
        //Debug.Log(targetPos.y);


        //dir = (targetPoint.position - transform.position).normalized;
        // var targetingDirection = joystick.joystickHeld ? joystick.Value : (Vector2)dir;
        // targetingDirection.Normalize();
        //dir = targetingDirection;

        // dir.Normalize();
        // var groundDir = new Vector3(dir.x, -a, dir.y);
        //  Debug.Log("targetPos: " + targetPos);
        //Debug.Log("groundDir: " + groundDir.normalized);

        //CalculateProjectile(targetPos*distance);
        CalculateProjectile(tmp);
        DrawPath(groundDir.normalized, v0, angle, timeNew, step);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Throw(targetPos);

        }

    }
    private void MainAttack()
    {
        var targetingDirection = joystick.Value;
        var held = joystick.joystickHeld;


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

        // throwingCoroutine= StartCoroutine(Coroutine_Movement(this.gameObject, groundDirection.normalized, v0, angle, timeNew));
        throwingCoroutine = StartCoroutine(Coroutine_Movement(dir, v0, angle, timeNew, .1f));


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
        height = projectileType == ProjectileType.Bomb ? (dir.y + dir.magnitude / 2f) /** 5*/ : 0;
        height = Mathf.Max(0.01f, height);

        var targetPos = new Vector3(new Vector3(dir.x, 0, dir.z).magnitude, dir.y, 0);


        // DrawPath(groundDirection.normalized, v0, angle, timeNew, _step);


        // Debug.Log(dir);
        CalculatePathWithHeight((targetPos - OffSetHandler(dir))/* * distance*/, height, out v0, out angle, out timeNew);





    }

    public Vector3 OffSetHandler(Vector3 dir)
    {


        //var direction = target.position - transform.position;
        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition = /*+offsetVector * localHorizontalOffset*/  direction * radialOffset;


        return startPosition;
        // return Vector3.zero;
    }

    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);

        line.positionCount = (int)(time / step) + 2;

        int count = 0;

        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);

            var FirstUpValue = projectileType == ProjectileType.Bomb ? (Vector3.up * y) : Vector3.zero;

            line.SetPosition(count, firePoint.position + direction * x + FirstUpValue + OffSetHandler(direction));

            count++;

        }

        float xFinal = v0 * time * Mathf.Cos(angle);
        // float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

        var upValue = projectileType == ProjectileType.Bomb ? (Vector3.up * yFinal) : Vector3.zero;

        line.SetPosition(count, firePoint.position + (direction * xFinal + upValue) + OffSetHandler(direction));


    }




}
