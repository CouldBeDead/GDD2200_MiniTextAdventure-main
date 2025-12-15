using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private const string FileName = "save.json";
    public static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

    public static bool HasSave() => File.Exists(SavePath);

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    public static SaveData Load()
    {
        if (!File.Exists(SavePath)) return null;

        var json = File.ReadAllText(SavePath);
        if (string.IsNullOrWhiteSpace(json)) return null;

        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void Save(GameManager gm)
    {
        if (gm == null)
        {
            Debug.LogError("SaveSystem.Save: GameManager is null.");
            return;
        }

        var data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            currentNodeId = gm.DialogueManager != null ? gm.DialogueManager.GetCurrentNodeId() : "",
            flags = gm.FlagManager != null ? gm.FlagManager.GetAllFlags() : new()
        };

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        // Debug (optional):
        // Debug.Log($"Saved -> {SavePath}\n{json}");
    }
}
