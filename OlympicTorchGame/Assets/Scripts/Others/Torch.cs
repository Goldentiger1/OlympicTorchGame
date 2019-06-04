using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    #region VARIABLES

    [Header("Variables")]
    [Range(0, 100)]
    public float FlameStrenght = 100f;
    public float BurnRateMultiplier = 4f;
    public float IgniteCauldronDuration = 6f;
    public float RayMaDistance = 100f;
    public LayerMask HitLayerMask;
    private float currentFireStartDuration;
    private float startFlameStrenght;

    [Header("UI")]
    public Image FireCounterImage;
    public Image FireStrenghtImage_Fill;

    private ParticleSystem torchFlameEffect;
    private ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule;

    private float startRateOverTime;
    public float SliderValue;

    private Coroutine iStartLifeTime, iStartFire;

    private bool startingFire;

    #endregion VARIABLES

    #region PROPERTIES

    public Transform FirePoint
    {
        get;
        private set;
    }

    public bool IsBurning
    {
        get
        {
            return FlameStrenght > 0;
        }
    }

    public bool IsHandInsideFire
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        FirePoint = transform.Find("FirePoint");
    }

    private void Start()
    {
        currentFireStartDuration = IgniteCauldronDuration;
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
                StartFire(other.bounds.center, IgniteCauldronDuration);
            }

            break;

            // Hand layer index
            case 14:

            IsHandInsideFire = true;

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

                GameManager.Instance.OlympicCauldron.HideHint();
            }

                break;

            // Hand layer index
            case 14:

            IsHandInsideFire = false;

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
        SpawnFireEffect(ResourceManager.Instance.FireEffectPrefab, FirePoint.position, FirePoint);

        torchFlameEffect = FirePoint.GetComponentInChildren<ParticleSystem>();

        var emission = torchFlameEffect.emission;
        startRateOverTime = emission.rateOverTime.constant;

        var ratio = FlameStrenght / startFlameStrenght;

        var origin = WeatherManager.Instance.WindSource.transform.position;
        var direction = WeatherManager.Instance.WindSource.transform.forward;

        var ray = new Ray(origin, direction);

        var hit = new RaycastHit();

        velocityOverLifetimeModule = torchFlameEffect.velocityOverLifetime;

        while (IsBurning)
        {
            origin = WeatherManager.Instance.WindSource.transform.position;
            direction = WeatherManager.Instance.WindSource.transform.forward;

            ray.origin = origin;
            ray.direction = direction;
           
            if (Physics.Raycast(origin, direction, out hit, RayMaDistance, HitLayerMask) && IsHandInsideFire == false)
            {
                Debug.DrawLine(origin, FirePoint.transform.position, Color.red);

                torchFlameEffect.transform.rotation = Quaternion.Euler(Vector3.zero);

                if (hit.collider != null)
                {
                    print(hit.collider.name);


                }
            } 
            else
            {
                ratio = FlameStrenght / startFlameStrenght;
                UIManager.Instance.UpdateTorchStrenght(FlameStrenght, ratio);
                FireStrenghtImage_Fill.fillAmount = ratio;

                emission.rateOverTime = ratio * startRateOverTime;

                FlameStrenght -= Time.deltaTime * BurnRateMultiplier;

                Debug.DrawLine(origin, FirePoint.transform.position, Color.white);

                torchFlameEffect.transform.rotation = Quaternion.LookRotation(WeatherManager.Instance.WindSource.transform.forward);
            }

            

            // Debug.DrawRay(ray.origin, ray.direction * RayMaDistance, isEffeecting ? Color.red : Color.white);


            yield return null;
        }

        UIManager.Instance.UpdateTorchStrenght(FlameStrenght, 0f);
        FirePoint.gameObject.SetActive(false);
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
            ratio = currentFireStartDuration / IgniteCauldronDuration;

            currentFireStartDuration -= Time.deltaTime;

            if (currentFireStartDuration <= 0)
            {
                FireCounterImage.fillAmount = 0f;

                yield return new WaitUntil(() => SpawnFireEffect(ResourceManager.Instance.BigFireEffectPrefab, position, GameManager.Instance.OlympicCauldron.transform));

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
