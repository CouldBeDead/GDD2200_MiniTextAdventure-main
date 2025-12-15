using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string sceneName;
    public string currentNodeId;
    public List<string> flags = new();
}
