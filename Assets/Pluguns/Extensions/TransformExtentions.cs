using UnityEngine;

public static class TransformExtentions
{

    public static Transform FindByName(this Transform instance, string name)
    {
     Component[] objs=   instance.GetComponentsInChildren(typeof(Transform), true);
        foreach (Component item in objs)
        {
            if (item.gameObject.name == "Spine")
            {
                instance = item.transform;
            
            }
        }
        return instance;

    }


}
