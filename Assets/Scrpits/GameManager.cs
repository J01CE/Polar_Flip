using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton that controls all game-flow events: death, retry, level win, scene loading.
///
/// Can be added to a scene via PolarFlip > Setup Game Flow in Scene (for full UI),
/// OR it will auto-create itself at runtime as a fallback (no UI panels, but
/// scene transitions still work correctly).
/// </summary>
public class GameManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            // Try to find one in the scene
            _instance = FindFirstObjectByType<GameManager>();
            if (_instance != null) return _instance;

            // Last resort: auto-create (no UI panels, but scene flow works)
            var go = new GameObject("GameManager [Auto]");
            _instance = go.AddComponent<GameManager>();
            Debug.LogWarning("GameManager: Auto-created at runtime. " +
                             "Run PolarFlip ► Setup Game Flow in Scene for full UI support.");
            return _instance;
        }
    }

    // ── Inspector Refs ────────────────────────────────────
    [Header("UI Panels  (optional — assign via Setup Game Flow)")]
    [SerializeField] private GameObject tryAgainPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject congratsPanel;

    [Header("Settings")]
    [SerializeField] private int lastLevelBuildIndex = 5;
    [SerializeField] private string comingSoonScene   = "coming soon";

    // ── Retry delay when no UI is available ───────────────
    [SerializeField] private float autoRetryDelay  = 1.5f;
    [SerializeField] private float autoWinDelay    = 1.0f;

    // ── Internal ──────────────────────────────────────────
    private Rigidbody2D ballRb;
    private Transform   spawnPoint;
    private bool        isGameOver = false;

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    void Start()
    {
        HideAllPanels();

        GameObject ball = GameObject.FindWithTag("ball");
        if (ball != null)
            ballRb = ball.GetComponent<Rigidbody2D>();

        // Try SpawnPoint tag first; if the tag isn't defined yet fall back to name "start"
        try
        {
            GameObject spawn = GameObject.FindWithTag("SpawnPoint");
            if (spawn != null) spawnPoint = spawn.transform;
        }
        catch (UnityEngine.UnityException)
        {
            GameObject spawn = GameObject.Find("start");
            if (spawn != null) spawnPoint = spawn.transform;
            else Debug.LogWarning("GameManager: Could not find spawn point. " +
                                  "Tag 'start' object as SpawnPoint or name it 'start'.");
        }
    }

    // ── Public API ────────────────────────────────────────

    public void BallFell()
    {
        if (isGameOver) return;
        isGameOver = true;
        FreezeBall();

        if (tryAgainPanel != null)
        {
            tryAgainPanel.SetActive(true);   // show UI
        }
        else
        {
            // No UI: auto-retry after a short pause
            Invoke(nameof(RetryLevel), autoRetryDelay);
        }
    }

    public void BallWon()
    {
        if (isGameOver) return;
        isGameOver = true;
        FreezeBall();

        int current = SceneManager.GetActiveScene().buildIndex;
        bool isLastLevel = current >= lastLevelBuildIndex;

        if (isLastLevel)
        {
            // Load "coming soon" scene after final level
            Invoke(nameof(LoadComingSoon), autoWinDelay);
        }
        else if (!isLastLevel && winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            // No UI: auto-advance
            Invoke(nameof(LoadNextLevel), autoWinDelay);
        }
    }

    // ── Button Callbacks ──────────────────────────────────

    public void RetryLevel()
    {
        if (ballRb != null)
        {
            ballRb.isKinematic    = false;
            ballRb.linearVelocity  = Vector2.zero;
            ballRb.angularVelocity = 0f;
            if (spawnPoint != null)
                ballRb.transform.position = spawnPoint.position;
        }
        HideAllPanels();
        isGameOver = false;
    }

    public void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);  // build index 1 = l1
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadComingSoon()
    {
        SceneManager.LoadScene(comingSoonScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ── Helpers ───────────────────────────────────────────
    void FreezeBall()
    {
        if (ballRb == null) return;
        ballRb.linearVelocity  = Vector2.zero;
        ballRb.angularVelocity = 0f;
        ballRb.isKinematic     = true;
    }

    void HideAllPanels()
    {
        tryAgainPanel?.SetActive(false);
        winPanel?.SetActive(false);
        congratsPanel?.SetActive(false);
    }
}
