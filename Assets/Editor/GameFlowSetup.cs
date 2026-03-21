#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// One-click tool to set up the full game flow system in any level scene.
/// Menu: PolarFlip ▶ Setup Game Flow in Scene
/// </summary>
public static class GameFlowSetup
{
    [MenuItem("PolarFlip/Setup Game Flow in Scene")]
    static void Run()
    {
        // ── 0. Ensure required tags exist ─────────────────────
        EnsureTagExists("SpawnPoint");

        // ── 1. GameManager ────────────────────────────────────
        GameManager gm = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        if (gm == null)
        {
            var gmGO = new GameObject("GameManager");
            Undo.RegisterCreatedObjectUndo(gmGO, "Create GameManager");
            gm = gmGO.AddComponent<GameManager>();
            Debug.Log("✅ Created GameManager");
        }
        else Debug.Log("ℹ️  GameManager already exists — skipping creation.");

        // ── 2. DeathZone ──────────────────────────────────────
        if (UnityEngine.Object.FindFirstObjectByType<DeathZone>() == null)
        {
            var dzGO = new GameObject("DeathZone");
            Undo.RegisterCreatedObjectUndo(dzGO, "Create DeathZone");
            dzGO.transform.position = new Vector3(0, -12, 0);

            var col = dzGO.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(200, 4);   // wide enough to catch ball anywhere

            dzGO.AddComponent<DeathZone>();
            Debug.Log("✅ Created DeathZone at y=-12");
        }
        else Debug.Log("ℹ️  DeathZone already exists — skipping creation.");

        // ── 3. EventSystem ────────────────────────────────────
        if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() == null)
        {
            var esGO = new GameObject("EventSystem");
            Undo.RegisterCreatedObjectUndo(esGO, "Create EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<StandaloneInputModule>();
        }

        // ── 4. Canvas ─────────────────────────────────────────
        var canvasGO = new GameObject("GameCanvas");
        Undo.RegisterCreatedObjectUndo(canvasGO, "Create GameCanvas");

        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // ── 5. Build the three panels ─────────────────────────
        var tapPanel  = BuildPanel(canvasGO.transform, "TryAgainPanel",  "You Fell!",
                                    new Color(0.5f, 0.1f, 0.1f, 0.93f));
        var winPanel  = BuildPanel(canvasGO.transform, "WinPanel",       "Level Complete! 🎉",
                                    new Color(0.1f, 0.35f, 0.1f, 0.93f));
        var conPanel  = BuildPanel(canvasGO.transform, "CongratsPanel",  "You Beat The Game! 🎉",
                                    new Color(0.1f, 0.1f, 0.45f, 0.93f));

        AddButton(tapPanel,  "Try Again",  gm, "RetryLevel",    new Color(0.85f, 0.25f, 0.25f));
        AddButton(tapPanel,  "Main Menu",  gm, "LoadMainMenu",  new Color(0.35f, 0.35f, 0.35f));

        AddButton(winPanel,  "Next Level", gm, "LoadNextLevel", new Color(0.25f, 0.75f, 0.35f));
        AddButton(winPanel,  "Main Menu",  gm, "LoadMainMenu",  new Color(0.35f, 0.35f, 0.35f));

        AddButton(conPanel,  "Play Again", gm, "PlayAgain",     new Color(0.25f, 0.45f, 0.9f));
        AddButton(conPanel,  "Main Menu",  gm, "LoadMainMenu",  new Color(0.35f, 0.35f, 0.35f));

        // Start hidden
        tapPanel.SetActive(false);
        winPanel.SetActive(false);
        conPanel.SetActive(false);

        // ── 6. Wire panels into GameManager via SerializedObject ──
        var so = new SerializedObject(gm);
        so.FindProperty("tryAgainPanel").objectReferenceValue  = tapPanel;
        so.FindProperty("winPanel").objectReferenceValue       = winPanel;
        so.FindProperty("congratsPanel").objectReferenceValue  = conPanel;
        so.ApplyModifiedProperties();

        // ── 7. Mark scene dirty so Ctrl+S saves everything ────
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        Debug.Log("✅ Game Flow Setup complete! " +
                  "Remember to:\n" +
                  "  1. Tag the 'start' object with the 'SpawnPoint' tag\n" +
                  "  2. Add LevelEndTrigger.cs to the 'end' object\n" +
                  "  3. Save the scene (Ctrl+S)");
    }

    // ── Helpers ───────────────────────────────────────────────

    static GameObject BuildPanel(Transform parent, string name, string titleText, Color bgColor)
    {
        // Overlay (fills whole screen, semi-transparent)
        var panelGO = new GameObject(name);
        panelGO.transform.SetParent(parent, false);

        var rt = panelGO.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        var bg = panelGO.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.6f);

        // Inner card
        var cardGO = new GameObject("Card");
        cardGO.transform.SetParent(panelGO.transform, false);

        var cardRT = cardGO.AddComponent<RectTransform>();
        cardRT.anchorMin = new Vector2(0.5f, 0.5f);
        cardRT.anchorMax = new Vector2(0.5f, 0.5f);
        cardRT.pivot     = new Vector2(0.5f, 0.5f);
        cardRT.sizeDelta = new Vector2(500, 320);

        var cardImg = cardGO.AddComponent<Image>();
        cardImg.color = bgColor;

        // Vertical layout on card
        var layout = cardGO.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(30, 30, 30, 30);
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = false;
        layout.childControlWidth  = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth  = true;

        // Title text
        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(cardGO.transform, false);

        var titleRT = titleGO.AddComponent<RectTransform>();
        titleRT.sizeDelta = new Vector2(0, 70);

        var titleTxt = titleGO.AddComponent<Text>();
        titleTxt.text      = titleText;
        titleTxt.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleTxt.fontSize  = 38;
        titleTxt.fontStyle = FontStyle.Bold;
        titleTxt.color     = Color.white;
        titleTxt.alignment = TextAnchor.MiddleCenter;

        return panelGO;
    }

    static void AddButton(GameObject panel, string label, GameManager gm,
                          string methodName, Color btnColor)
    {
        // Find the card child (second child: overlay > card)
        var card = panel.transform.Find("Card");
        if (card == null) { Debug.LogError("Card not found in " + panel.name); return; }

        var btnGO = new GameObject(label);
        btnGO.transform.SetParent(card, false);

        var rt = btnGO.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, 60);

        var img = btnGO.AddComponent<Image>();
        img.color = btnColor;

        var btn = btnGO.AddComponent<Button>();

        // Hover color state
        var colors = btn.colors;
        colors.highlightedColor = btnColor * 1.2f;
        colors.pressedColor     = btnColor * 0.8f;
        btn.colors = colors;

        // Label
        var txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);

        var txtRT = txtGO.AddComponent<RectTransform>();
        txtRT.anchorMin = Vector2.zero;
        txtRT.anchorMax = Vector2.one;
        txtRT.sizeDelta = Vector2.zero;

        var txt = txtGO.AddComponent<Text>();
        txt.text      = label;
        txt.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize  = 24;
        txt.fontStyle = FontStyle.Bold;
        txt.color     = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;

        // Wire OnClick to GameManager method
        MethodInfo mi = typeof(GameManager).GetMethod(methodName,
            BindingFlags.Public | BindingFlags.Instance);
        if (mi != null)
        {
            var action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), gm, mi);
            UnityEventTools.AddPersistentListener(btn.onClick, action);
        }
        else Debug.LogError($"GameFlowSetup: method '{methodName}' not found on GameManager.");
    }
    /// <summary>Registers a tag in Unity's TagManager if it doesn't already exist.</summary>
    static void EnsureTagExists(string tag)
    {
        var tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        var tagsProp = tagManager.FindProperty("tags");

        // Check if already exists
        for (int i = 0; i < tagsProp.arraySize; i++)
            if (tagsProp.GetArrayElementAtIndex(i).stringValue == tag) return;

        // Add it
        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
        tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
        tagManager.ApplyModifiedProperties();
        Debug.Log($"✅ Tag '{tag}' added to TagManager.");
    }
}
#endif
