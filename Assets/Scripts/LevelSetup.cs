
using UnityEngine;

public sealed class LevelSetup : MonoBehaviour
{
    public GameObject LightTop;
    public GameObject LightBottom;
    public GameObject TopFloor;

    void LateUpdate()
    {
        var cam = Camera.main;
        Vector3 top = cam.ScreenToWorldPoint(new Vector3(0.0f, cam.pixelHeight, cam.nearClipPlane));
        Vector3 bottom = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, cam.nearClipPlane));
        float screenHeight = Mathf.Abs(bottom.y - top.y);

        Vector3 topPos = new Vector3(0.0f, top.y, 0.0f);
        float maxY = TopFloor.transform.position.y;
        if (topPos.y > maxY)
            topPos.y = maxY;
        LightTop.transform.position = topPos;

        Vector3 bottomPos = new Vector3(0.0f, bottom.y, 0.0f);
        LightBottom.transform.position = bottomPos;

        var scale = LightTop.transform.localScale;
        scale.y = (float)Screen.height / Screen.width / 1.4f;
        LightTop.transform.localScale = scale;

        scale = LightBottom.transform.localScale;
        scale.y = (float)Screen.height / Screen.width / 1.4f;
        LightBottom.transform.localScale = scale;
    }
}
