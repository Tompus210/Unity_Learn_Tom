using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform target;           // der Player

    [Header("Offset & Bewegung")]
    public Vector3 offset = new Vector3(0f, 2f, -4f);
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        // gewünschte Position berechnen
        Vector3 desiredPosition = target.position + offset;

        // sanft hinterhergleiten
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // auf den Player schauen
        transform.LookAt(target);
    }
}

