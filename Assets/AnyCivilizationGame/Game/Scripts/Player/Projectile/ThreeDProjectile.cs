using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PredictedProjectileExample
{

    public class ThreeDProjectile : MonoBehaviour
    {

        public enum ProjectileType { Bullet,Bomb}
        public ProjectileType projectileType;


        [SerializeField]
        float initialVelocity;

        [SerializeField]
        LineRenderer line;


        float _step;

        [SerializeField]
        Transform firePoint;

     
        public Transform targetPoint;

        [HideInInspector]
        public GameObject BulletObj;
        private PlayerAttack playerAttack;

        private bool throwable = false;



        float height;
        float angle;
        float v0;
        float time;
        Vector3 direction;
        Vector3 groundDirection;
        Vector3 targetPos;
        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
        }

        private void Update()
        {



            height = projectileType == ProjectileType.Bomb ? (targetPos.y + targetPos.magnitude / 2f) : 0;
            height = Mathf.Max(0.01f, height);
            //float angle;
            //float v0;
            //float time;
            CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);

       
            DrawPath(groundDirection.normalized, v0, angle, time, _step);

            direction = targetPoint.position - firePoint.position;
            groundDirection = new Vector3(direction.x, 0, direction.z);
            targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);


            if (Input.GetKeyDown(KeyCode.Space))
            {


                //StopAllCoroutines();
                //StopCoroutine(Coroutine_Movement(groundDirection.normalized, v0, angle, time));
                StartCoroutine(Coroutine_Movement(BulletObj,groundDirection.normalized, v0, angle, time));


            } 
        }
        public void ThrowThisObject()
        {
     

           // StopAllCoroutines();
            StopCoroutine(Coroutine_Movement(BulletObj, groundDirection.normalized, v0, angle, time));
            StartCoroutine(Coroutine_Movement(BulletObj,groundDirection.normalized, v0, angle, time));


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


        IEnumerator Coroutine_Movement(GameObject bulletObj, Vector3 direction, float v0, float angle, float time)
        {
            var FirePoint = firePoint.position;

            float t = 0;
           // Debug.Log(time / (initialVelocity ));
                Debug.Log(BulletObj.transform.name);
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
            bulletObj.SetActive(false);
        }

    }
}
