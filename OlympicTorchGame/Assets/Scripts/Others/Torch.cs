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
