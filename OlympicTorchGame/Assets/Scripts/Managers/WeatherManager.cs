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

    [Header("Audio")]
    public AudioClip Rain_light;
    public AudioClip Rain_medium;
    public AudioClip Rain_heavy;
    public AudioClip Wind;
    public AudioSource RainSource;
    public AudioSource WindSource;

    private ParticleSystem rainDropsEffect;
    private ParticleSystem rainRipplesEffect;

    private void Awake() 
    {
        var rainEffectPrefab = ResourceManager.Instance.RainEffectPrefab;
        var rainEffect = Instantiate(rainEffectPrefab, transform).transform;
        rainEffect.name = rainEffectPrefab.name;

        rainDropsEffect = rainEffect.Find("RainDrops").GetComponent<ParticleSystem>();
        rainRipplesEffect = rainEffect.Find("RainRipples").GetComponent<ParticleSystem>();
    }

    public void ChangeWeatherState(WEATHER_STATE newWeatherState)
    {
        currentWeatherState = newWeatherState;

        switch (currentWeatherState)
        {
            case WEATHER_STATE.LIGHT:

                break;

            case WEATHER_STATE.MEDIUM:

                break;

            case WEATHER_STATE.HEAVY:

                break;

            case WEATHER_STATE.NONE:

                if (rainDropsEffect.isPlaying == true)
                {
                    rainDropsEffect.Stop();
                }

                if (rainRipplesEffect.isPlaying == true)
                {
                    rainRipplesEffect.Stop();
                }

                break;

            default:

                break;
        }

        if (rainDropsEffect.isPlaying == false)
        {
            rainDropsEffect.Play();
        }

        if (rainRipplesEffect.isPlaying == false)
        {
            rainRipplesEffect.Play();
        }
    }

    private void ModifyRain(int minDropParticles, int maxDropParticles, int minRippleParticles, int maxRippleParticles)
    {

    }

    private void ModifyWind(float speed) 
    {
       
    }

    private IEnumerator IChangeWind()
    {
        while (currentWeatherState.Equals(WEATHER_STATE.NONE))
        {
            var newWind = Random.Range(2f, 6f);

            yield return new WaitForSeconds(newWind);

            ModifyWind(20f);
        }
    }
}
