using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    private Grid grid;
    public GameObject cube;
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && GridManager.instance.inputState==GridManager.InputState.Idle)
        {

            RaycastHit hitInfo;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction*100, Color.black,10);
            if(Physics.Raycast(ray,out hitInfo))
            {
                Debug.Log("HİTTEd " + hitInfo.transform.name);
                PlaceCubeNear(hitInfo.point);


            }
        }
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {

        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
         //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
         Instantiate(cube,transform).transform.position =new Vector3(finalPosition.x,cube.transform.position.y,finalPosition.z); 
       

    }

}
