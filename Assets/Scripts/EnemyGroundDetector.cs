
using UnityEngine;

public sealed class EnemyGroundDetector : MonoBehaviour
{
    public bool IsOnGround => mIsOnGround != 0;
    private int mIsOnGround;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
            return;
        ++mIsOnGround;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
            return;
        --mIsOnGround;
    }
}
