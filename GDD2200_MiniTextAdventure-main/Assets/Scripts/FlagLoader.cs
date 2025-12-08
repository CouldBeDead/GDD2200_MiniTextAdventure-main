using UnityEngine;

public class FlagLoader : MonoBehaviour
{
    private void Awake()
    {
        var flagManager = FindObjectOfType<FlagManager>();
        if (flagManager == null)
        {
            Debug.LogError("No FlagManager found in scene.");
            return;
        }

        var loaded = FlagSaveSystem.LoadFlags();
        flagManager.LoadFlags(loaded);

        Debug.Log($"Loaded {loaded.Count} flags.");
    }
}
