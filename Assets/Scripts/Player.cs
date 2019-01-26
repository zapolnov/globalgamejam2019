
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player : MonoBehaviour
{
    public float MinJump = 10.0f;
    public float MaxJump = 50.0f;
    public float JumpScale = 10.0f;
    public float RecoverTime = 3.0f;
    public float DeathTime = 1.0f;
    public int Lives = 3;

    public GameObject Visual;
    public GroundDetector GroundDetector;
    public EnemyContactDetector EnemyContactDetector;
    public GameObject BarSpawnPoint;
    public GameObject BarPrefab;

    public Vector2 LastVelocity { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    private Vector3 mStartPosition;
    private GameObject mCurrentBar;
    private float mRecoveringTimer;
    private bool mDragging;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (EnemyContactDetector.CollidesWithEnemy && mRecoveringTimer <= 0.0f && Lives > 0)
        {
            --Lives;
            if (Lives > 0)
                mRecoveringTimer = RecoverTime;
            else {
                Time.timeScale = 0.0f;
                mRecoveringTimer = DeathTime;
            }
        }

        if (mRecoveringTimer <= 0.0f) {
            Visual.SetActive(true);
            if (Lives <= 0) {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("GameOver");
                return;
            }
        } else {
            mRecoveringTimer -= Time.unscaledDeltaTime;
            Visual.SetActive(mRecoveringTimer % 0.5f < 0.25f);
        }

        if (Lives <= 0)
            return;

        if (Input.GetMouseButtonDown(0)) {
            mStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GroundDetector.IsOnGround)
                mDragging = true;
            else {
                if (mCurrentBar != null)
                    Destroy(mCurrentBar);
                mCurrentBar = Instantiate(BarPrefab, BarSpawnPoint.transform.position, Quaternion.identity);
                Rigidbody.velocity = new Vector2(0.0f, 0.0f);
            }
        } else if (Input.GetMouseButtonUp(0) && mDragging) {
            mDragging = false;
            if (!GroundDetector.IsOnGround)
                return;

            if (mCurrentBar != null)
                Destroy(mCurrentBar);

            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mStartPosition;
            dir *= JumpScale;
            float length = dir.magnitude;
            if (length == 0.0f)
                return;

            if (length < MinJump) {
                dir /= length;
                dir *= MinJump;
            } else if (length > MaxJump) {
                dir /= length;
                dir *= MaxJump;
            }

            Rigidbody.AddForce(dir);
        }
    }

    void FixedUpdate()
    {
        LastVelocity = Rigidbody.velocity;
    }
}
