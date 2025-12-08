using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class JsonSaveSystem
{
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(FlagManager flagManager)
    {
        if (flagManager == null)
        {
            Debug.LogError("[JsonSaveSystem] FlagManager is null, cannot save.");
            return;
        }

        var data = new GameSaveData
        {
            flags = flagManager.GetAllFlags()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[JsonSaveSystem] Saved to {SavePath}");
    }

    public static bool TryLoad(out List<string> flags)
    {
        flags = null;

        if (!File.Exists(SavePath))
        {
            Debug.Log("[JsonSaveSystem] No save file found.");
            return false;
        }

        string json = File.ReadAllText(SavePath);
        var data = JsonUtility.FromJson<GameSaveData>(json);

        if (data == null)
        {
            Debug.LogWarning("[JsonSaveSystem] Failed to parse save file.");
            flags = new List<string>();
            return false;
        }

        flags = data.flags ?? new List<string>();
        Debug.Log($"[JsonSaveSystem] Loaded {flags.Count} flags from save.");
        return true;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[JsonSaveSystem] Deleted save file.");
        }
    }
}
