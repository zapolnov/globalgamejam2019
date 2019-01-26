
using UnityEngine;

public sealed class Enemy : MonoBehaviour
{
    public EnemyGroundDetector LeftGroundDetector;
    public EnemyGroundDetector RightGroundDetector;

    public float Speed = 0.1f;
    public float WaitTime = 1.0f;
    public bool MovesRight;

    public GameObject Walk1;
    public GameObject Walk2;
    public GameObject Death1;

    private float mWaiting;
    private bool mDead;

    void AdjustScale(Transform t, float mult = 1.0f)
    {
        var scale = t.localScale;
        scale.x = (MovesRight ? 1.0f : -1.0f) * mult;
        t.localScale = scale;
    }

    void Update()
    {
        if (mWaiting > 0.0f) {
            mWaiting -= Time.deltaTime;
            if (mWaiting > 0.0f)
                return;
        }

        if (!mDead && (LeftGroundDetector.IsOnGround || RightGroundDetector.IsOnGround)) {
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

        Walk1.SetActive(!mDead && Time.timeSinceLevelLoad % 1.0f < 0.5f);
        Walk2.SetActive(!mDead && Time.timeSinceLevelLoad % 1.0f >= 0.5f);
        Death1.SetActive(mDead);

        AdjustScale(Walk1.transform);
        AdjustScale(Walk2.transform);
        AdjustScale(Death1.transform);
    }
}
