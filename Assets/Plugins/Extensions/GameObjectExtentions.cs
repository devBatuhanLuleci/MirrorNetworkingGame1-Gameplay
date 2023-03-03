using UnityEngine;

public static class GameObjectExtentions
{

    public static GameObject CreatePrimitiveObject(this GameObject instance, Vector3 pos ,Color color,  float scale )
    {


        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = Vector3.one * scale;
        go.GetComponent<Renderer>().material.color = color;
        go.transform.position = pos;
        instance = go;
        return instance; 



    }


}
