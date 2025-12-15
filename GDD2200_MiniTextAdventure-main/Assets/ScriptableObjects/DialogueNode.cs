using UnityEngine;
using System.Collections.Generic;

public enum DialogueNodeType
{
    Normal,
    ReturnToMenu
}

[CreateAssetMenu(menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [Header("Identity")]
    public string NodeId;  // e.g. "ending_return_to_menu"

    [Header("Type")]
    public DialogueNodeType NodeType = DialogueNodeType.Normal;

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

    [Header("Flow")]
    public string NextNodeId;
    public bool ReloadScene;

    [Header("Conditions")]
    public List<string> RequiredFlags = new();
    public List<string> ForbiddenFlags = new();

    [Header("Flags On Select")]
    public List<string> GrantFlags = new();
}
