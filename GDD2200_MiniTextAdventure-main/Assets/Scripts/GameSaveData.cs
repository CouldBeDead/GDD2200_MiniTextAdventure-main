using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<string> flags = new List<string>();
    public string currentNodeId;
}
