using System.Collections;
using UnityEngine;

public class WeatherManager : Singelton<WeatherManager>
{
    #region VARIABLES

    [Header("General")]
    public bool ShowGizmos = true;
    public WEATHER_STATE WeatherState;

    [Header("Wind")]
    public Vector3[] WindPoints;
    private Vector3 currentWindPoint;

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

    public bool WeHitCoverZone
    {
        get 
        {
            if(Physics.Raycast(WindSource.transform.position, Vector3.forward * 100f))
            {
                
            }

            return false;
        }
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartCoroutine(IChangeWind());
    }

    private void OnDrawGizmos()
    {
        if (ShowGizmos)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < WindPoints.Length; i++)
            {
                currentWindPoint = WindPoints[i];
                Gizmos.DrawLine(currentWindPoint + Vector3.forward * 0.2f, currentWindPoint + Vector3.back * 0.2f);
                Gizmos.DrawLine(currentWindPoint + Vector3.right * 0.2f, currentWindPoint + Vector3.left * 0.2f);
            }

            

        }
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

    private void Foo()
    {
      
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
            print(rainDropsEffect.isStopped);
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
        //    windDirection = newDirection;
        //    windSpeed = newSpeed;

        var newOrbitalValue = velocityOverLifeTimeModule.orbitalOffsetX;
        newOrbitalValue.constant = 10f;
        velocityOverLifeTimeModule.orbitalOffsetX = newOrbitalValue;
    }

    private void ChangeWindSourcePosition(Vector3 position)
    {
        WindSource.transform.position = position;

        // Rotate local forward vector to look at World center position
        WindSource.transform.LookAt(Vector3.zero);
        // Convert direction -> local to world
        WindSource.transform.InverseTransformDirection(WindSource.transform.localPosition);
    }

    private IEnumerator IChangeWind()
    {
        while (currentWeatherState.Equals(WEATHER_STATE.NONE) == false)
        {
            var nextWind = Random.Range(2f, 6f);

            var randomWindPosition = WindPoints[Random.Range(0, WindPoints.Length)];
            ChangeWindSourcePosition(randomWindPosition);

            yield return new WaitForSeconds(nextWind);

            var windDuration = Random.Range(2f, 3f);

            // FIX ME!!

            //ModifyWind(currentWindPoint, 20f);

            yield return new WaitForSeconds(windDuration);
        }
    }

    #endregion CUSTOM_FUNCTIONS
}
