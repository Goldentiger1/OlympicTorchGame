using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
    #region VARIABLES

    [Header("Characters")]
    public GameObject PlayerPrefab;
    public GameObject BubiPrefab;

    [Header("Effects")]
    public GameObject RainEffectPrefab;
    public GameObject BigFireEffectPrefab;
    public GameObject FireEffectPrefab;

    [Header("Others")]
    public GameObject TorchPrefab;

    #endregion VARIABLES
}
