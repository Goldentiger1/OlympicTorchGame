using UnityEngine;

public class WeatherManager : Singelton<WeatherManager>
{
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

    public void StartRain() 
    {
        if(rainDropsEffect.isPlaying == false) 
        {
            rainDropsEffect.Play();
        }

        if (rainRipplesEffect.isPlaying == false)
        {
            rainRipplesEffect.Play();
        }
    }

    public void EndRain() 
    {
        if (rainDropsEffect.isPlaying == true)
        {
            rainDropsEffect.Stop();
        }

        if (rainRipplesEffect.isPlaying == true) 
        {
            rainRipplesEffect.Stop();
        }
    }

    public void Wind(float speed, float duration) 
    {
        var foo = rainDropsEffect.velocityOverLifetime;
        foo.x = speed;
    }
}
