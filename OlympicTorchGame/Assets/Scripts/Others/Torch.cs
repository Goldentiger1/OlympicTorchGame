using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    [Header("Variables")]
    public float FlameStrenght = 100f;
    public float FireStartDuration = 6f;
    private float currentFireStartDuration;

    [Header("UI")]
    public Image FireCounterImage;

    private Transform flamingPart;

    private Coroutine iStartLifeTime, iStartFire;

    private bool startingFire;

    public bool IsBurning
    {
        get
        {
            return FlameStrenght > 0;
        }
    }

    private void Awake()
    {
        flamingPart = transform.Find("FlamingPart");
    }

    private void Start()
    {
        currentFireStartDuration = FireStartDuration;

        StartLifeTime();
    }

    public void ModifyFlameStrenght(float value)
    {
        var newStrenght = FlameStrenght + value;
        FlameStrenght = newStrenght < 0 ? 0 : newStrenght;
    }

    private void StartLifeTime() 
    {
        if (iStartLifeTime == null)
            iStartLifeTime = StartCoroutine(ILifeTime());
    }

    private void StartFire(Vector3 position, float duration) 
    {
        if (iStartFire == null)
            iStartFire = StartCoroutine(IStartFire(position, duration));
    }

    private void OnTriggerEnter(Collider other) 
    {     
        var layer = other.gameObject.layer;

        switch (layer)
        {
            // FirePoint layer index
            case 11:

                if (GameManager.Instance.OlympicFlameStarted)
                    return;

                if (GameManager.Instance.TimeToStartFire) 
                {
                    StartFire(other.bounds.center, FireStartDuration);
                }

                break;

            default:

                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var layer = other.gameObject.layer;

        switch (layer)
        {
            // FirePoint layer index
            case 11:

                if (GameManager.Instance.OlympicFlameStarted)
                    return;

                if (GameManager.Instance.TimeToStartFire) 
                {
                    startingFire = false;
                    FireCounterImage.enabled = false;
                }

                break;

            default:

                break;
        }
    }

    private bool SpawnFireEffect(Vector3 position) 
    {
        var bigFirePrefab = ResourceManager.Instance.BigFireEffect;

        var bigFire = Instantiate(bigFirePrefab, position, Quaternion.identity);
        bigFire.name = bigFirePrefab.name;

        return GameManager.Instance.OlympicFlameStarted = true;
    }

    private IEnumerator ILifeTime()
    {
        while (IsBurning)
        {
            FlameStrenght -= Time.deltaTime;
            yield return null;
            UIManager.Instance.UpdateTorchStrenght(FlameStrenght);
        }

        flamingPart.gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GAME_STATE.END);

        iStartLifeTime = null;
    }

    private IEnumerator IStartFire(Vector3 position, float duration) 
    {
        startingFire = true;
        FireCounterImage.enabled = true;

        var ratio = 0f;

        while (startingFire) 
        {
            ratio = currentFireStartDuration / FireStartDuration;

            currentFireStartDuration -= Time.deltaTime;

            if (currentFireStartDuration <= 0) 
            {
                FireCounterImage.fillAmount = 0f;

                yield return new WaitUntil(() => SpawnFireEffect(position));
                
                break;
            }

            FireCounterImage.fillAmount = ratio;
            print("Current duration: " + currentFireStartDuration);
            print("Fill amount: " + FireCounterImage.fillAmount);

            yield return null;
        }

        iStartFire = null;
    }
}
