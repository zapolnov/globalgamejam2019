
using UnityEngine;

public sealed class Enemy : MonoBehaviour
{
    public EnemyGroundDetector LeftGroundDetector;
    public EnemyGroundDetector RightGroundDetector;
    public GameObject Trigger;
    public GameObject KillPoint;

    public float Speed = 0.1f;
    public float WaitTime = 1.0f;
    public float DeathAnimationTime = 2.0f;
    public bool MovesRight;

    public GameObject Walk1;
    public GameObject Walk2;
    public GameObject Death1;
    public GameObject Death2;

    public AudioClip DeathSound;

    private float mWaiting;
    private float mDeathAnimationTimer;
    private bool mDead;

    public void Kill()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        collider.tag = "DeadEnemy";
        collider.enabled = true;
        Destroy(LeftGroundDetector.gameObject);
        Destroy(RightGroundDetector.gameObject);
        Destroy(Trigger);
        mDead = true;
        mDeathAnimationTimer = DeathAnimationTime;
        Walk1.SetActive(false);
        Walk2.SetActive(false);
        Death1.SetActive(true);
        Death2.SetActive(false);
        SoundManager.Instance.PlaySound(DeathSound);
    }

    void AdjustScale(Transform t, float mult = 1.0f)
    {
        var scale = t.localScale;
        scale.x = (MovesRight ? 1.0f : -1.0f) * mult;
        t.localScale = scale;
    }

    void Update()
    {
        if (mDead) {
            if (mDeathAnimationTimer > 0.0f) {
                Death1.SetActive(true);
                Death2.SetActive(false);
                mDeathAnimationTimer -= TimeManager.deltaTime;
                if (mDeathAnimationTimer > 0.0f)
                    return;
            }

            Death1.SetActive(false);
            Death2.SetActive(true);
        } else {
            if (mWaiting > 0.0f) {
                mWaiting -= TimeManager.deltaTime;
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
                    pos.x += (MovesRight ? Speed : -Speed) * TimeManager.deltaTime * 60.0f;
                    transform.position = pos;
                }
            }

            Death1.SetActive(false);
            Death2.SetActive(false);
        }

        Walk1.SetActive(!mDead && TimeManager.timeSinceLevelLoad % 1.0f < 0.5f);
        Walk2.SetActive(!mDead && TimeManager.timeSinceLevelLoad % 1.0f >= 0.5f);

        AdjustScale(Walk1.transform);
        AdjustScale(Walk2.transform);
        AdjustScale(Death1.transform);
        AdjustScale(Death2.transform);
    }
}
