using TMPro;
using UnityEngine;

public class UIManager : Singelton<UIManager>
{
    [Header("Texts")]
    public TextMeshProUGUI GameTimeText;

    public void UpdateGameTime(float newTime)
    {
        GameTimeText.text = newTime.ToString();
    }
}
