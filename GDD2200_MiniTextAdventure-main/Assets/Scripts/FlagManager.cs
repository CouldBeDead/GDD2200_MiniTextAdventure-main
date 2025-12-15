using UnityEngine;
using System.Collections.Generic;

public class FlagManager : MonoBehaviour
{
    private readonly HashSet<string> _flags = new HashSet<string>();

    /// <summary>Adds a flag (does nothing if it already exists). Auto-saves when newly added.</summary>
    public void AddFlag(string flag, bool suppressSave = false)
    {
        if (string.IsNullOrWhiteSpace(flag)) return;

        bool added = _flags.Add(flag);
        if (!added) return;

        if (!suppressSave && GameManager.Instance != null)
            GameManager.Instance.SaveNow();
    }

    /// <summary>Checks whether a flag is set.</summary>
    public bool HasFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag)) return false;
        return _flags.Contains(flag);
    }

    public void ClearAllFlags() => _flags.Clear();

    public List<string> GetAllFlags() => new List<string>(_flags);
}
