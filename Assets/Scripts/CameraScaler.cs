
using UnityEngine;

public sealed class CameraScaler : MonoBehaviour
{
    public float WidthToBeSeen = 640.0f; 

    void Update()
    {
        Camera.main.orthographicSize = WidthToBeSeen * Screen.height / Screen.width * 0.5f;
    }
}
