using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene References (auto-bound)")]
    public FlagManager FlagManager;
    public DialogueManager DialogueManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        RebindSceneRefs();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindSceneRefs();
    }

    private void RebindSceneRefs()
    {
        // Unity 2023+:
        FlagManager = FindFirstObjectByType<FlagManager>();
        DialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    public void ApplySaveData(SaveData data)
    {
        if (data == null) return;

        // Ensure current scene refs are valid
        if (FlagManager == null || DialogueManager == null)
            RebindSceneRefs();

        // Flags first
        if (FlagManager != null)
        {
            FlagManager.ClearAllFlags();
            foreach (var f in data.flags)
                FlagManager.AddFlag(f, suppressSave: true);
        }
        else
        {
            Debug.LogWarning("GameManager.ApplySaveData: FlagManager not found in scene.");
        }

        // Prevent DialogueManager.Start() from overriding the loaded node
        if (DialogueManager != null)
        {
            DialogueManager.SuppressAutoStart = true;

            if (!string.IsNullOrWhiteSpace(data.currentNodeId))
                DialogueManager.GoToNode(data.currentNodeId);
        }
        else
        {
            Debug.LogWarning("GameManager.ApplySaveData: DialogueManager not found in scene.");
        }
    }

    public void SaveNow()
    {
        SaveSystem.Save(this);
    }
}
