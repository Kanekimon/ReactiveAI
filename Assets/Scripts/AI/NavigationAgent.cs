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
    NavMeshAgent NavMeshAgent;
    Agent Agent;
    [SerializeField] float NearestPointSearchRange = 5f;
    bool DestinationSet = false;
    EOffMeshLinkStatus OffMeshLinkStatus = EOffMeshLinkStatus.NotStarted;
    bool ReachedDestination = false;
    public bool IsMoving => NavMeshAgent.velocity.magnitude > float.Epsilon;
    public bool AtDestination => ReachedDestination;
    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Agent = GetComponent<Agent>();
    }

    void Update()
    {
        if(!NavMeshAgent.pathPending && !NavMeshAgent.isOnOffMeshLink && DestinationSet && (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance))
        {
            DestinationSet = false;
            ReachedDestination = true;
        }

        if (NavMeshAgent.isOnOffMeshLink)
        {
            if (OffMeshLinkStatus == EOffMeshLinkStatus.NotStarted)
                StartCoroutine(FollowOffmeshLink());
        }
        Agent.WorldState.ChangeValue("currentPosition", transform.position);
        
    }

    IEnumerator FollowOffmeshLink()
    {
        OffMeshLinkStatus = EOffMeshLinkStatus.InProgress;
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updateUpAxis = false;

        Vector3 newPosition = transform.position;

        while(!Mathf.Approximately(Vector3.Distance(newPosition, NavMeshAgent.currentOffMeshLinkData.endPos), 0f))
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

    protected void CancelCurrentCommand()
    {
        NavMeshAgent.ResetPath();
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
            NavMeshAgent.SetDestination(hitResult.position);
            DestinationSet = true;
            ReachedDestination = false;
        }
    }




}

