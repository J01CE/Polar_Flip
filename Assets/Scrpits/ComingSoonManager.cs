using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Place this anywhere in the "coming soon" scene.
/// It auto-finds the "main menu" and "quit" buttons by name and wires them up.
/// </summary>
public class ComingSoonManager : MonoBehaviour
{
    void Awake()
    {
        // Wire "main menu" button
        GameObject mainMenuBtn = GameObject.Find("main menu");
        if (mainMenuBtn != null)
        {
            Button btn = mainMenuBtn.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(LoadMainMenu);
        }

        // Wire "quit" button
        GameObject quitBtn = GameObject.Find("quit");
        if (quitBtn != null)
        {
            Button btn = quitBtn.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(QuitGame);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
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
