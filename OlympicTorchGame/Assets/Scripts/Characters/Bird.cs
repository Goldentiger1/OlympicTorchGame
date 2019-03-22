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

    [Header("Movement variables")]
    public float HorizontalSpeed;
    public float VerticalSpeed;
    public float Amplitude;

    private Vector3 currentPosition;

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

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentPosition = transform.position;
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
            currentPosition.x += HorizontalSpeed;
            currentPosition.y = Mathf.Sin(VerticalSpeed * Time.realtimeSinceStartup) * Amplitude;

            yield return null;
        }
    }

    private IEnumerator IAttack()
    {
        while (Current_AI_State.Equals(AI_STATE.ATTACK))
        {
            animator.SetTrigger("Attack");
            yield return null;
        }       
    }

    #endregion
}
