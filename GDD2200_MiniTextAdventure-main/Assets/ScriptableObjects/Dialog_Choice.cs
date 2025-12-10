using System;
using System.Collections.Generic;

[Serializable]
public class DialogueChoice
{
    public string ChoiceText;
    public string NextNodeId;

    public SceneChange SceneChangeAsset;

    public List<string> RequiredFlags = new List<string>();
    public List<string> ForbiddenFlags = new List<string>();
    public List<string> GrantFlags = new List<string>();
}
