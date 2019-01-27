
using UnityEngine;

public sealed class Coin : MonoBehaviour
{
    public AudioClip Sound; 

    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null) {
            ++player.CoinsCollected;
            SoundManager.Instance.PlaySound(Sound);
            Destroy(gameObject);
        }
    }
}
