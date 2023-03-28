using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    //public Transform startPoint;
    //public Transform endPoint;
    public Transform[] waypoints;
    public float speed = 5;
    float distanceTravelled;
    private bool closedLoop = true;
    private bool active = false;
    private bool isReached = false;
    private bool isThrowed = false;
    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;

            if (waypoints.Length > 0)
            {
                // Create a new bezier path from the waypoints.
                BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
                pathCreator.bezierPath = bezierPath;
                // pathCreator.bezierPath.NotifyPathModified();
            }

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowThisObject();

        }

        if (isThrowed)
        {



            if (pathCreator != null)
            {

                if (waypoints.Length > 0)
                {
                    // Create a new bezier path from the waypoints.
                    BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
                    pathCreator.bezierPath = bezierPath;
                    pathCreator.TriggerPathUpdate();

                }
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);


                //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                if (!isReached)
                {

                    if (pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) == pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1))
                    {

                        isReached = true;
                        isThrowed = false;
                        Debug.Log("!REACHED");

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
        transform.position = pathCreator.path.GetPoint(0);
        active = false;
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        //Debug.Log("changed");
        if (!active && isThrowed )
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
