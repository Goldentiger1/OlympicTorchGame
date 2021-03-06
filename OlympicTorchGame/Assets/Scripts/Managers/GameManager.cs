﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singelton<GameManager>
{
    #region VARIABLES

    [Header("Variables")]
    public float LevelTime = 60f;
    public float LevelStartDelay = 4f;
    public bool OlympicFlameStarted;
    public bool TimeToStartFire = false;

    [Header("References")]
    public OlympicCauldron OlympicCauldron;

    [Header("Audio")]
    public AudioClip Victory;
    public AudioClip Lose;
    public AudioClip Fanfare;
    public AudioClip StartLevel;

    private bool gameIsCreated;

    #endregion VARIABLES

    #region PROPERTIES

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

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Start()
    {
        StartLevelTime = LevelTime;

        ChangeGameState(GAME_STATE.START);
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

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
            //var playerPrefab = ResourceManager.Instance.PlayerPrefab;
            //var player = Instantiate(playerPrefab);
            //player.name = playerPrefab.name;

            // Bubi
            var bubiPrefab = ResourceManager.Instance.BubiPrefab;
            var bubi = Instantiate(bubiPrefab, transform);
            bubi.name = bubiPrefab.name;

            //// Torch
            //var torchPrefab = ResourceManager.Instance.TorchPrefab;
            //var torch = Instantiate(
            //    torchPrefab,
            //    PlayerEngine.Instance.feetPositionGuess + Vector3.forward,
            //    Quaternion.identity,
            //    transform);
            //torch.name = torchPrefab.name;
        }

        return gameIsCreated = true;
    }

    private IEnumerator IStart()
    {
        OlympicFlameStarted = false;
        TimeToStartFire = false;

        yield return new WaitForSeconds(LevelStartDelay);

        WeatherManager.Instance.StartWeather();

        if (StartLevel != null)
        {
            AudioSource.PlayClipAtPoint(StartLevel, PlayerEngine.Instance.feetPositionGuess);
        }

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

            if (LevelTime <= 0)
            {
                TimeToStartFire = true;

                UIManager.Instance.GameTimeText = "START FIRE";

                OlympicCauldron.ShowHint();
                AudioSource.PlayClipAtPoint(Fanfare, OlympicCauldron.transform.position);

                yield return new WaitUntil(() => OlympicFlameStarted);

                ChangeGameState(GAME_STATE.END);
            }

            yield return null;
        }
    }

    private IEnumerator IEnd()
    {
        OlympicCauldron.HideHint();

        WeatherManager.Instance.ChangeWeatherState(WEATHER_STATE.NONE);

        UIManager.Instance.GameTimeText = OlympicFlameStarted ? "VICTORY!" : "GAME OVER!";
        AudioSource.PlayClipAtPoint(OlympicFlameStarted ? Victory : Lose, PlayerEngine.Instance.feetPositionGuess + Vector3.up);

        yield return new WaitForSeconds(4f);

        UIManager.Instance.GameTimeText = "RESTARTING...";

        yield return new WaitForSeconds(4f);

        LoadScene(CurrentSceneIndex);

        //ChangeGameState(GAME_STATE.START);
    }

    #endregion CUSTOM_FUNCTIONS
}