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
    private int waypointIndex;

    private Animator animator;

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

    private void Idle()
    {
        StartCoroutine(IIdle());
    }

    private void Roam()
    {
        StartCoroutine(IRoam());
    }

    private void Attack()
    {
        StartCoroutine(IAttack());
    }

    private int GetClosestWaypoint()
    {
        var closestDistance = Mathf.Infinity;
        var closestWaypointIndex = 0;

        for (int i = 0; i < Waypoints.Length; i++)
        {
            float dist = Vector3.Distance(Waypoints[i], transform.position);
            if (dist < closestDistance)
            {
                closestWaypointIndex = i;
            }
        }

        return closestWaypointIndex;
    }

    private void Move(Vector3 target, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    private void Rotate(Vector3 target, float rotationSpeed)
    {
        var targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region COROUTINES

    private IEnumerator IIdle()
    {
        while (Current_AI_State.Equals(AI_STATE.IDLE))
        {
            yield return null;
        }     
    }

    private IEnumerator IRoam()
    {
        while (Current_AI_State.Equals(AI_STATE.ROAM))
        {
            var destination = Waypoints[waypointIndex];

            Move(destination, MoveSpeed);
            Rotate(destination, RotationSpeed);

            if (Vector3.Distance(transform.position, destination) <= 0.1f)
            {
                waypointIndex = waypointIndex == Waypoints.Length - 1 ? 0 : waypointIndex + 1;
            }

            yield return null;
        }
    }   

    private IEnumerator IAttack()
    {
        while (Current_AI_State.Equals(AI_STATE.ATTACK))
        {
            if(Target == null)
            {
                Target = FindObjectOfType<Torch>().transform;
            }

            var targetPosition = Target.position;

            Rotate(targetPosition, RotationSpeed * 2);
            Move(targetPosition, MoveSpeed * 2);

            var distance = Vector3.Distance(transform.position, targetPosition);

            if(distance <= 1f)
            {
                animator.SetTrigger("Attack");

 

                //var foo = animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");
                //var foo2 = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

                //print(foo + " " + foo2);

                yield return new WaitForSeconds(2f);
                // !!!

                waypointIndex = 2;

                //waypointIndex = GetClosestWaypoint();
                Change_AI_State(AI_STATE.ROAM);
                break;
            }

            yield return null;
        }       
    }

    #endregion
}
