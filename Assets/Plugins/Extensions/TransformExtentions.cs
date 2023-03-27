using UnityEngine;

public static class TransformExtentions
{

    public static Transform FindByName(this Transform instance, string name)
    {
     Component[] objs=   instance.GetComponentsInChildren(typeof(Transform), true);
        foreach (Component item in objs)
        {
            if (item.gameObject.name == name)
            {
                instance = item.transform;
            
            }
        }
        return instance;

    }
    public static Vector3 RandomDirection(this Vector3 vector)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }




}
