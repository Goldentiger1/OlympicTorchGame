using UnityEngine;

public class OlympicCauldron : MonoBehaviour
{
    [Header("References")]
    public GameObject Arrow;

    private Transform target;
  
    private void Awake()
    {
        Arrow.SetActive(false);
    }

    private void Start()
    {
        target = PlayerEngine.Instance.hmdTransform;
    }

    public void ShowHint()
    {
        Arrow.SetActive(true);
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {

            Arrow.transform.LookAt(
           new Vector3(
           target.position.x,
           Arrow.transform.position.y,
           target.position.z)
           );
        }
    }

    public void HideHint()
    {
        Arrow.SetActive(false);
    }
}
