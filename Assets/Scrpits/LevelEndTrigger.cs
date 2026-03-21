using UnityEngine;

/// <summary>
/// Attach to the 'end' object in each level scene.
/// Uses PHYSICAL COLLISION (not a trigger) so the existing solid collider
/// on the 'end' block is preserved — the ball walks into it normally.
/// </summary>
public class LevelEndTrigger : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("ball")) return;
        GetGameManager()?.BallWon();
    }

    // Also catch it as a trigger in case the designer wants isTrigger = true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ball")) return;
        GetGameManager()?.BallWon();
    }

    GameManager GetGameManager()
    {
        if (GameManager.Instance != null) return GameManager.Instance;
        // Fallback: search the scene (handles script execution order edge cases)
        var gm = FindFirstObjectByType<GameManager>();
        if (gm == null)
            Debug.LogError("LevelEndTrigger: No GameManager found in this scene! " +
                           "Run PolarFlip ► Setup Game Flow in Scene, then save.");
        return gm;
    }
}
