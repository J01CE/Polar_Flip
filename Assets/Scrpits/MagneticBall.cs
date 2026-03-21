using UnityEngine;

public class MagneticBall : MonoBehaviour
{
    // ── Polarity ──────────────────────────────────────────
    [Header("Polarity")]
    public bool isPositive = true;

    // ── Magnet Settings ───────────────────────────────────
    [Header("Magnet Settings")]
    public float triggerHeight = 4f;
    public float hoverHeight   = 1.2f;
    public float attractSpeed  = 10f;
    public float repelForce    = 12f;

    // ── Visual ────────────────────────────────────────────
    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color positiveColor = new Color(0.2f, 0.5f, 1f);
    public Color negativeColor = new Color(1f, 0.3f, 0.3f);

    // ── Internal ──────────────────────────────────────────
    private Rigidbody2D      rb;
    private MagneticObject[] magneticObjects;

    // ─────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateColor();

        // Cache all static magnetic objects once at scene start
        magneticObjects = FindObjectsByType<MagneticObject>(FindObjectsSortMode.None);
    }

    // ─────────────────────────────────────────────────────
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipPolarity();
        }
    }

    /// <summary>
    /// Toggles the ball's polarity, adds a small upward kick, and updates visuals.
    /// Public so it can be called by UI buttons (FlipButton.cs).
    /// </summary>
    public void FlipPolarity()
    {
        isPositive = !isPositive;

        // Small upward kick so the player can navigate after a flip
        if (rb != null)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 3f);

        UpdateColor();
    }

    // ─────────────────────────────────────────────────────
    void FixedUpdate()
    {
        bool reduceGravity = false;

        foreach (MagneticObject obj in magneticObjects)
        {
            if (obj == null) continue;

            Vector2 toObject = (Vector2)obj.transform.position - (Vector2)transform.position;
            float   distance = toObject.magnitude;

            if (distance > triggerHeight) continue;

            // 0 at the edge of range → 1 right at the magnet center
            float t = 1f - (distance / triggerHeight);

            bool samePolarity = (isPositive == obj.isPositive);
            bool magnetBelow  = obj.transform.position.y < transform.position.y - 0.5f;
            bool magnetAbove  = obj.transform.position.y > transform.position.y + 1.0f;

            if (samePolarity)
            {
                // ── REPEL ────────────────────────────────────────────
                // Push ball away from the magnet.
                rb.AddForce(-toObject.normalized * repelForce * t, ForceMode2D.Force);

                // When the magnet is BELOW and we're repelling upward,
                // gravity fights the force and keeps the ball glued to the surface.
                // Reduce gravity so the repel force can actually levitate the ball.
                if (magnetBelow)
                    reduceGravity = true;
            }
            else
            {
                // ── ATTRACT ──────────────────────────────────────────
                // Pull directly toward the magnet — strong, snappy feel.
                rb.AddForce(toObject.normalized * attractSpeed * t * 2f, ForceMode2D.Force);

                // Only reduce gravity when the magnet is ABOVE (need to lift upward).
                // For floor magnets, gravity cooperates with the downward pull naturally.
                if (magnetAbove)
                    reduceGravity = true;
            }
        }

        // 0.4 gravity scale lets the magnetic forces clearly win over gravity
        // without making the ball feel completely weightless.
        rb.gravityScale = reduceGravity ? 0.6f : 1f;
    }

    // ─────────────────────────────────────────────────────
    void UpdateColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = isPositive ? positiveColor : negativeColor;
    }

    // ─────────────────────────────────────────────────────
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isPositive ? Color.blue : Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerHeight);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            transform.position - Vector3.right,
            transform.position + Vector3.right);
    }
}
