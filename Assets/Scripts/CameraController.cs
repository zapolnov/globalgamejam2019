
using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
    public float Speed = 1.0f;
    public GameObject Target;
    public GameObject Floor;

    public void LateUpdate()
    {
        Vector3 position = transform.position;

        float step = Target.transform.position.y - position.y;
        float maxStep = Time.unscaledDeltaTime * Speed;
        if (Mathf.Abs(step) > Mathf.Abs(maxStep))
            step = maxStep * Mathf.Sign(step);

        position.y += step;
        transform.position = position;

        var cam = Camera.main;
        Vector3 bottom = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, cam.nearClipPlane));
        Vector3 center = cam.ScreenToWorldPoint(new Vector3(0.0f, cam.pixelHeight * 0.5f, cam.nearClipPlane));
        if (bottom.y < Floor.transform.position.y) {
            position.y = Floor.transform.position.y + Mathf.Abs(center.y - bottom.y);
            transform.position = position;
        }
    }
}
