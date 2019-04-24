using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
    #region VARIABLES

    [Header("Characters")]
    public GameObject PlayerPrefab;
    public GameObject BubiPrefab;

    [Header("Effects")]
    public GameObject RainEffectPrefab;
    public GameObject BigFireEffect;

    [Header("Others")]
    public GameObject TorchPrefab;

    #endregion VARIABLES
}
