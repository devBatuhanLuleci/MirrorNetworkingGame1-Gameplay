using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationManager : MonoBehaviour
{
    public static float GetAngle(Transform from, Transform to)
    {
        float angle = Vector3.Angle((to.position - from.position), Vector3.forward);
        float angle2 = Vector3.Angle((to.position - from.position), Vector3.right);

        if (angle2 > 90)
        {
            angle = 360 - angle;
        }

        return angle;
    }
    public static float GetAngle(Transform from, Vector3 to)
    {
        float angle = Vector3.Angle((to - from.position), Vector3.forward);
        float angle2 = Vector3.Angle((to - from.position), Vector3.right);

        if (angle2 > 90)
        {
            angle = 360 - angle;
        }

        return angle;
    }
    public static float GetAngle(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, Vector3.forward);
        float angle2 = Vector3.Angle(dir, Vector3.right);

        if (angle2 > 90)
        {
            angle = 360 - angle;
        }

        return angle;
    }
    public static float GetAngle(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle((to - from), Vector3.forward);
        float angle2 = Vector3.Angle((to - from), Vector3.right);

        if (angle2 > 90)
        {
            angle = 360 - angle;
        }

        return angle;
    }
}
