using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE
{
    START,
    RUN,
    END
}

public class GameManager : Singelton<GameManager>
{
    [Header("Variables")]
    public float LevelTime = 60f;
    public bool OlympicFlameStarted;
    public bool TimeToStartFire = false;

    private bool gameIsCreated;

    public GAME_STATE CurrentGameState
    {
        get;
        private set;
    }

    public float StartLevelTime
    {
        get;
        private set;
    }

    public int CurrentSceneIndex
    {
        get
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }

    private void Start()
    {
        StartLevelTime = LevelTime;

        ChangeGameState(GAME_STATE.START);
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);     
    }

    public void ChangeGameState(GAME_STATE newState) 
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
            // Player
            var playerPrefab = ResourceManager.Instance.PlayerPrefab;
            var player = Instantiate(playerPrefab, transform);
            player.name = playerPrefab.name;

            // Bubi
            var bubiPrefab = ResourceManager.Instance.BubiPrefab;
            var bubi = Instantiate(bubiPrefab, transform);
            bubi.name = bubiPrefab.name;

            // Torch
            var torchPrefab = ResourceManager.Instance.TorchPrefab;
            var torch = Instantiate(
                torchPrefab, 
                PlayerEngine.Instance.feetPositionGuess + Vector3.forward,
                Quaternion.identity,
                transform);
            torch.name = torchPrefab.name;

            WeatherManager.Instance.ChangeWeatherState(WEATHER_STATE.LIGHT);
        }     

        return gameIsCreated = true;
    }

    private IEnumerator IStart()
    {
        OlympicFlameStarted = false;
        TimeToStartFire = false;

        LevelTime = StartLevelTime;
        UIManager.Instance.GameTimeText = "STARTING...";

        yield return new WaitUntil(() => CreateLevelObjects());

        ChangeGameState(GAME_STATE.RUN);
    }

    private IEnumerator IRun()
    {
        var ratio = LevelTime / StartLevelTime;

        while (CurrentGameState.Equals(GAME_STATE.RUN))
        {
            ratio = LevelTime / StartLevelTime;
            LevelTime -= Time.deltaTime;
            UIManager.Instance.UpdateGameTime(LevelTime, ratio);        

            if(LevelTime <= 0)
            {
                TimeToStartFire = true;

                UIManager.Instance.GameTimeText = "START FIRE";

                yield return new WaitUntil(() => OlympicFlameStarted);

                ChangeGameState(GAME_STATE.END);
            }

            yield return null;
        }
    }

    private IEnumerator IEnd()
    {
        UIManager.Instance.GameTimeText = "GAME OVER";

        yield return new WaitForSeconds(4f);

        UIManager.Instance.GameTimeText = "RESTARTING...";

        yield return new WaitForSeconds(4f);

        LoadScene(CurrentSceneIndex);

        //ChangeGameState(GAME_STATE.START);
    }
}
