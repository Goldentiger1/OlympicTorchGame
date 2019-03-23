using System.Collections;
using UnityEngine;

public enum GAME_STATE
{
    START,
    RUN,
    END
}

public class GameManager : Singelton<GameManager>
{
    public float LevelTime = 60f;
    private float startLevelTime;
    private bool gameIsCreated;

    public GAME_STATE CurrentGameState
    {
        get;
        private set;
    }

    private void Start()
    {
        startLevelTime = LevelTime;

        ChangeGameState(GAME_STATE.START);
    }

    private void ChangeGameState(GAME_STATE newState) 
    {
        CurrentGameState = newState;

        switch (CurrentGameState)
        {
            case GAME_STATE.START:

                StartCoroutine(IStart());

                break;

            case GAME_STATE.RUN:

                StartCoroutine(IRun());

                break;

            case GAME_STATE.END:

                StartCoroutine(IEnd());

                break;

            default:

                break;
        }
    }

    private bool CreateLevelObjects()
    {
        if (gameIsCreated == false)
        {
            var player = Instantiate(ResourceManager.Instance.PlayerPrefab, transform);
            player.SetActive(false);

            Instantiate(ResourceManager.Instance.BubiPrefab, transform);
            Instantiate(ResourceManager.Instance.TorchPrefab, transform);
            Instantiate(ResourceManager.Instance.RainEffectPrefab, transform);
        }     

        return gameIsCreated = true;
    }

    private IEnumerator IStart()
    {
        LevelTime = startLevelTime;
        UIManager.Instance.GameTimeText.text = "STARTING...";

        yield return new WaitUntil(() => CreateLevelObjects());

        ChangeGameState(GAME_STATE.RUN);
    }

    private IEnumerator IRun()
    {
        while (CurrentGameState.Equals(GAME_STATE.RUN))
        {
            yield return new WaitForSeconds(1f);
            LevelTime--;
            UIManager.Instance.UpdateGameTime(LevelTime);
         
            if(LevelTime <= 0)
            {
                ChangeGameState(GAME_STATE.END);
                break;
            }
        }
    }

    private IEnumerator IEnd()
    {
        UIManager.Instance.GameTimeText.text = "GAME OVER";

        yield return new WaitForSeconds(4f);

        ChangeGameState(GAME_STATE.START);
    }
}
