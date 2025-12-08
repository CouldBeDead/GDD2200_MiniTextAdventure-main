using UnityEngine;
using System.Collections.Generic;

public static class FlagSaveSystem
{
    private const string Key = "PLAYER_FLAGS";

    public static void SaveFlags(List<string> flags)
    {
        PlayerPrefs.SetString(Key, string.Join("|", flags));
        PlayerPrefs.Save();
    }

    public static List<string> LoadFlags()
    {
        if (!PlayerPrefs.HasKey(Key)) 
            return new List<string>();

        string raw = PlayerPrefs.GetString(Key);
        if (string.IsNullOrEmpty(raw))
            return new List<string>();

        return new List<string>(raw.Split('|'));
    }
}
