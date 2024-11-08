using System;
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
    
    Transform followTarget;
    
    bool isInWater;
    Vector2 direction;
    Vector3 lastVelocity;
    
    public float Speed
    {
        get => Agent.speed;
        set => Agent.speed = value;
    }
    
    public void Move(Vector2 toDirection)
    { 
        direction = toDirection;
        
        if(!Agent.hasPath)
            return;
        
        Agent.ResetPath();
    }

    void Start()
    {
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    private void FixedUpdate()
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

    public bool IsPlayerWalking()
    {
        return Agent.velocity.magnitude > 0.1f;
    }
    
    private void CheckWaterMask()
    {
        var waterMask = NavMesh.GetAreaFromName("Water");

        isInWater = !NavMesh.SamplePosition(Agent.transform.position, out _, 0.1f, waterMask);
    }

    void Moved()
    {
        if(Agent.velocity == lastVelocity)
            return;

        lastVelocity = Agent.velocity;
        OnMoved.Invoke(Agent.velocity, isInWater);
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