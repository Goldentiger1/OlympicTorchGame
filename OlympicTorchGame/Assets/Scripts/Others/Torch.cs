using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    #region VARIABLES

    [Header("Variables")]
    [Range(0, 100)]
    public BoxCollider CoverZones;
    public float FlameStrenght = 100f;
    public float FireStartDuration = 6f;
    private float currentFireStartDuration;
    private float startFlameStrenght;

    [Header("UI")]
    public Image FireCounterImage;
    public Image FireStrenghtImage_Fill;

    private Transform flamingPart;
    private ParticleSystem torchFlameEffect;

    private float startRateOverTime;
    public float SliderValue;

    private Coroutine iStartLifeTime, iStartFire;

    private bool startingFire;

    #endregion VARIABLES

    #region PROPERTIES

    public bool IsBurning
    {
        get
        {
            return FlameStrenght > 0;
        }
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        flamingPart = transform.Find("FlamingPart");
    }

    private void Start()
    {
        currentFireStartDuration = FireStartDuration;
        startFlameStrenght = FlameStrenght;

        FireCounterImage.enabled = false;

        StartLifeTime();
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

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private bool SpawnFireEffect(GameObject fireEffectPrefab, Vector3 position, Transform parent)
    {
        var fireEffectInstance = Instantiate(fireEffectPrefab, position, Quaternion.identity, parent);
        fireEffectInstance.name = fireEffectPrefab.name;

        return true;
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

    public void ModifyFlameStrenght(float value)
    {
        var newStrenght = FlameStrenght + value;
        FlameStrenght = newStrenght < 0 ? 0 : newStrenght;
    }

    private IEnumerator ILifeTime()
    {
        SpawnFireEffect(ResourceManager.Instance.FireEffectPrefab, flamingPart.position, flamingPart);

        torchFlameEffect = flamingPart.GetComponentInChildren<ParticleSystem>();
        var emission = torchFlameEffect.emission;
        startRateOverTime = emission.rateOverTime.constant;

        var ratio = FlameStrenght / startFlameStrenght;

        while (IsBurning)
        {
            FlameStrenght -= Time.deltaTime;

            ratio = FlameStrenght / startFlameStrenght;

            UIManager.Instance.UpdateTorchStrenght(FlameStrenght, ratio);
            FireStrenghtImage_Fill.fillAmount = ratio;
   
            emission.rateOverTime = ratio * startRateOverTime;
            //print(emission.rateOverTime.constant);

            yield return null;
        }

        UIManager.Instance.UpdateTorchStrenght(FlameStrenght, 0f);
        flamingPart.gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GAME_STATE.END);

        iStartLifeTime = null;
    }

    private IEnumerator IStartFire(Vector3 position, float duration)
    {
        startingFire = true;
        FireCounterImage.enabled = true;

        var ratio = 0f;

        var target = PlayerEngine.Instance.hmdTransform;

        while (startingFire && IsBurning)
        {
            ratio = currentFireStartDuration / FireStartDuration;

            currentFireStartDuration -= Time.deltaTime;

            if (currentFireStartDuration <= 0)
            {
                FireCounterImage.fillAmount = 0f;

                yield return new WaitUntil(() => SpawnFireEffect(ResourceManager.Instance.BigFireEffectPrefab, position, GameManager.Instance.OlympicCauldron));

                GameManager.Instance.OlympicFlameStarted = true;

                break;
            }

            FireCounterImage.fillAmount = ratio;

            FireCounterImage.transform.LookAt(target.position);

            yield return null;
        }

        iStartFire = null;
    }

    #endregion CUSTOM_FUNCTIONS
}
