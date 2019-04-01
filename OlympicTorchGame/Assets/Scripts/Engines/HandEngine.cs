using Valve.VR.InteractionSystem;

public class HandEngine : Hand
{
    private readonly int enemyLayerIndex = 13;

    private readonly float minHitVelocityMagnitude = 2f;

    private void OnTriggerEnter(UnityEngine.Collider other) 
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
            else 
            {
                
            }

            PlayerEngine.Instance.Bubi.Change_AI_State(AI_STATE.WITHDRAW);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other) 
    {
        
    }
}
