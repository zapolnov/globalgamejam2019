
using UnityEngine;

public sealed class EnemyContactDetector : MonoBehaviour
{
    public bool CollidesWithEnemy => mCollidesWithEnemy != 0;
    private int mCollidesWithEnemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
            ++mCollidesWithEnemy;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
            --mCollidesWithEnemy;
    }
}
