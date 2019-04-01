using System.Collections;
using UnityEngine;

public enum WEATHER_STATE
{
    LIGHT,
    MEDIUM,
    HEAVY,
    NONE
};

public class WeatherManager : Singelton<WeatherManager>
{
    private WEATHER_STATE currentWeatherState;

    [Header("Wind")]
    public bool ShowWindPoints = true;
    public Vector3[] WindPoints;

    [Header("Audio")]
    public AudioClip Rain_light;
    public AudioClip Rain_medium;
    public AudioClip Rain_heavy;
    public AudioClip Wind;
    public AudioSource RainSource;
    public AudioSource WindSource;

    private ParticleSystem rainDropsEffect;
    private ParticleSystem rainRipplesEffect;
    private ParticleSystem.EmissionModule dropsModule;
    private ParticleSystem.EmissionModule ripplesModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityOverLifeTimeModule;

    private Vector3 windDirection;
    private float windSpeed;

    private void Awake() 
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

    private void Start()
    {
        StartCoroutine(IChangeWind());
    }

    private void OnDrawGizmos()
    {
        if (ShowWindPoints)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < WindPoints.Length; i++)
            {
                var waypoint = WindPoints[i];
                Gizmos.DrawLine(waypoint + Vector3.forward * 0.2f, waypoint + Vector3.back * 0.2f);
                Gizmos.DrawLine(waypoint + Vector3.right * 0.2f, waypoint + Vector3.left * 0.2f);
            }
        }
    }

    public void ChangeWeatherState(WEATHER_STATE newWeatherState)
    {
        currentWeatherState = newWeatherState;

        switch (currentWeatherState)
        {
            case WEATHER_STATE.LIGHT:

                ModifyRain(40, 60, 20, 40);

                ModifyWind(Vector3.forward, 0f);

                RainSource.clip = Rain_light;

                StartRain();

                break;

            case WEATHER_STATE.MEDIUM:

                ModifyRain(60, 80, 40, 60);

                ModifyWind(Vector3.forward, 0f);

                RainSource.clip = Rain_medium;

                StartRain();

                break;

            case WEATHER_STATE.HEAVY:

                ModifyRain(100, 120 ,60, 80);

                ModifyWind(Vector3.forward, 0f);

                RainSource.clip = Rain_heavy;

                StartRain();

                break;

            case WEATHER_STATE.NONE:

                StopRain();

                break;

            default:

                break;
        }    
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
        windDirection = newDirection;
        windSpeed = newSpeed;

        var newOrbitalValue = velocityOverLifeTimeModule.orbitalOffsetX;
        newOrbitalValue.constant = 10f;
        velocityOverLifeTimeModule.orbitalOffsetX = newOrbitalValue;
    }

    private void ChangeWindSourcePosition(Vector3 position)
    {
        WindSource.transform.position = position;
    }

    private IEnumerator IChangeWind()
    {
        while (currentWeatherState.Equals(WEATHER_STATE.NONE) == false)
        {
            var nextWind = Random.Range(2f, 6f);

            yield return new WaitForSeconds(nextWind);

            var randomWindPosition = WindPoints[Random.Range(0, WindPoints.Length)];

            ChangeWindSourcePosition(randomWindPosition);

            var windDuration = Random.Range(2f, 3f);

            // FIX ME!!

            //ModifyWind()

            yield return new WaitForSeconds(windDuration);
        }
    }
}
