using System.Collections;
using UnityEngine;

public class Bubi : Bird
{
    private Vector3 destination;

    public bool DestinationReached 
    {
        get 
        {
            return Vector3.Distance(transform.position, destination) <= 0.1f;
        }
    }

    protected override void Awake()
    {
        base.Awake();      
    }

    private void Start() 
    {
        transform.position = Waypoints[Random.Range(0, Waypoints.Length)];
        Change_AI_State(AI_STATE.ROAM);
    }

    protected override void Idle() 
    {
        base.Idle();

        StartCoroutine(IIdle());
    }

    protected override void Roam() 
    {
        base.Roam();

        StartCoroutine(IRoam());
    }

    protected override void Attack() 
    {
        base.Attack();

        StartCoroutine(IAttack());
    }

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
        destination = Waypoints[waypointIndex];

        var attackTime = Random.Range(6f, 10f);


        while (Current_AI_State.Equals(AI_STATE.ROAM)) 
        {
            if(attackTime > 0f) 
            {
                attackTime -= Time.deltaTime;
            }
            else 
            {
                print("ATTACK");
                Change_AI_State(AI_STATE.ATTACK);
            }

            Move(destination, MoveSpeed);
            Rotate(destination, RotationSpeed);

            if (DestinationReached)
            {
                waypointIndex = waypointIndex == Waypoints.Length - 1 ? 0 : waypointIndex + 1;
                destination = Waypoints[waypointIndex];
            }

            yield return null;
        }
    }

    private IEnumerator IAttack()
    {
        while (Current_AI_State.Equals(AI_STATE.ATTACK)) 
        {
            if (Target == null) 
            {
                Target = FindObjectOfType<Torch>().transform;

                if(Target == null) 
                {
                    Debug.LogError("No target...");
                    break;
                }
            }

            var targetPosition = Target.position;

            Rotate(targetPosition, RotationSpeed * 2);
            Move(targetPosition, MoveSpeed * 2);

            var distance = Vector3.Distance(transform.position, targetPosition);

            if (distance <= 1f) 
            {
                animator.SetTrigger("Attack");

                yield return new WaitForSeconds(2f);

                GetClosestWaypoint();

                Change_AI_State(AI_STATE.ROAM);
                break;
            }

            yield return null;
        }
    }

    #endregion
}
