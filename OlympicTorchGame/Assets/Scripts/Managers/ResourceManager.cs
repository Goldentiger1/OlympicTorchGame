﻿using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
    [Header("Characters")]
    public GameObject PlayerPrefab;
    public GameObject BubiPrefab;

    [Header("Effects")]
    public GameObject RainEffectPrefab;

    [Header("Others")]
    public GameObject TorchPrefab;
}
