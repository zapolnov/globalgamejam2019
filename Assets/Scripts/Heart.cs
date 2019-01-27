
using UnityEngine;

public sealed class Heart : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null && player.Lives < player.MaxLives) {
            ++player.Lives;
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null && player.Lives < player.MaxLives) {
            ++player.Lives;
            Destroy(gameObject);
        }
    }
}
