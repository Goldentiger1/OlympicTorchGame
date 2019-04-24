using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandEngine : Hand
{
    #region VARIABLES

    private readonly int enemyLayerIndex = 13;

    //private readonly float minHitVelocityMagnitude = 2f;

    #endregion VARIABLES

    #region UNITY_FUNCTIONS

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == enemyLayerIndex) 
        {
            if (Player.instance.rigSteamVR.activeSelf) 
            {
                // !!!!!!!
                //var currentVelocityMagnitude = GetTrackedObjectVelocity().magnitude;
                //print(currentVelocityMagnitude);

                //if(currentVelocityMagnitude >= minHitVelocityMagnitude) 
                //{
                //    PlayerEngine.Instance.Bubi.Change_AI_State(AI_STATE.WITHDRAW);
                //}
            }            

            PlayerEngine.Instance.Bubi.Change_AI_State(AI_STATE.WITHDRAW);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        
    }

    #endregion UNITY_FUNCTIONS
}
