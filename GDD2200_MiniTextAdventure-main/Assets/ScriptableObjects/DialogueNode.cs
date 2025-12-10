using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string NodeId;
    public string SpeakerName;
    [TextArea(3,10)]
    public string DialogueText;

    public List<DialogueChoice> Choices = new List<DialogueChoice>();
}
