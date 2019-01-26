
using UnityEngine;

public sealed class EnemyContactDetector : MonoBehaviour
{
    public bool CollidesWithEnemy => mCollidesWithEnemy != 0;
    public Vector2 LastContactDirection { get; private set; }
    private int mCollidesWithEnemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy") {
            ++mCollidesWithEnemy;
            LastContactDirection = (transform.position - collider.transform.position).normalized;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
            --mCollidesWithEnemy;
    }
}
