using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    [Header("Scene Names")]
    public string FirstGameSceneName = "GameScene_01";

    private SaveData _pendingLoad;

    public void NewGame()
    {
        SaveSystem.DeleteSave();

        // Optional safety:
        if (GameManager.Instance != null && GameManager.Instance.FlagManager != null)
            GameManager.Instance.FlagManager.ClearAllFlags();

        SceneManager.LoadScene(FirstGameSceneName);
    }

    public void LoadGame()
    {
        if (!SaveSystem.HasSave())
        {
            Debug.Log("No save found.");
            return;
        }

        _pendingLoad = SaveSystem.Load();
        if (_pendingLoad == null)
        {
            Debug.LogError("Failed to load save.");
            return;
        }

        // Subscribe FIRST (critical)
        SceneManager.sceneLoaded -= OnSceneLoadedApply;
        SceneManager.sceneLoaded += OnSceneLoadedApply;

        SceneManager.LoadScene(_pendingLoad.sceneName);
    }

    private void OnSceneLoadedApply(Scene scene, LoadSceneMode mode)
    {
        if (_pendingLoad == null) return;
        if (scene.name != _pendingLoad.sceneName) return;

        SceneManager.sceneLoaded -= OnSceneLoadedApply;

        if (GameManager.Instance != null)
            GameManager.Instance.ApplySaveData(_pendingLoad);

        _pendingLoad = null;
    }
}
