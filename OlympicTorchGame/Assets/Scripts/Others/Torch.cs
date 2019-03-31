using System.Collections;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [Header("Variables")]
    public float FlameStrenght;

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

    public void ModifyFlameStrenght(float value)
    {
        FlameStrenght += value;
    }

    private void OnTriggerEnter(Collider other) 
    {     
        var foo = other.gameObject.layer;

        switch (foo)
        {
            // FirePoint layer index
            case 11:

                if (GameManager.Instance.OlympicFlameStarted)
                    return;

                if (GameManager.Instance.TimeToStartFire) 
                {
                    GameManager.Instance.OlympicFlameStarted = true;

                    var bigFirePrefab = ResourceManager.Instance.BigFireEffect;

                    var bigFire = Instantiate(bigFirePrefab, other.bounds.center, Quaternion.identity);
                    bigFire.name = bigFirePrefab.name;
                }

                break;

            default:

                break;
        }
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
    }
}
