using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrystalMovement : MonoBehaviour
{
    #region public variables

    public PathCreator pathPrefab;
    public PathCreator currentpathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    // public Transform[] waypoints;
    public float speed = 5;
    public List<Transform> waypoints;
    #endregion
    public Transform startPoint;
    public Transform middlePoint;
    public Transform endPoint;
    public UnityEvent OnReachedTargetEvent;
    public Vector3 offSet;
    #region private variables

    float distanceTravelled;
    private bool closedLoop = false;
    private bool active = false;
    private bool isReached = false;
    private bool isThrowed = false;

    #endregion


    public void HandleWayPoints(List<Transform> waypoints)
    {
        var MidPos = (waypoints[0].position + waypoints[1].position) / 2f;
        var MidPoint = gameObject.CreateEmptyGameObject(MidPos).transform;

        waypoints.Insert(1, MidPoint);
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
    public void InitInfo(List<Transform> waypoints)
    {
        currentpathCreator = Instantiate(pathPrefab);

        HandleWayPoints(waypoints);
        if (waypoints.Count > 0)
        {
            BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);

            currentpathCreator.bezierPath = bezierPath;

            currentpathCreator.TriggerPathUpdate();

        }

        ThrowThisObject();

    }

    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //InitInfo(new Transform[] { startPoint/*, middlePoint*/, endPoint });
            //   ThrowThisObject();

        }
#endif

        if (isThrowed)
        {

            if (waypoints.Count > 0)
            {
                if (waypoints[2] != null)
                {


                    if (currentpathCreator != null)
                    {


                        waypoints[1].position = (waypoints[0].position + waypoints[2].position) / 2;

                        BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
                        currentpathCreator.bezierPath = bezierPath;
                        currentpathCreator.TriggerPathUpdate();


                        distanceTravelled += speed * Time.deltaTime;
                        transform.position = currentpathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);


                        //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                        if (!isReached)
                        {

                            if (currentpathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) == currentpathCreator.path.GetPoint(currentpathCreator.path.NumPoints - 1))
                            {
                                OnReachedTarget();



                            }
                        }
                    }
                }

            }
        }
    }
    public void OnReachedTarget()
    {
        if (OnReachedTargetEvent != null)
        {
            OnReachedTargetEvent.Invoke();

        }




        isReached = true;
        isThrowed = false;
        Debug.Log("!REACHED");

        //GemModeNetworkedGameManager gemModeNetworkedGameManager = NetworkedGameManager.Instance as GemModeNetworkedGameManager;
        //gemModeNetworkedGameManager.OnGemCollected(otherPlayerController.connectionToClient.connectionId);

        //NetworkServer.UnSpawn(gameObject);
        //ReturnHandler();


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



