using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class assdd : MonoBehaviour
{
    private CrystalMovement CrystalMovement;
    private void Start()
    {
            CrystalMovement=GetComponent<CrystalMovement>();    
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = new GameObject();

           var crystalPos = new Vector3(1, 0,3);
  
            go.transform.position = crystalPos;

            var startPos = transform;

            var points = new Transform[] {CrystalMovement.startPoint /*go.transform*/, CrystalMovement.endPoint };
    
            CrystalMovement.InitInfo(points);
        }
    }
}
