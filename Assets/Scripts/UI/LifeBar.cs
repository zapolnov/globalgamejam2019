
using System.Collections.Generic;
using UnityEngine;

public sealed class LifeBar : MonoBehaviour
{
    public Player Player;
    public GameObject LifePrefab;

    private readonly Stack<GameObject> Lives = new Stack<GameObject>();

    void Update()
    {
        int count = Player.Lives;
        if (count < 0)
            count = 0;

        while (Lives.Count < count) {
            var image = Instantiate(LifePrefab, transform);
            var rt = image.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 0.5f);
            rt.offsetMin = new Vector2(-25.0f * (Lives.Count + 1), 5.0f);
            rt.offsetMax = new Vector2(-5.0f - 25.0f * (Lives.Count), -5.0f);
            Lives.Push(image);
        }

        while (Lives.Count > count) {
            GameObject live = Lives.Pop();
            Destroy(live);
        }
    }
}
