using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    // ── Internal ──────────────────────────────────────────
    private Rigidbody2D rb;
    private float       moveX;

    // ─────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Read input in Update for responsiveness
    void Update()
    {
        // Support both keyboard and touch buttons
        float keyboardInput = Input.GetAxis("Horizontal");
        float touchInput = MoveButton.HeldDirection;
        
        // Sum the inputs, then clamp between -1 and 1
        moveX = Mathf.Clamp(keyboardInput + touchInput, -1f, 1f);
        
        Flip(moveX);
    }

    // Apply movement in FixedUpdate to sync with physics engine
    void FixedUpdate()
    {
        // Only override X velocity; preserve Y so gravity and magnetic forces work correctly
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    // ─────────────────────────────────────────────────────
    // Flip sprite horizontally by negating localScale.x
    // Uses Mathf.Abs to preserve the original scale magnitude (e.g. 7)
    void Flip(float x)
    {
        if (x < -0.01f)
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y, 1f);
        else if (x > 0.01f)
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y, 1f);
    }
}