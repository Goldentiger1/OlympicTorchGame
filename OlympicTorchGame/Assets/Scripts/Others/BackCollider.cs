using UnityEngine;

public class BackCollider : MonoBehaviour
{
    private void Update()
    {
        MovePosition(PlayerEngine.Instance.headCollider.transform.position);
    }

    private void MovePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.z);

     
    }
}
