
using UnityEngine;

public class BouncingWall : MonoBehaviour
{
    public float BounceDirection;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];

        var player = collision.otherRigidbody.GetComponent<Player>();
        if (player == null) {
            player = collision.rigidbody.GetComponent<Player>();
            if (player == null)
                return;
        }

        var newVelocity = new Vector2(-player.LastVelocity.x, player.LastVelocity.y);
        if (newVelocity.y < 3.5f)
            newVelocity.y = 3.5f;
        player.Rigidbody.velocity = newVelocity;
    }
}
