
using UnityEngine;

public sealed class Dragon : MonoBehaviour
{
    enum State
    {
        Init,
        Appear,
        Shoot,
        Wait,
        Disappear,
        WaitHidden,
    }

    public bool LaserShoot;
    public bool MovesRight;
    public int ShootCount = 3;
    public float AppearTime = 3.0f;
    public float ShootTime = 1.0f;
    public float WaitTime = 1.0f;
    public float DisappearTime = 3.0f;
    public float WaitHiddenTime = 3.0f;
    public SpriteRenderer Normal;
    public SpriteRenderer Open;
    public Fireball FireballPrefab;
    public Transform FireballSpawn;
    public GameObject Laser;

    private Vector3 mInitialPosition;

    public AudioClip ShotSound;
    public AudioClip LaserSound; 

    private State mState = State.Init;
    private float mTimeLeft;
    private int mShotsLeft;

    void Awake()
    {
        mInitialPosition = transform.position;
        Normal.gameObject.SetActive(false);
        Open.gameObject.SetActive(false);
    }

    void Shoot()
    {
        mState = State.Shoot;
        mTimeLeft = ShootTime;

        var viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool visibleOnScreen = viewportPoint.y > -0.3f && viewportPoint.y < 1.3f;

        if (LaserShoot) {
            Laser.SetActive(true);
            if (visibleOnScreen)
                SoundManager.Instance.PlaySound(LaserSound);
        } else {
            var fireball = Instantiate(FireballPrefab.gameObject, FireballSpawn.position, Quaternion.identity);
            fireball.GetComponent<Fireball>().MovesRight = MovesRight;
            if (visibleOnScreen)
                SoundManager.Instance.PlaySound(ShotSound);
        }

        --mShotsLeft;
    }

    void Update()
    {
        if (mTimeLeft > 0.0f) {
            mTimeLeft -= TimeManager.deltaTime;
            if (mTimeLeft < 0.0f)
                mTimeLeft = 0.0f;
        }

        switch (mState) {
            case State.Init:
                mState = State.Appear;
                mTimeLeft = AppearTime;
                break;

            case State.Appear: {
                Vector3 delta = new Vector3((MovesRight ? -1.0f : 1.0f) * (mTimeLeft / AppearTime), 0.0f, 0.0f);
                transform.position = mInitialPosition + delta;
                if (mTimeLeft <= 0.0f) {
                    mShotsLeft = ShootCount;
                    Shoot();
                }
                break;
            }

            case State.Shoot: {
                if (mTimeLeft <= 0.0f) {
                    mState = State.Wait;
                    mTimeLeft = WaitTime;
                    Laser.SetActive(false);
                }
                break;
            }

            case State.Wait: {
                if (mTimeLeft <= 0.0f) {
                    if (mShotsLeft > 0)
                        Shoot();
                    else {
                        mState = State.Disappear;
                        mTimeLeft = DisappearTime;
                    }
                }
                break;
            }

            case State.Disappear: {
                Vector3 delta = new Vector3((MovesRight ? -1.0f : 1.0f) * (1.0f - mTimeLeft / DisappearTime), 0.0f, 0.0f);
                transform.position = mInitialPosition + delta;
                if (mTimeLeft <= 0.0f) {
                    mState = State.WaitHidden;
                    mTimeLeft = WaitHiddenTime;
                }
                break;
            }

            case State.WaitHidden: {
                if (mTimeLeft <= 0.0f) {
                    mState = State.Appear;
                    mTimeLeft = AppearTime;
                }
                break;
            }
        }

        Normal.gameObject.SetActive(mState != State.Shoot && mState != State.WaitHidden);
        Open.gameObject.SetActive(mState == State.Shoot);
    }
}
