
using UnityEngine;

public sealed class Enemy : MonoBehaviour
{
    public EnemyGroundDetector LeftGroundDetector;
    public EnemyGroundDetector RightGroundDetector;

    public float Speed = 0.1f;
    public float WaitTime = 1.0f;
    public bool MovesRight;

    private float mWaiting;

    void Update()
    {
        if (mWaiting > 0.0f) {
            mWaiting -= Time.deltaTime;
            if (mWaiting > 0.0f)
                return;
        }

        if (LeftGroundDetector.IsOnGround || RightGroundDetector.IsOnGround) {
            if (!LeftGroundDetector.IsOnGround && !MovesRight) {
                MovesRight = true;
                mWaiting = WaitTime;
            } else if (!RightGroundDetector.IsOnGround && MovesRight) {
                MovesRight = false;
                mWaiting = WaitTime;
            } else {
                Vector3 pos = transform.position;
                pos.x += (MovesRight ? Speed : -Speed) * Time.deltaTime * 60.0f;
                transform.position = pos;
            }
        }
    }
}
