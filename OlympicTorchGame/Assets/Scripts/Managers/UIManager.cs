using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singelton<UIManager>
{
    #region VARIABLES

    [Header("HUD Panel Variables")]
    public Vector3 Offset;
    public float SmoothMultiplier;

    private Coroutine iShowHUD;

    private Image gameTimeFillImage; 
    private Image torchStrenghtFillImage; 
    private TextMeshProUGUI gameTimeText;
    private TextMeshProUGUI torchStrenghtText;

    private Transform gameTimePanel, torchStrenghtPanel;

    #endregion VARIABLES

    #region PROPERTIES

    public string GameTimeText
    {
        get
        {
            return gameTimeText.text;
        }
        set
        {
            gameTimeText.text = value;
        }
    }
    public string TorchStrenghtText
    {
        get
        {
            return torchStrenghtText.text;
        }
        set
        {
            torchStrenghtText.text = value;
        }
    }

    public Vector3 HudStartPosition
    {
        get
        {
            return PlayerEngine.Instance.feetPositionGuess + Vector3.forward;
        }
    }

    public Transform HUDCanvas
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        Initialize();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.CurrentGameState.Equals(GAME_STATE.RUN));

        ShowHUD(HudStartPosition, 1f, GameManager.Instance.StartLevelTime);
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void Initialize()
    {
        HUDCanvas = transform.Find("HUDCanvas");

        var gameStats = HUDCanvas.Find("GameStats");

        gameTimePanel = gameStats.Find("GameTimePanel");
        torchStrenghtPanel = gameStats.Find("TorchStrenghtPanel");

        gameTimeFillImage = gameTimePanel.GetComponentInChildren<Image>();
        torchStrenghtFillImage = torchStrenghtPanel.GetComponentInChildren<Image>();
        gameTimeText = gameTimePanel.GetComponentInChildren<TextMeshProUGUI>();
        torchStrenghtText = torchStrenghtPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateGameTime(float currentTime, float ratio)
    {
        gameTimeFillImage.fillAmount = ratio;
        GameTimeText = "TIME: " + currentTime.ToString("0");
    }

    public void UpdateTorchStrenght(float currentStrenght, float ratio)
    {
        torchStrenghtFillImage.fillAmount = ratio;
        TorchStrenghtText = "TORCH: " + currentStrenght.ToString("0");
    }

    public void ShowHUD(Vector3 startPosition, float showDelay = 2f, float showDuration = 20f)
    {
        if (iShowHUD == null)
        {
            iShowHUD = StartCoroutine(IShowHUD(startPosition, showDelay, showDuration));
        }
    }

    public void HideHUD()
    {
        if (iShowHUD != null)
        {
            StopCoroutine(iShowHUD);
        }

        HUDCanvas.gameObject.SetActive(false);
    }

    private void MoveHUD(Transform target, Transform element, Vector3 offset, float smoothMultiplier = 1f)
    {
        var desiredPosition = new Vector3(
            target.position.x,
            element.position.y,
            target.position.z
            );

        element.position = Vector3.Lerp(
            element.position,
            desiredPosition + (target.forward * offset.z),
            Time.deltaTime * SmoothMultiplier);

        element.position = new Vector3(
            element.position.x,
            offset.y,
            element.position.z
            );
    }

    private void RotateHUD(Transform target, Transform element)
    {
        element.LookAt(
            new Vector3(
            target.position.x,
            element.position.y,
            target.position.z)
            );
    }

    private IEnumerator IShowHUD(Vector3 startPosition, float showDelay, float showDuration)
    {
        yield return new WaitForSeconds(showDelay);

        HUDCanvas.position = startPosition;
        //HUDCanvas.gameObject.SetActive(true);

        StartCoroutine(IUpdateHUDMotion());

        yield return new WaitForSeconds(showDuration);

        iShowHUD = null;
    }

    private IEnumerator IUpdateHUDMotion()
    {
        var target = PlayerEngine.Instance.hmdTransform;

        while (HUDCanvas.gameObject.activeSelf)
        {
            MoveHUD(target, HUDCanvas, Offset, SmoothMultiplier);
            RotateHUD(target, HUDCanvas);

            yield return null;
        }
    }

    #endregion CUSTOM_FUNCTIONS
}
