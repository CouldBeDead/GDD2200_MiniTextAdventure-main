using UnityEngine;
using System.Collections.Generic;

public class FlagManager : MonoBehaviour
{
    // Internal storage (fast lookups, no duplicates)
    private HashSet<string> flags = new HashSet<string>();

    // --- PUBLIC API ---

    /// <summary>
    /// Adds a flag if not already present.
    /// </summary>
    public void AddFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag))
        {
            Debug.LogWarning("[FlagManager] Tried to add an empty flag.");
            return;
        }

        flags.Add(flag);
    }

    /// <summary>
    /// Removes a flag if present.
    /// </summary>
    public void RemoveFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag))
            return;

        flags.Remove(flag);
    }

    /// <summary>
    /// Checks whether a flag is active.
    /// </summary>
    public bool HasFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag))
            return false;

        return flags.Contains(flag);
    }

    /// <summary>
    /// Replaces all existing flags with a new set.
    /// Used when loading saves.
    /// </summary>
    public void SetFlags(List<string> newFlags)
    {
        flags.Clear();

        if (newFlags == null)
            return;

        foreach (var f in newFlags)
        {
            if (!string.IsNullOrWhiteSpace(f))
                flags.Add(f);
        }
    }

    /// <summary>
    /// Clears everything (used when starting a new game).
    /// </summary>
    public void ClearAllFlags()
    {
        flags.Clear();
    }

    /// <summary>
    /// Returns a copy of all flags for saving.
    /// </summary>
    public List<string> GetAllFlags()
    {
        return new List<string>(flags);
    }
}
