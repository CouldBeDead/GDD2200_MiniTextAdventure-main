using UnityEngine;
using System.Collections.Generic;

public class FlagManager : MonoBehaviour
{
    // Fast lookups, no duplicates
    private HashSet<string> _flags = new HashSet<string>();

    /// <summary>Adds a flag (does nothing if it already exists).</summary>
    public void AddFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag)) return;
        _flags.Add(flag);
    }

    /// <summary>Checks whether a flag is set.</summary>
    public bool HasFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag)) return false;
        return _flags.Contains(flag);
    }

    /// <summary>Clears all flags (use if you want a fresh run).</summary>
    public void ClearAllFlags()
    {
        _flags.Clear();
    }
}
