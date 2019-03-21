using UnityEngine;

public enum GAME_STATE
{
    STARTING,
    RUNNING,
    ENDED
}

public class GameManager : MonoBehaviour
{
    public float LevelTime = 60f;

    public GAME_STATE CurrentGameState { get; private set; }

    private void ChangeGameState(GAME_STATE newState) 
    {
        CurrentGameState = newState;

        switch (CurrentGameState)
        {
            case GAME_STATE.STARTING:

                break;

            case GAME_STATE.RUNNING:

                break;

            case GAME_STATE.ENDED:

                break;

            default:

                break;
        }
    }

    private void Update()
    {
            
    }
}
