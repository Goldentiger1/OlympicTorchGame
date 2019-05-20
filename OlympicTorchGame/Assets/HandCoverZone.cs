using UnityEngine;

public class HandCoverZone : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();    
    }

    private void Start()
    {
        meshRenderer.enabled = false;
    }

    public void OnHit()
    {
        meshRenderer.enabled = true;
    }
}
