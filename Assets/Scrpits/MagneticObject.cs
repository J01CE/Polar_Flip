using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    // ── Static Polarity ───────────────────────────────────
    [Header("Polarity  —  Fixed, never changes")]
    public bool isPositive = false;     // set per object in Inspector

    // ── Visual ────────────────────────────────────────────
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color positiveColor = new Color(0.5f, 0.8f, 1f);   // light blue
    public Color negativeColor = new Color(1f, 0.6f, 0.6f);   // light red

    [Header("Debug Info  —  read only")]
    public bool ballIsAbove    = false;  // shows in Inspector during Play
    public bool interacting    = false;  // shows in Inspector during Play

    // ─────────────────────────────────────────────────────
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateColor();
    }

    // ─────────────────────────────────────────────────────
    void Update()
    {
        // Continuously check if ball is above this object
        // This updates the debug flags visible in Inspector
        GameObject ball = GameObject.FindWithTag("ball");
        if (ball == null) return;

        float ballY   = ball.transform.position.y;
        float objectY = transform.position.y;

        ballIsAbove = ballY > objectY + 0.5f;
        interacting = ballIsAbove && ballY < objectY + 4f;
    }

    // ─────────────────────────────────────────────────────
    void UpdateColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = isPositive ? positiveColor : negativeColor;
    }

    // Shows field area above this object in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isPositive ? new Color(0,0,1,0.3f)
                                  : new Color(1,0,0,0.3f);

        // Draw a box above this object showing the trigger zone
        Vector3 center = transform.position + Vector3.up * 2f;
        Vector3 size   = new Vector3(2f, 4f, 0f);
        Gizmos.DrawCube(center, size);
    }
}