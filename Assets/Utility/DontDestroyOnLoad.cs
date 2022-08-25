using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField]
    private string parent = "";

    private void Awake()
    {
        if(string.IsNullOrEmpty(parent))
        {
            DontDestroyOnLoad(gameObject);
        }
        var parentObject = GameObject.Find(parent);
        if (parentObject == null)
        {
            parentObject = new GameObject(parent);
            DontDestroyOnLoad(parentObject);
        }
        transform.SetParent(parentObject.transform);
    }
}
