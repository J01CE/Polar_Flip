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
    public float attractSpeed  = 5f;
    public float repelForce    = 15f;

    // ── Visual ────────────────────────────────────────────
    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color positiveColor = new Color(0.2f, 0.5f, 1f);
    public Color negativeColor = new Color(1f, 0.3f, 0.3f);

    // ── Internal ──────────────────────────────────────────
    private Rigidbody2D rb;
    private bool        currentPolarity;

    // ─────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        currentPolarity = isPositive;
        UpdateColor();
    }

    // ─────────────────────────────────────────────────────
    void Update()
{
    // Existing F key code stays here
    if (Input.GetKeyDown(KeyCode.F))
    {
        isPositive      = !isPositive;
        currentPolarity = isPositive;
        rb.gravityScale = 1f;
        rb.linearVelocity     = new Vector2(rb.linearVelocity.x, 3f);
        UpdateColor();
        Debug.Log("Polarity: "
            + (currentPolarity ? "POSITIVE(+)" : "NEGATIVE(-)"));
    }

    // ── TEMP TEST — press G to manually toggle gravity ──
    if (Input.GetKeyDown(KeyCode.G))
    {
        rb.gravityScale = rb.gravityScale == 0 ? 1f : 0f;
        Debug.Log("Manual gravity scale: " + rb.gravityScale);
    }
}

    // ─────────────────────────────────────────────────────
    void FixedUpdate()
{
    // TEMP TEST — ignore all magnet logic
    // Just print gravity scale every frame
    Debug.Log("GravityScale this frame: " + rb.gravityScale);
}
    // ─────────────────────────────────────────────────────
    void UpdateColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = currentPolarity
                                   ? positiveColor
                                   : negativeColor;
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
