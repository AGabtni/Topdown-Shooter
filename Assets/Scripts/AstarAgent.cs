using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class AstarAgent : MonoBehaviour
{

    public Transform target;
    [SerializeField] float speed = 200f;
    Seeker seeker;
    Rigidbody2D body;

    Path path;

    [Tooltip("How close enemy is to target before moving to next targer")]
    [SerializeField] float nexWaypointDistance = 3f;
    int currentWaypoint = 0;

    [SerializeField] float repathRate = 0.5f;
    float lastRepath = float.NegativeInfinity;

    bool _hasReachEndOfPath = false;
    public bool reachedEndOfPath
    {

        get { return _hasReachEndOfPath; }
    }
    Vector2 _velocity;
    public Vector2 velocity{

        get{return _velocity;}
    }

    [SerializeField] float distanceBeforeStop = 0.02f;
    public bool reachedDestination
    {
        get
        {

            if (!reachedEndOfPath ) return false;
            // Note: distanceToSteeringTarget is the distance to the end of the path when approachingPathEndpoint is true

            // Check against using a very small margin . Margin should be changed when changing target's or this object's collider .
            if (pathLength > distanceBeforeStop) return false;
            return true;
        }
    }

    void Start()
    {


        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();


    }





    float pathLength;
    //Callback for when new path is generated
    public void OnPathComplete(Path newPath)
    {


        newPath.Claim(this);
        if (!newPath.error)
        {
            if (path != null)
                path.Release(this);

            path = newPath;
            currentWaypoint = 0;
            pathLength = path.GetTotalLength();

        }
        else
        {

            newPath.Release(this);
        }



    }


    void Update()
    {
        //Save last time AI generated new path after repathting and request new path
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }

        //Do nothing when no path is available
        if (path == null)
            return;

        _hasReachEndOfPath = false;
        float distanceToNextWaypoint;
        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        while (true)
        {

            //Calculate distance to next waypoint in path
            distanceToNextWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToNextWaypoint < nexWaypointDistance)
            {

                //Check whether reached end of path or if another waypoint is available in current path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    _hasReachEndOfPath = true;
                    break;
                }

            }
            else
            {
                //end of path reached
                break;
            }

        }

        //Slow down upon approaching end of path . Going from 1 to 0 smoothly when approching last waypoint
        var speedFactor = _hasReachEndOfPath ? Mathf.Sqrt(distanceToNextWaypoint / nexWaypointDistance) : 1f;
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        _velocity = direction * speed * speedFactor * Time.deltaTime;



        body.AddForce(_velocity);

    

    }

    void FixedUpdate()
    {



    }


    void OnEnable()
    {
        _hasReachEndOfPath = false;
        path = null;
    }

}   
