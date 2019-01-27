
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI mText;

    void Awake()
    {
        mText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Color c = mText.color;
        c.a = Mathf.Abs(1.0f - TimeManager.timeSinceLevelLoad % 2.0f);
        mText.color = c;
    }
}
