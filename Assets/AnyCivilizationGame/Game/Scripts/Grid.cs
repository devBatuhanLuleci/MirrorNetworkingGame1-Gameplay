using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;

    public float Size { get { return size; } }

    public GameObject SimpleGrid;
    public int xDistance = 20;
    public int zDistance = 40;

    public List<Vector3> Grids = new List<Vector3>();
    public List<Vector3> ExcludedGrids = new List<Vector3>();

    public Vector3 offSet;
    public int DistanceX
    {
        get { return xDistance; }
        set
        {
            if (xDistance == value) return;
            xDistance = value;
            if (OnVariableChange != null)
                OnVariableChange(xDistance);
        }
    }
    public delegate void OnVariableChangeDelegate(int newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    public List<Vector3> tempGrid = new List<Vector3>();

    public Transform GridParent;


    private void Start()
    {

        if (Grids.Count > 0)
            Grids.Clear();
        // Debug.Log(GridParent.childCount);
        foreach (Transform child in GridParent)
        {
            StartCoroutine(Destroy(child.gameObject));
        }

        Gizmos.color = Color.yellow;
        for (float x = 0; x < DistanceX; x += size)
        {
            for (float z = 0; z < zDistance; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0, z)+transform.position);
                Instantiate(SimpleGrid, GridParent).transform.position = point + new Vector3(0, 0.015f, 0);
                if (!isGridPlaced(point, Grids))
                {

                    Grids.Add(point);

                }



            }
        }
    }



    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {

        position -= transform.position;


        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);


        Vector3 result = new Vector3(

            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;


        return result;


    }
    public Vector3 Draw(Vector3 position)
    {

       // position += transform.position;


        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);


        Vector3 result = new Vector3(

            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;


        return result;


    }

    public bool toggleValidation;

  
    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        for (float x = 0; x < DistanceX; x += size)
        {
            for (float z = 0; z < zDistance; z += size)
            {
                var point = Draw(new Vector3(x, 0, z));

                Gizmos.DrawSphere(point, 0.1f);


            }
        }


    }

    public bool isGridPlaced(Vector3 gridPos, List<Vector3> gridT)
    {

        bool exists = gridT.Any(x => x == gridPos);

        return exists;

    }

   

}
