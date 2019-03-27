using System.Collections;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [Header("Variables")]
    public float FlameStrenght;
    public float BaseBurnRate;

    private float currentBurnRate;

    private Transform flamingPart;

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
        StartCoroutine(ILifeTime());
    }

    public void ChangeBurningRate(float newBurnRate)
    {
        currentBurnRate = newBurnRate;
    }

    private void OnTriggerEnter(Collider other) 
    {
        // 11 == FirePoint layer index
        if (other.gameObject.layer == 11) 
        {
            if (GameManager.Instance.OlympicFlameStarted)
                return;

            if (GameManager.Instance.TimeToStartFire)
            {
                GameManager.Instance.OlympicFlameStarted = true;

                var bigFirePrefab = ResourceManager.Instance.BigFireEffect;

                var bigFire = Instantiate(bigFirePrefab, other.bounds.center, Quaternion.identity);
                bigFire.name = bigFirePrefab.name;
            }
           
            GameManager.Instance.ChangeGameState(GAME_STATE.END);
           
        }
    }

    private IEnumerator ILifeTime()
    {
        ChangeBurningRate(BaseBurnRate);

        while (IsBurning)
        {
            FlameStrenght -= currentBurnRate;
            yield return new WaitForSeconds(currentBurnRate);
            UIManager.Instance.UpdateTorchStrenght(FlameStrenght);
        }

        flamingPart.gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GAME_STATE.END);
    }
}
