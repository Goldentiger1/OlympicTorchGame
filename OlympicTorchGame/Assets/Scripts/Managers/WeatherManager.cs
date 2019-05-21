using System.Collections;
using UnityEngine;

public class WeatherManager : Singelton<WeatherManager>
{
    #region VARIABLES

    [Header("General")]
    public bool ShowGizmos = true;
    public WEATHER_STATE WeatherState;

    [Header("Wind")]
    public float Radius;
    public float Frequency = 1;

    [Header("Audio")]
    public AudioClip Rain_light;
    public AudioClip Rain_medium;
    public AudioClip Rain_heavy;
    public AudioClip Wind;
    public AudioSource RainSource;
    public AudioSource WindSource;

    private WEATHER_STATE currentWeatherState;

    private ParticleSystem rainDropsEffect;
    private ParticleSystem rainRipplesEffect;
    private ParticleSystem.EmissionModule dropsModule;
    private ParticleSystem.EmissionModule ripplesModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityOverLifeTimeModule;

    //private Vector3 windDirection;
    //private float windSpeed;

    #endregion VARIABLES

    #region PROPERTIES

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartCoroutine(IStartWind());
    }

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
       
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void Initialize()
    {
        var rainEffectPrefab = ResourceManager.Instance.RainEffectPrefab;
        var rainEffect = Instantiate(rainEffectPrefab, transform).transform;
        rainEffect.name = rainEffectPrefab.name;

        rainDropsEffect = rainEffect.Find("RainDrops").GetComponent<ParticleSystem>();
        rainRipplesEffect = rainEffect.Find("RainRipples").GetComponent<ParticleSystem>();
        dropsModule = rainDropsEffect.emission;
        ripplesModule = rainRipplesEffect.emission;
        velocityOverLifeTimeModule = rainDropsEffect.velocityOverLifetime;
    }

    private void StartRain()
    {
        if (rainDropsEffect.isPlaying == false)
        {
            rainDropsEffect.Play();
        }

        if (rainRipplesEffect.isPlaying == false)
        {
            rainRipplesEffect.Play();
        }

        if (RainSource.isPlaying == false)
        {
            RainSource.Play();
        }
    }

    public void ChangeWeatherState(WEATHER_STATE newWeatherState)
    {
        currentWeatherState = newWeatherState;

        switch (currentWeatherState)
        {
            case WEATHER_STATE.LIGHT:

                ModifyRain(400, 600, 200, 400);

                ModifyWind(Vector3.forward, 0f);

                RainSource.clip = Rain_light;

                break;

            case WEATHER_STATE.MEDIUM:

                ModifyRain(600, 800, 400, 600);

                ModifyWind(Vector3.forward, 0f);

                RainSource.clip = Rain_medium;

                break;

            case WEATHER_STATE.HEAVY:

                ModifyRain(800, 1000, 600, 800);

                ModifyWind(Vector3.forward, 0f);

                RainSource.clip = Rain_heavy;

                break;

            case WEATHER_STATE.NONE:

            StopRain();

                break;

            default:

                break;
        }

        StartRain();
    }

    public void StartWeather()
    {
        ChangeWeatherState(WeatherState);
    }

    private void StopRain()
    {
        if (rainDropsEffect.isPlaying == true)
        {
            rainDropsEffect.Stop();
        }

        if (rainRipplesEffect.isPlaying == true)
        {
            rainRipplesEffect.Stop();
        }

        if (RainSource.isPlaying == true)
        {
            RainSource.Stop();
        }
    }

    private void ModifyRain(int minDropParticles, int maxDropParticles, int minRippleParticles, int maxRippleParticles)
    {
        var tempDropCurve = dropsModule.rateOverTime;
        tempDropCurve.constantMin = minDropParticles;
        tempDropCurve.constantMax = maxDropParticles;
        dropsModule.rateOverTime = tempDropCurve;

        var tempRippleCurve = ripplesModule.rateOverTime;
        tempRippleCurve.constantMin = minRippleParticles;
        tempRippleCurve.constantMax = maxRippleParticles;
        ripplesModule.rateOverTime = tempRippleCurve;
    }

    private void ModifyWind(Vector3 newDirection, float newSpeed)
    {      
        var newOrbitalValue = velocityOverLifeTimeModule.orbitalOffsetX;
        newOrbitalValue.constant = 10f;
        velocityOverLifeTimeModule.orbitalOffsetX = newOrbitalValue;
    }

    private IEnumerator IStartWind()
    {
        while (currentWeatherState.Equals(WEATHER_STATE.NONE) == false)
        {
            var t = Time.time / Frequency;
            float noise = Mathf.PerlinNoise(t, t) * 2 - 1;
            var v = Vector3.forward * Radius;
            var rot = Quaternion.Euler(0, noise * 180, 0);
            WindSource.transform.localPosition = rot * v;

            yield return null;
        }
    }

    #endregion CUSTOM_FUNCTIONS
}
