using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Attach this to any Button GameObject — it auto-wires itself.
/// No need to set anything in the Inspector.
/// </summary>
[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadMainMenu);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
