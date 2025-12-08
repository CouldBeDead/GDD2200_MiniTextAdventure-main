using UnityEngine;
using System.Collections.Generic;

public class FlagManager : MonoBehaviour
{
    private HashSet<string> _flags = new();

    public bool HasFlag(string flag)
    {
        return _flags.Contains(flag);
    }

    public void AddFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return;
        _flags.Add(flag);
    }

    // NEW – used for saving
    public List<string> GetAllFlags()
{
    return new List<string>(_flags);
}

public void LoadFlags(IEnumerable<string> flags)
{
    _flags.Clear();
    if (flags == null) return;

    foreach (var flag in flags)
    {
        if (!string.IsNullOrEmpty(flag))
            _flags.Add(flag);
    }
}

}
