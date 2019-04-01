using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
    [Header("HUD Variables")]
    public Vector3 offset;
    public float SmoothMultiplier;

    private Coroutine iShowHUD;

    [SerializeField]
    private Image gameTimeFillImage; 
    [SerializeField]
    private Image torchStrenghtFillImage; 
    [SerializeField]
    private TextMeshProUGUI gameTimeText;
    [SerializeField]
    private TextMeshProUGUI torchStrenghtText;

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

    private void Awake()
    {
        HUDCanvas = transform.Find("HUDCanvas");
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.CurrentGameState.Equals(GAME_STATE.RUN)); ;

        ShowHUD(HudStartPosition, 1f, GameManager.Instance.StartLevelTime);
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
        HUDCanvas.gameObject.SetActive(true);

        StartCoroutine(IUpdateHUDMotion());

        yield return new WaitForSeconds(showDuration);

        iShowHUD = null;
    }

    private IEnumerator IUpdateHUDMotion()
    {
        var target = PlayerEngine.Instance.hmdTransform;

        while (HUDCanvas.gameObject.activeSelf)
        {
            MoveHUD(target, HUDCanvas, offset, SmoothMultiplier);
            RotateHUD(target, HUDCanvas);

            yield return null;
        }
    }
}
