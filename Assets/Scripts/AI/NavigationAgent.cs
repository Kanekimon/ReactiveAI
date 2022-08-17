using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    NavMeshAgent Agent;
    [SerializeField] float NearestPointSearchRange = 5f;
    bool DestinationSet = false;
    EOffMeshLinkStatus OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
    bool ReachedDestination = false;
    public bool IsMoving => Agent.velocity.magnitude > float.Epsilon;
    public bool AtDestination => ReachedDestination;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(!Agent.pathPending && !Agent.isOnOffMeshLink && DestinationSet && (Agent.remainingDistance <= Agent.stoppingDistance))
        {
            DestinationSet = false;
            ReachedDestination = true;
        }

        if (Agent.isOnOffMeshLink)
        {
            if (OffMeshLinkStatus == EOffMeshLinkStatus.NotStarted)
                StartCoroutine(FollowOffmeshLink());
        }
    }

    IEnumerator FollowOffmeshLink()
    {
        OffMeshLinkStatus = EOffMeshLinkStatus.InProgress;
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        Vector3 newPosition = transform.position;

        while(!Mathf.Approximately(Vector3.Distance(newPosition, Agent.currentOffMeshLinkData.endPos), 0f))
        {
            newPosition = Vector3.MoveTowards(transform.position, Agent.currentOffMeshLinkData.endPos, Agent.speed * Time.deltaTime);
            transform.position = newPosition;

            yield return new WaitForEndOfFrame();
        }

        OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
        Agent.CompleteOffMeshLink();


        Agent.updatePosition = true;
        Agent.updateRotation = true;
        Agent.updateUpAxis = true;
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

    protected void CancelCurrentCommand()
    {
        Agent.ResetPath();
        DestinationSet = false;
        ReachedDestination = false;
        OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
    }

    public void MoveTo(Vector3 destination)
    {
        CancelCurrentCommand();
        SetDestination(destination);
    }

    public void StopMoving()
    {
        CancelCurrentCommand();
    }

    public void SetDestination(Vector3 destination)
    {
        NavMeshHit hitResult;

        if(NavMesh.SamplePosition(destination, out hitResult, NearestPointSearchRange, NavMesh.AllAreas))
        {
            Agent.SetDestination(hitResult.position);
            DestinationSet = true;
            ReachedDestination = false;
        }
    }




}

