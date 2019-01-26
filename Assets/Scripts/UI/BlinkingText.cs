
using UnityEngine;
using UnityEngine.UI;

public sealed class BlinkingText : MonoBehaviour
{
    private Text mText;
    void Awake()
    {
        mText = GetComponent<Text>();
    }

    void Update()
    {
        Color c = mText.color;
        c.a = Mathf.Abs(1.0f - Time.timeSinceLevelLoad % 2.0f);
        mText.color = c;
    }
}
