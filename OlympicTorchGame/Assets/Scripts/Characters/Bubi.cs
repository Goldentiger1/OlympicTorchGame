using System.Collections;
using UnityEngine;

public class Bubi : Bird
{
    private Coroutine idle, roam, attack, withdraw;

    [Header("Audio")]
    public AudioClip AttackClip;
    public AudioClip HitClip;

    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() 
    {
        transform.position = Waypoints[Random.Range(0, Waypoints.Length)];
        Change_AI_State(AI_STATE.ROAM);
    }

    protected override void Idle() 
    {
        base.Idle();

        if (idle == null)
            idle = StartCoroutine(IIdle());
    }

    protected override void Roam() 
    {
        base.Roam();

        if (roam == null)
            roam = StartCoroutine(IRoam());
    }

    protected override void Attack() 
    {
        base.Attack();

        if (attack == null)
            attack = StartCoroutine(IAttack());
    }

    protected override void Withdraw() 
    {
        base.Withdraw();

        if(withdraw == null)
            withdraw = StartCoroutine(IWithdraw());
    }

    #region COROUTINES

    private IEnumerator IIdle() 
    {
        while (Current_AI_State.Equals(AI_STATE.IDLE)) 
        {
            yield return null;
        }

        idle = null;
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

        roam = null;
    }

    private IEnumerator IAttack()
    {
        if(audioSource.isPlaying == false) 
        {
            audioSource.clip = AttackClip;
            audioSource.volume = Random.Range(0.9f, 1.1f);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    
        while (Current_AI_State.Equals(AI_STATE.ATTACK)) 
        {
            if (Target == null) 
            {
                Target = PlayerEngine.Instance._Torch.transform;

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

                PlayerEngine.Instance._Torch.ModifyFlameStrenght(-50f);

                yield return new WaitForSeconds(2f);

                SetClosestWaypoint();

                Change_AI_State(AI_STATE.ROAM);
                break;
            }

            yield return null;
        }

        attack = null;
    }

    private IEnumerator IWithdraw() 
    {
        if (audioSource.isPlaying == false) 
        {
            audioSource.clip = HitClip;
            audioSource.volume = Random.Range(0.9f, 1.1f);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }

        while (Current_AI_State.Equals(AI_STATE.WITHDRAW)) 
        {
            animator.SetTrigger("Hit");
            yield return new WaitForSeconds(1f);
            Change_AI_State(AI_STATE.ROAM);

        }

        withdraw = null;
    }

    #endregion
}
