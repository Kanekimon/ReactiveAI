﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EOffMeshLinkStatus
{
    NotStarted,
    InProgress
}

[RequireComponent(typeof(NavMeshAgent))]
public class NavigationAgent : MonoBehaviour
{
    public float IdleTimer;
    float idleTime;
    Vector3 lastUpdatePosition;
    bool idle;

    NavMeshAgent NavMeshAgent;
    NavMeshObstacle NavMeshObstacle;
    Agent Agent;
    StateMemory WorldState;
    [SerializeField] float NearestPointSearchRange = 5f;
    public float RotationSpeed;
    bool DestinationSet = false;
    EOffMeshLinkStatus OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
    bool ReachedDestination = false;
    public bool IsMoving => NavMeshAgent.velocity.magnitude > float.Epsilon;
    public bool AtDestination => ReachedDestination;
    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshObstacle = GetComponent<NavMeshObstacle>();
        Agent = GetComponent<Agent>();
        WorldState = Agent.WorldState;

        NavMeshAgent.avoidancePriority = Random.Range(0, 100);
    }

    void Update()
    {
        if (lastUpdatePosition == this.transform.position)
        {
            idleTime += Time.deltaTime;
            if (idle)
                idleTime = 0;

            if (idleTime >= IdleTimer)
            {
                SetIdle(true);
            }
        }
        else
        {
            SetIdle(false);
            idleTime = 0;
            lastUpdatePosition = this.transform.position;
        }



        if (NavMeshAgent.enabled == true)
        {
            Agent.GetComponent<Animator>().SetFloat("WalkSpeed", NavMeshAgent.velocity.magnitude);
            if (!NavMeshAgent.pathPending && !NavMeshAgent.isOnOffMeshLink && DestinationSet && (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance))
            {
                DestinationSet = false;
                ReachedDestination = true;
            }

            if (NavMeshAgent.isOnOffMeshLink)
            {
                if (OffMeshLinkStatus == EOffMeshLinkStatus.NotStarted)
                    StartCoroutine(FollowOffmeshLink());
            }
            WorldState.ChangeValue("currentPosition", transform.position);
        }

    }

    private void FixedUpdate()
    {
    }

    IEnumerator FollowOffmeshLink()
    {
        OffMeshLinkStatus = EOffMeshLinkStatus.InProgress;
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updateUpAxis = false;

        Vector3 newPosition = transform.position;

        while (!Mathf.Approximately(Vector3.Distance(newPosition, NavMeshAgent.currentOffMeshLinkData.endPos), 0f))
        {
            newPosition = Vector3.MoveTowards(transform.position, NavMeshAgent.currentOffMeshLinkData.endPos, NavMeshAgent.speed * Time.deltaTime);
            transform.position = newPosition;

            yield return new WaitForEndOfFrame();
        }

        OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
        NavMeshAgent.CompleteOffMeshLink();


        NavMeshAgent.updatePosition = true;
        NavMeshAgent.updateRotation = true;
        NavMeshAgent.updateUpAxis = true;
    }

    public Vector3 PickLocationInRange(float range)
    {
        Vector3 searchLocation = transform.position;
        searchLocation += UnityEngine.Random.Range(-range, range) * Vector3.forward;
        searchLocation += UnityEngine.Random.Range(-range, range) * Vector3.right;

        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(searchLocation, out hitResult, range, NavMesh.AllAreas))
            return hitResult.position;

        return transform.position;
    }

    public Vector3 PickClosestPositionInRange(GameObject targetObject, float range)
    {

        Vector3 target = targetObject.transform.position;
        Vector3 dist = (transform.position - target).normalized;
        Vector3 targetPoint = targetObject.transform.position + (dist * range);
        NavMeshHit hitResult;

        targetPoint.y = Terrain.activeTerrain.SampleHeight(targetPoint);


        if (NavMesh.SamplePosition(targetPoint, out hitResult, 1, NavMesh.AllAreas))
        {
            return hitResult.position;
        }

        return GetRandomPoint(targetPoint, range);
    }

    // Get Random Point on a Navmesh surface
    public static Vector3 GetRandomPoint(Vector3 center, float maxDistance)
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return hit.position;
    }

    public Vector3 PickLocationNearTarget(Vector3 targetPos, float range)
    {
        Vector3 searchLocation = targetPos;
        searchLocation += UnityEngine.Random.Range(-range, range) * Vector3.forward;
        searchLocation += UnityEngine.Random.Range(-range, range) * Vector3.right;

        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(searchLocation, out hitResult, range, NavMesh.AllAreas))
            return hitResult.position;

        return transform.position;
    }

    protected void CancelCurrentCommand()
    {
        NavMeshAgent.ResetPath();
        DestinationSet = false;
        ReachedDestination = false;
        OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
    }

    public void MoveTo(Vector3 destination)
    {
        SetIdle(false);


        CancelCurrentCommand();
        RotateTowards(destination);
        SetDestination(destination);
    }

    public void StopMoving()
    {
        CancelCurrentCommand();
    }

    public void SetDestination(Vector3 destination)
    {
        NavMeshHit hitResult;

        if (NavMesh.SamplePosition(destination, out hitResult, NearestPointSearchRange, NavMesh.AllAreas))
        {


            NavMeshAgent.SetDestination(hitResult.position);
            DestinationSet = true;
            ReachedDestination = false;
        }
    }

    public void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    void SetIdle(bool pidle)
    {
        if (idle == pidle)
            return;

        idle = pidle;
        if (idle)
        {
            NavMeshAgent.enabled = false;
            NavMeshObstacle.enabled = true;
        }
        else
        {
            NavMeshObstacle.enabled = false;
            NavMeshAgent.enabled = true;
        }

    }


}

