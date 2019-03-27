﻿using System.Collections;
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
    [Header("VARIABLES")]
    public float LevelTime = 60f;
    public bool OlympicFlameStarted;
    public bool UseVR_Player = true;
    public bool TimeToStartFire = false;

    private float startLevelTime;
    private bool gameIsCreated;
    private Camera mainCamera;

    public GAME_STATE CurrentGameState
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

    private void Awake() 
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        mainCamera.gameObject.SetActive(!UseVR_Player);

        startLevelTime = LevelTime;

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
            player.SetActive(UseVR_Player);

            // Bubi
            var bubiPrefab = ResourceManager.Instance.BubiPrefab;
            var bubi = Instantiate(bubiPrefab, transform);
            bubi.name = bubiPrefab.name;

            // Torch
            var torchPrefab = ResourceManager.Instance.TorchPrefab;
            var torch = Instantiate(torchPrefab, transform);
            torch.name = torchPrefab.name;

            // Rain
            var rainEffectPrefab = ResourceManager.Instance.RainEffectPrefab;
            var rainEffect = Instantiate(rainEffectPrefab, transform);
            rainEffect.name = rainEffectPrefab.name;
        }     

        return gameIsCreated = true;
    }

    private IEnumerator IStart()
    {
        OlympicFlameStarted = false;
        TimeToStartFire = false;

        LevelTime = startLevelTime;
        UIManager.Instance.GameTimeText.text = "STARTING...";

        yield return new WaitUntil(() => CreateLevelObjects());

        ChangeGameState(GAME_STATE.RUN);
    }

    private IEnumerator IRun()
    {
        while (CurrentGameState.Equals(GAME_STATE.RUN))
        {
            LevelTime -= Time.deltaTime;
            UIManager.Instance.UpdateGameTime(LevelTime);        

            if(LevelTime <= 0)
            {
                TimeToStartFire = true;

                UIManager.Instance.GameTimeText.text = "START FIRE";

                yield return new WaitUntil(() => OlympicFlameStarted);

                ChangeGameState(GAME_STATE.END);
            }

            yield return null;
        }
    }

    private IEnumerator IEnd()
    {
        UIManager.Instance.GameTimeText.text = "GAME OVER";

        yield return new WaitForSeconds(4f);

        LoadScene(CurrentSceneIndex);

        //ChangeGameState(GAME_STATE.START);
    }
}
