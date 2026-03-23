using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this to any Button — pressing it quits the game.
/// Works in builds; in the Editor it just stops Play mode.
/// </summary>
[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
