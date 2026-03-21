using UnityEngine;

/// <summary>
/// Attach to ANY object that should kill the ball on contact:
///   - Works as a TRIGGER (invisible kill zone, e.g. below level)
///   - Works as a SOLID COLLIDER (e.g. the ground platform the ball lands on)
/// No setup needed — just add this component to the object.
/// </summary>
public class DeathZone : MonoBehaviour
{
    void Awake()
    {
        // If the collider is already a trigger, keep it that way.
        // If it's a solid collider, OnCollisionEnter2D handles it — no change needed.
    }

    // ── Trigger version (invisible kill zone below the level) ──
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ball")) return;
        GetGameManager()?.BallFell();
    }

    // ── Collision version (solid ground / platform) ────────────
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("ball")) return;
        GetGameManager()?.BallFell();
    }

    GameManager GetGameManager()
    {
        if (GameManager.Instance != null) return GameManager.Instance;
        var gm = FindFirstObjectByType<GameManager>();
        if (gm == null)
            Debug.LogError("DeathZone: No GameManager found in this scene! " +
                           "Run PolarFlip ► Setup Game Flow in Scene, then save.");
        return gm;
    }
}
