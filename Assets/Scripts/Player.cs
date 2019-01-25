
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    public float MinJump = 10.0f;
    public float MaxJump = 50.0f;
    public float JumpScale = 10.0f;

    public GroundDetector GroundDetector;

    public Vector2 LastVelocity { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    private Vector3 mStartPosition;
    private bool mDragging;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            mStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GroundDetector.IsOnGround)
                mDragging = true;
        } else if (Input.GetMouseButtonUp(0) && mDragging) {
            mDragging = false;
            if (!GroundDetector.IsOnGround)
                return;

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

            Debug.Log($"{dir.magnitude}");
            Rigidbody.AddForce(dir);
        }
    }

    void FixedUpdate()
    {
        LastVelocity = Rigidbody.velocity;
    }
}
