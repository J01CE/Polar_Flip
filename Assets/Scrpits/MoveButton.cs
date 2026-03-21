using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to the Left or Right UI button GameObject.
/// Set Direction to -1 for Left, +1 for Right in the Inspector.
/// Hold the button to move; release to stop.
/// </summary>
[RequireComponent(typeof(UnityEngine.UI.Image))]
public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("-1 = Left,  +1 = Right")]
    public float direction = 1f;

    /// <summary>Currently held horizontal direction from touch buttons (-1, 0, or 1).</summary>
    public static float HeldDirection { get; private set; } = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        HeldDirection = direction;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Only clear if this button was the one being held
        if (Mathf.Approximately(HeldDirection, direction))
            HeldDirection = 0f;
    }

    // Safety: clear if object is disabled mid-hold
    void OnDisable()
    {
        if (Mathf.Approximately(HeldDirection, direction))
            HeldDirection = 0f;
    }
}
