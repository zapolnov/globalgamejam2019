
using UnityEngine;
using UnityEngine.UI;

public sealed class CoinCounter : MonoBehaviour
{
    public Player Player;

    private Text mText;

    void Awake()
    {
        mText = GetComponent<Text>();
    }

    void Update()
    {
        mText.text = Player.CoinsCollected.ToString();
    }
}
