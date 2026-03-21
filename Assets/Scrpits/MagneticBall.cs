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
    public float attractSpeed  = 8f;
    public float repelForce    = 15f;

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
            isPositive = !isPositive;

            // Small upward kick to help player navigate after flip
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 3f);

            UpdateColor();
        }
    }

    // ─────────────────────────────────────────────────────
    void FixedUpdate()
    {
        bool isAttracting = false;

        foreach (MagneticObject obj in magneticObjects)
        {
            if (obj == null) continue;

            Vector2 toObject = (Vector2)obj.transform.position - (Vector2)transform.position;
            float   distance = toObject.magnitude;

            // Outside trigger range — skip
            if (distance > triggerHeight) continue;

            bool samePolarity = (isPositive == obj.isPositive);

            if (samePolarity)
            {
                // ── Repel ──────────────────────────────────────────
                // Push ball directly away; force grows stronger the closer it is
                Vector2 repelDir      = -toObject.normalized;
                float   forceMagnitude = repelForce * (1f - distance / triggerHeight);
                rb.AddForce(repelDir * forceMagnitude, ForceMode2D.Force);
            }
            else
            {
                // ── Attract ────────────────────────────────────────
                // Pull ball toward a hover position just above the magnet object
                Vector2 hoverTarget = (Vector2)obj.transform.position + Vector2.up * hoverHeight;
                Vector2 toTarget    = hoverTarget - (Vector2)transform.position;

                isAttracting = true;

                // Apply force toward hover point; scale by distance for smooth arrival
                float pullStrength = attractSpeed * Mathf.Clamp01(toTarget.magnitude / triggerHeight);
                rb.AddForce(toTarget.normalized * pullStrength * attractSpeed, ForceMode2D.Force);

                // Dampen velocity when very close to avoid oscillation
                if (toTarget.magnitude < 0.6f)
                    rb.linearVelocity *= 0.80f;
            }
        }

        // Reduce gravity so magnetic attraction can actually lift the ball
        rb.gravityScale = isAttracting ? 0.15f : 1f;
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
