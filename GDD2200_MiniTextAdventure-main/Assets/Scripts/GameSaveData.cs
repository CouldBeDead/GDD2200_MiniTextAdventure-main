using System;
using System.Collections.Generic;

// This is what we serialize to JSON
[Serializable]
public class GameSaveData
{
    public List<string> flags = new();
}
