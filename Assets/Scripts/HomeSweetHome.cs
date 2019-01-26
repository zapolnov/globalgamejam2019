
using UnityEngine;

public sealed class HomeSweetHome : MonoBehaviour
{
    public GameObject Empty;
    public GameObject Full;

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player") {
            // FIXME
        }
    }
}
