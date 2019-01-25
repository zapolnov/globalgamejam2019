
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    public float MinJump = 10.0f;
    public float MaxJump = 50.0f;
    public float JumpScale = 10.0f;

    private Vector3 mStartPosition;
    private Rigidbody2D mRigidbody;
    private bool mDragging;

    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            mStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mDragging = true;
        } else if (Input.GetMouseButtonUp(0) && mDragging) {
            mDragging = false;
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mStartPosition;
            dir *= JumpScale;
            float length = dir.magnitude;
            if (length < MinJump) {
                dir /= length;
                dir *= MinJump;
            } else if (length > MaxJump) {
                dir /= length;
                dir *= MaxJump;
            }
            Debug.Log($"{dir.magnitude}");
            mRigidbody.AddForce(dir);
        }
    }
}
