using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandEngine : Hand
{
    #region VARIABLES



    private readonly int enemyLayerIndex = 13;
    private readonly float cooldownDuration = 4f;
    private bool canAttack = true;

    //private readonly float minHitVelocityMagnitude = 2f;

    #endregion VARIABLES

    #region UNITY_FUNCTIONS

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == enemyLayerIndex && canAttack) 
        {
            // CheckHitForce();        

            PlayerEngine.Instance.Bubi.Change_AI_State(AI_STATE.WITHDRAW);

            canAttack = false;
            Invoke("AttackCooldown", cooldownDuration);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        
    }

    private void CheckHitForce()
    {
        if (Player.instance.rigSteamVR.activeSelf == false)
            return;

            // !!!!!!!
            //var currentVelocityMagnitude = GetTrackedObjectVelocity().magnitude;
            //print(currentVelocityMagnitude);

            //if(currentVelocityMagnitude >= minHitVelocityMagnitude) 
            //{
            //    PlayerEngine.Instance.Bubi.Change_AI_State(AI_STATE.WITHDRAW);
            //}
        }

    private void AttackCooldown()
    {
        canAttack = true;
    }

    #endregion UNITY_FUNCTIONS
}
