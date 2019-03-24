using TMPro;
using UnityEngine;

public class UIManager : Singelton<UIManager>
{
    [Header("Texts")]
    public TextMeshProUGUI GameTimeText;
    public TextMeshProUGUI TorchStrenghtText;

    public void UpdateGameTime(float currentTime)
    {
        GameTimeText.text = "TIME: " + currentTime.ToString();
    }

    public void UpdateTorchStrenght(float currentStrenght)
    {
        TorchStrenghtText.text = "TORCH STRENGHT: " + currentStrenght.ToString();
    }
}
