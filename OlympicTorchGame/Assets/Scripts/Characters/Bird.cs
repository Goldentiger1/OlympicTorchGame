using System;
using System.Collections;
using UnityEngine;

public enum AI_STATE
{
    IDLE,
    ROAM,
    ATTACK
}

public abstract class Bird : MonoBehaviour
{
    #region VARIABLES

    [Header("Movement")]
    public float MoveSpeed = 1f;
    public float RotationSpeed = 5f;
    public Transform Target;

    [Header("Waypoints")]
    public bool ShowWaypoints;
    public Vector3[] Waypoints;
    protected int waypointIndex;

    protected Animator animator;

    #endregion

    #region PROPERTIES

    public AI_STATE Current_AI_State
    {
        get;
        private set;
    }
    public AI_STATE Next_AI_STATE
    {
        get
        {
            var nextEnumValue = Current_AI_State + 1;
            return nextEnumValue >= (AI_STATE)Enum.GetValues(typeof(AI_STATE)).Length ? 0 : nextEnumValue;
        }
    }
    public AI_STATE Previous_AI_State
    {
        get;
        private set;
    }
  
    #endregion

    #region UNITY_FUNCTIONS

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Change_AI_State(Next_AI_STATE);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangePrevious_AI_State();
        }
    }

    private void OnDrawGizmos()
    {
        if (ShowWaypoints)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < Waypoints.Length; i++)
            {
                var waypoint = Waypoints[i];
                Gizmos.DrawLine(waypoint + Vector3.forward * 0.2f, waypoint + Vector3.back * 0.2f);
                Gizmos.DrawLine(waypoint + Vector3.right * 0.2f, waypoint + Vector3.left * 0.2f);
            }
        }
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    public void Change_AI_State(AI_STATE new_AI_State)
    {
        Previous_AI_State = Current_AI_State;
        Current_AI_State = new_AI_State;

        switch (Current_AI_State)
        {
            case AI_STATE.IDLE:

                Idle();

                break;

            case AI_STATE.ROAM:

                Roam();

                break;

            case AI_STATE.ATTACK:

                Attack();

                break;

            default:

                Debug.Log("DEFAULT STATE");
                Change_AI_State(AI_STATE.IDLE);

                break;
        }
    }

    public void ChangePrevious_AI_State()
    {
        Change_AI_State(Previous_AI_State);
    }

    protected virtual void Idle()
    {

    }

    protected virtual void Roam()
    {

    }

    protected virtual void Attack()
    {
        
    }

    protected void GetClosestWaypoint()
    {
        var currentPosition = transform.position;
        var closestDistance = Mathf.Infinity;

        for (int i = 0; i < Waypoints.Length; i++)
        {
            var distanceToWaypoint = Vector3.Distance(currentPosition, Waypoints[i]);

            //print("Distance: " + distanceToWaypoint + "  Index: " + i);

            if (distanceToWaypoint < closestDistance) 
            {     
                closestDistance = distanceToWaypoint;
                waypointIndex = i;              
            }
        }

        //print("Closest index: " + waypointIndex);
    }

    protected void Move(Vector3 target, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    protected void Rotate(Vector3 target, float rotationSpeed)
    {
        var targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion
}
