
using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
    public float Speed = 1.0f;
    public Player Player;

    public void LateUpdate()
    {
        Vector3 position = transform.position;

        float step = Player.transform.position.y - position.y;
        float maxStep = Time.deltaTime * Speed;
        if (Mathf.Abs(step) > Mathf.Abs(maxStep))
            step = maxStep * Mathf.Sign(step);

        position.y += step;
        transform.position = position;
    }
}
