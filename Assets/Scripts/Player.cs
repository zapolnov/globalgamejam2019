
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player : MonoBehaviour
{
    enum VisualState
    {
        Idle1,
        Idle2,
        Jump1,
        Jump2,
        Landing,
        BeingHit,
        Draw1,
        Draw2,
        Attack1,
        Attack2,
        Death1,
        Death2,
    }

    public float MinJump = 10.0f;
    public float MaxJump = 50.0f;
    public float JumpScale = 10.0f;
    public float RecoverTime = 3.0f;
    public float DeathTime1 = 0.1f;
    public float DeathTime2 = 3.0f;
    public float DrawTime1 = 0.1f;
    public float DrawTime2 = 0.1f;
    public float AttackTime1 = 0.1f;
    public float AttackTime2 = 0.1f;
    public float IdleSwitchTime = 0.5f;
    public float JumpAnticipationTime = 0.2f;
    public float DeathAnticipationTime = 0.3f;
    public float BeingHitTime = 0.5f;
    public float LandingTime = 0.15f;
    public int Lives = 3;

    public GameObject Visual;
    public GroundDetector GroundDetector;
    public EnemyContactDetector EnemyContactDetector;
    public EnemyBeneathDetector EnemyBeneathDetector;
    public GameObject BarSpawnPoint;
    public GameObject BarPrefab;

    private VisualState mVisualState = VisualState.Idle1;
    public GameObject Idle1;
    public GameObject Idle2;
    public GameObject Jump1;
    public GameObject Jump2;
    public GameObject Draw1;
    public GameObject Draw2;
    public GameObject Attack1;
    public GameObject Attack2;
    public GameObject Death1;
    public GameObject Death2;
    public GameObject BeingHit;
    public GameObject Landing;

    public Vector2 LastVelocity { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    private Vector3 mStartPosition;
    private GameObject mCurrentBar;
    private float mRecoveringTimer;
    private float mIdleTimer;
    private float mJumpAnticipationTimer;
    private float mDrawTimer;
    private float mDeathTimer;
    private float mBeingHitTimer;
    private float mLandingTimer;
    private float mAttackTimer;
    private Vector2 mJumpDirection;
    private float mLookDirection = 1.0f;
    private bool mDragging;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        LastVelocity = Rigidbody.velocity;
    }

    void UpdateLives()
    {
        if (mVisualState == VisualState.Jump1
            || mVisualState == VisualState.Draw1
            || mVisualState == VisualState.Draw2
            || mVisualState == VisualState.Attack1
            || mVisualState == VisualState.Attack2
            || mVisualState == VisualState.BeingHit
            || mVisualState == VisualState.Death1
            || mVisualState == VisualState.Death2)
            return;

        if (EnemyContactDetector.CollidesWithEnemy && mRecoveringTimer <= 0.0f && Lives > 0)
        {
            Vector2 dir;
            if (EnemyContactDetector.LastContactDirection.x < 0.0f)
                dir.x = -100.0f;
            else if (EnemyContactDetector.LastContactDirection.x > 0.0f)
                dir.x = 100.0f;
            else
                dir.x = (mLookDirection > 0.0f ? 100.0f : -100.0f);
            dir.y = 200.0f; 
            Rigidbody.AddForce(dir);

            if (mCurrentBar != null)
                Destroy(mCurrentBar);

            --Lives;
            if (Lives > 0) {
                mRecoveringTimer = RecoverTime;
                mVisualState = VisualState.BeingHit;
                mBeingHitTimer = BeingHitTime;
            } else {
                mVisualState = VisualState.Death1;
                mDeathTimer = DeathTime1;
            }
        }

        if (mRecoveringTimer <= 0.0f)
            Visual.SetActive(true);
        else {
            mRecoveringTimer -= Time.unscaledDeltaTime;
            Visual.SetActive(mRecoveringTimer % 0.5f >= 0.25f);
        }
    }

    void UpdateInputs()
    {
        if (mVisualState == VisualState.Jump1
            || mVisualState == VisualState.Draw1
            || mVisualState == VisualState.Draw2
            || mVisualState == VisualState.Attack1
            || mVisualState == VisualState.Attack2
            || mVisualState == VisualState.BeingHit
            || mVisualState == VisualState.Death1
            || mVisualState == VisualState.Death2)
            return;

        if (Input.GetMouseButtonDown(0)) {
            mStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GroundDetector.IsOnGround)
                mDragging = true;
            else if (EnemyBeneathDetector.HasEnemyBeneath) {
                Vector2 delta = transform.position - BarSpawnPoint.transform.position;
                transform.position = EnemyBeneathDetector.GetKillPoint() + delta;
                EnemyBeneathDetector.KillCollidingEnemies();
                mAttackTimer = AttackTime1;
                mVisualState = VisualState.Attack1;
            } else {
                if (mCurrentBar != null)
                    Destroy(mCurrentBar);
                mDrawTimer = DrawTime1;
                mVisualState = VisualState.Draw1;
            }
        } else if (Input.GetMouseButtonUp(0) && mDragging) {
            mDragging = false;
            if (!GroundDetector.IsOnGround)
                return;

            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mStartPosition;
            dir *= JumpScale;
            float length = dir.magnitude;
            if (length == 0.0f) {
                if (mCurrentBar != null)
                    Destroy(mCurrentBar);
                return;
            }

            if (length < MinJump) {
                dir /= length;
                dir *= MinJump;
            } else if (length > MaxJump) {
                dir /= length;
                dir *= MaxJump;
            }

            mVisualState = VisualState.Jump1;
            mJumpAnticipationTimer = JumpAnticipationTime;
            mJumpDirection = dir; 
        }
    }

    void Update()
    {
        UpdateLives();

        if (Lives > 0)
            UpdateInputs();

        UpdateVisual();
    }

    void AdjustScale(Transform t, float mult = 1.0f)
    {
        var scale = t.localScale;
        scale.x = mLookDirection * mult;
        t.localScale = scale;
    }

    void UpdateVisual()
    {
        if (LastVelocity.x < 0.0f)
            mLookDirection = 1.0f;
        else if (LastVelocity.x > 0.0f)
            mLookDirection = -1.0f;

        switch (mVisualState) {
            case VisualState.Idle1:
            case VisualState.Idle2:
                 mIdleTimer -= Time.deltaTime;
                 if (mIdleTimer <= 0.0f) {
                    mIdleTimer += IdleSwitchTime;
                    mVisualState = mVisualState == VisualState.Idle1 ? VisualState.Idle2 : VisualState.Idle1;
                 }
                 if (!GroundDetector.IsOnGround)
                    mVisualState = VisualState.Jump2;
                 break;

            case VisualState.Jump1:
                mJumpAnticipationTimer -= Time.unscaledDeltaTime;
                if (mJumpAnticipationTimer <= 0.0f) {
                    mVisualState = VisualState.Jump2;
                    Rigidbody.AddForce(mJumpDirection);
                    if (mCurrentBar != null)
                        Destroy(mCurrentBar);
                }
                break;

            case VisualState.Jump2:
                if (GroundDetector.IsOnGround) {
                    mVisualState = VisualState.Landing;
                    mLandingTimer = LandingTime;
                }
                break;

            case VisualState.Draw1:
                Rigidbody.velocity = new Vector2(0.0f, 0.0f);
                Visual.SetActive(true);
                mDrawTimer -= Time.unscaledDeltaTime;
                if (mDrawTimer <= 0.0f) {
                    mVisualState = VisualState.Draw2;
                    mDrawTimer = DrawTime2;
                    mCurrentBar = Instantiate(BarPrefab, BarSpawnPoint.transform.position, Quaternion.identity);
                }
                break;

            case VisualState.Draw2:
                Rigidbody.velocity = new Vector2(0.0f, 0.0f);
                Visual.SetActive(true);
                mDrawTimer -= Time.unscaledDeltaTime;
                if (mDrawTimer <= 0.0f)
                    mVisualState = VisualState.Idle1;
                break;

            case VisualState.Attack1:
                Rigidbody.velocity = new Vector2(0.0f, 0.0f);
                Visual.SetActive(true);
                mAttackTimer -= Time.unscaledDeltaTime;
                if (mAttackTimer <= 0.0f) {
                    mVisualState = VisualState.Attack2;
                    mAttackTimer = AttackTime2;
                }
                break;

            case VisualState.Attack2:
                Rigidbody.velocity = new Vector2(0.0f, 0.0f);
                Visual.SetActive(true);
                mAttackTimer -= Time.unscaledDeltaTime;
                if (mAttackTimer <= 0.0f)
                    mVisualState = VisualState.Jump2;
                break;

            case VisualState.BeingHit:
                mBeingHitTimer -= Time.unscaledDeltaTime;
                if (mBeingHitTimer <= 0.0f)
                    mVisualState = VisualState.Idle1;
                break;

            case VisualState.Landing:
                mLandingTimer -= Time.unscaledDeltaTime;
                if (mLandingTimer <= 0.0f)
                    mVisualState = VisualState.Idle1;
                break;

            case VisualState.Death1:
                mDeathTimer -= Time.unscaledDeltaTime;
                if (mDeathTimer <= 0.0f) {
                    mDeathTimer = DeathTime2;
                    mVisualState = VisualState.Death2;
                }
                break;

            case VisualState.Death2:
                mDeathTimer -= Time.unscaledDeltaTime;
                if (mDeathTimer <= 0.0f)
                    SceneManager.LoadScene("GameOver");
                break;
        }

        Idle1.SetActive(mVisualState == VisualState.Idle1);
        Idle2.SetActive(mVisualState == VisualState.Idle2);
        Jump1.SetActive(mVisualState == VisualState.Jump1);
        Jump2.SetActive(mVisualState == VisualState.Jump2);
        Draw1.SetActive(mVisualState == VisualState.Draw1);
        Draw2.SetActive(mVisualState == VisualState.Draw2);
        Death1.SetActive(mVisualState == VisualState.Death1);
        Death2.SetActive(mVisualState == VisualState.Death2);
        BeingHit.SetActive(mVisualState == VisualState.BeingHit);
        Landing.SetActive(mVisualState == VisualState.Landing);
        Attack1.SetActive(mVisualState == VisualState.Attack1);
        Attack2.SetActive(mVisualState == VisualState.Attack2);

        AdjustScale(Idle1.transform);
        AdjustScale(Idle2.transform);
        AdjustScale(Jump1.transform);
        AdjustScale(Jump2.transform);
        AdjustScale(Death1.transform, -1.0f);
        AdjustScale(Death2.transform);
        AdjustScale(BeingHit.transform, -1.0f);
        AdjustScale(Landing.transform);
    }
}
