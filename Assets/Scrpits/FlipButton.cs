using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to the Flip UI button GameObject.
/// Auto-wires itself — no Inspector configuration needed.
/// Calls MagneticBall.FlipPolarity() when tapped.
/// </summary>
[RequireComponent(typeof(Button))]
public class FlipButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnFlip);
    }

    void OnFlip()
    {
        var ball = FindFirstObjectByType<MagneticBall>();
        if (ball != null)
            ball.FlipPolarity();
        else
            Debug.LogWarning("FlipButton: No MagneticBall found in scene.");
    }
}
