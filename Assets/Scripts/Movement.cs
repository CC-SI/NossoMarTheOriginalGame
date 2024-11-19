using System.Collections;
using Actors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IMovement
{
    [field: Header("Componentes")]
    [field: SerializeField]
    public NavMeshAgent Agent { get; private set; }
    
    [field: Header("Eventos")]
    [field: SerializeField]
    public UnityEvent<Vector2, bool> OnMoved { get; private set; }
    
    [field: SerializeField]
    public UnityEvent<bool> OnWaterEvent { get; private set; }
    
    Transform followTarget;

    bool isOnWater;
    bool isMoving;
    Vector2 direction;
    Vector3 lastVelocity;

    public bool IsOnWater
    {
        get => isOnWater;
        private set
        {
            // if (isOnWater == value)
            //     return;

            isOnWater = value;
            OnWaterEvent.Invoke(isOnWater);
        }
    }
    
    public bool IsMoving
    {
        get
        {
            if (!Agent
                || !Agent.enabled
                || !isActiveAndEnabled)
                return false;
            return isMoving;
        }

        private set
        {
            isMoving = value;
            
            OnMoved.Invoke(Agent.velocity, isMoving);
        }
    }
    
    public float Speed
    {
        get => Agent.speed;
        set => Agent.speed = value;
    }
    
    public void Disable()
    {
        Agent.enabled = false;
        enabled = false;
    }
    
    public void Enable()
    {
        Agent.enabled = true;
        enabled = false;
    }
    
    public void Move(Vector2 toDirection)
    { 
        direction = toDirection;
        
        if(!Agent.hasPath)
            return;
        
        Agent.ResetPath();
    }

    IEnumerator Start()
    {
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        yield return new WaitUntil(() => Agent.isOnNavMesh);
        
        CheckWaterMask();
        Moved();
    }

    void FixedUpdate()
    {
        CheckWaterMask();
        
        if (followTarget)
        {
            FollowTarget();
            return;
        }
        
        Agent.velocity = direction * Agent.speed;
        Moved();
    }
    
    public bool MoveTo(Vector3 position)
    {
        StopFollowing();
        return Agent.SetDestination(position);
    }
    
    public bool WarpTo(Vector3 position)
    {
        StopFollowing();
        return Agent.Warp(position);
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
    
    public void StopFollowing()
    {
        followTarget = null;
        Agent.ResetPath();
    }

    void FollowTarget()
    {
        _ = Agent.SetDestination(followTarget.position);
        
        Moved();
    }
    
    void CheckWaterMask()
    {
        var waterMask = NavMesh.GetAreaFromName("Water");

        IsOnWater = !NavMesh.SamplePosition(Agent.transform.position, out _, 0.1f, waterMask);
    }

    void Moved()
    {
        if(Agent.velocity == lastVelocity)
            return;

        lastVelocity = Agent.velocity;
        IsMoving = Agent.velocity.magnitude > 0.1f;
    }

    void OnDisable()
    {
        direction = Vector3.zero;
    }

#if UNITY_EDITOR
    void Reset()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
#endif
}