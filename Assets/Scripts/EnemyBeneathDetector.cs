
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyBeneathDetector : MonoBehaviour
{
    public bool HasEnemyBeneath => mEnemySet.Count > 0;
    private readonly HashSet<Enemy> mEnemySet = new HashSet<Enemy>();

    public Vector2 GetKillPoint()
    {
        if (mEnemySet.Count == 0)
            return new Vector2();

        return mEnemySet.First().KillPoint.transform.position;
    }

    public void KillCollidingEnemies()
    {
        List<Enemy> allEnemies = new List<Enemy>(mEnemySet);
        foreach (var enemy in allEnemies) {
            if (enemy != null)
                enemy.Kill();
        }
        mEnemySet.Clear();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy") {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                mEnemySet.Add(enemy);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy") {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                mEnemySet.Remove(enemy);
        }
    }
}
