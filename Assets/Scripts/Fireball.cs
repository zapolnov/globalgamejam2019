
using UnityEngine;

public sealed class Fireball : MonoBehaviour
{
    public bool MovesRight;
    public float Speed = 60.0f;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += (MovesRight ? 1.0f : -1.0f) * Speed * TimeManager.deltaTime;
        transform.position = pos;

        if (pos.x < -10.0f || pos.x > 10.0f) {
            Destroy(gameObject);
            return;
        }

        Vector3 scale = transform.localScale;
        scale.x = (MovesRight ? -1.0f : 1.0f);
        transform.localScale = scale;
    }
}
