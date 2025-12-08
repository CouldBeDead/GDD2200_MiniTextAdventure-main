using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [Header("Identity")]
    public string NodeId;  // e.g. narrator_intro_01

    [Header("Dialogue")]
    public string SpeakerName;
    [TextArea(2, 5)]
    public string DialogueText;

    [Header("Choices")]
    public List<DialogueChoice> Choices = new();
}

[System.Serializable]   
public class DialogueChoice
{
    [Header("UI")]
    public string ChoiceText;

    [Header("Next Dialogue")]
    public string NextNodeId;

    [Header("Scene Transition")]
    public SceneChange SceneChangeAsset;  // can be null if no scene change

    [Header("Conditions")]
    public List<string> RequiredFlags = new();
    public List<string> ForbiddenFlags = new();

    [Header("Flags On Select")]
    public List<string> GrantFlags = new();
}
