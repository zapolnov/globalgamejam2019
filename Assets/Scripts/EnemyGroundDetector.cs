
using UnityEngine;

public sealed class EnemyGroundDetector : MonoBehaviour
{
    public bool IsOnGround => mIsOnGround != 0;
    private int mIsOnGround;

    //void OnCollisionEnter2D(Collision collision)
    void OnTriggerEnter2D(Collider2D collider)
    {
        ++mIsOnGround;
    }

    //void OnCollisionExit2D(Collision collision)
    void OnTriggerExit2D(Collider2D collider)
    {
        --mIsOnGround;
    }
}
