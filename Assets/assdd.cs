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
            //CrystalMovement.startPoint.position = go.transform.position;
            CrystalMovement.middlePoint.position = (CrystalMovement.startPoint.position + CrystalMovement.endPoint.position) / 2;
            //TODO: start point'de sıkıntı vaqr endpoint'de bir sıkıntı yok.
            var points = new Transform[] { CrystalMovement.startPoint,CrystalMovement.middlePoint /*go.transform*/, CrystalMovement.endPoint /*go.transform*/ };
    
            CrystalMovement.InitInfo(points);
        }
    }
}   
