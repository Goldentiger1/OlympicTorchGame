using UnityEngine;

public class HandCoverZone : MonoBehaviour
{
    private bool isCoverState;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void Start()
    {
        
    }

    public void OnHit()
    {
        if (isCoverState)
            return;

        print(transform.name);

        isCoverState = true;
        meshRenderer.enabled = true;
    }
}
