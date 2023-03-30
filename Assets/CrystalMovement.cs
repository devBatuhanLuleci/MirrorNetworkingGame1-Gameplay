using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMovement : MonoBehaviour
{
    #region public variables

    public PathCreator pathPrefab;
    public PathCreator currentpathCreator;
    public EndOfPathInstruction endOfPathInstruction;
   // public Transform[] waypoints;
    public float speed = 5;
    public Transform[] waypoints;
    #endregion
    public Transform startPoint;
    public Transform middlePoint;
    public Transform endPoint;

    #region private variables

    float distanceTravelled;
    private bool closedLoop = false;
    private bool active = false;
    private bool isReached = false;
    private bool isThrowed = false;

    #endregion


    public void HandleWayPoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }
    void Start()
    {
        if (currentpathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            currentpathCreator.pathUpdated += OnPathChanged;



        }

    }
    public void InitInfo(Transform[] waypoints)
    {
        currentpathCreator = Instantiate(pathPrefab);
       
        HandleWayPoints(waypoints);
        if (waypoints.Length > 0)
        {
            // Create a new bezier path from the waypoints.
            // BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
           // BezierPath bezierPath = new BezierPath(,waypoints, closedLoop, PathSpace.xyz);
            BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);

            currentpathCreator.bezierPath = bezierPath;
            currentpathCreator.TriggerPathUpdate();
            // pathCreator.bezierPath.NotifyPathModified();
        }

       ThrowThisObject();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //InitInfo(new Transform[] { startPoint/*, middlePoint*/, endPoint });
        //   ThrowThisObject();

        }

        if (isThrowed)
        {



            if (currentpathCreator != null)
            {

                if (waypoints.Length > 0)
                {
                    waypoints[1].position = (waypoints[0].position + waypoints[2].position) / 2;

                    BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
                    currentpathCreator.bezierPath = bezierPath;
                    currentpathCreator.TriggerPathUpdate();

                }
                distanceTravelled += speed * Time.deltaTime;
                transform.position = currentpathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);


                //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                if (!isReached)
                {

                    if (currentpathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) == currentpathCreator.path.GetPoint(currentpathCreator.path.NumPoints - 1))
                    {

                        //isReached = true;
                        //isThrowed = false;
                        //Debug.Log("!REACHED");

                    }
                }


            }
        }
    }
    private void ThrowThisObject()
    {
        isThrowed = true;
        isReached = false;
        active = true;
        distanceTravelled = 0;
        transform.position = currentpathCreator.path.GetPoint(0);
        active = false;
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        //Debug.Log("changed");
        if (!active && isThrowed)
            distanceTravelled = currentpathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

}
