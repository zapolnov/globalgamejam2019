
using UnityEngine;

public sealed class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null) {
            ++player.CoinsCollected;
            Destroy(gameObject);
        }
    }
}
