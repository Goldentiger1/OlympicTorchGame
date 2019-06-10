using System.Collections;
using UnityEngine;

public class BackCollider : MonoBehaviour
{
    [Header("HUD Panel Variables")]
    public Vector3 Offset;
    public float SmoothMultiplier;

    private Coroutine iUpdateMotion;

    private void Start()
    {
        StartUpdateMotion();
    }

    private void StartUpdateMotion()
    {
        if (iUpdateMotion == null)
        {
            iUpdateMotion = StartCoroutine(IUpdateMotion());
        }
    }

    private void Move(Transform target, Transform element, Vector3 offset, float smoothMultiplier = 1f)
    {
        var desiredPosition = new Vector3(
            target.position.x,
            element.position.y,
            target.position.z
            );

        element.position = Vector3.Lerp(
            element.position,
            desiredPosition + (target.forward * offset.z),
            Time.deltaTime * SmoothMultiplier);

        element.position = new Vector3(
            element.position.x,
            offset.y,
            element.position.z
            );
    }

    private void Rotate(Transform target, Transform element)
    {
        element.LookAt(
            new Vector3(
            target.position.x,
            element.position.y,
            target.position.z)
            );
    }

    private IEnumerator IUpdateMotion()
    {
        var target = PlayerEngine.Instance.hmdTransform;

        while (true)
        {
            Move(target, transform, Offset, SmoothMultiplier);
            Rotate(target, transform);

            yield return null;
        }
    }
}
