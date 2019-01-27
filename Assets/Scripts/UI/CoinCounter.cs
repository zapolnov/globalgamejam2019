
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class CoinCounter : MonoBehaviour
{
    public Player Player;

    private TextMeshProUGUI mText;

    void Awake()
    {
        mText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        mText.text = Player.CoinsCollected.ToString();
    }
}
