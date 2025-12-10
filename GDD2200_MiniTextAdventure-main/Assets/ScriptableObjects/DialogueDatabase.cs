using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Database")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueNode> Nodes = new List<DialogueNode>();
    private Dictionary<string, DialogueNode> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, DialogueNode>();
        foreach (var node in Nodes)
        {
            if (!lookup.ContainsKey(node.NodeId))
                lookup.Add(node.NodeId, node);
        }
    }

    public DialogueNode GetNode(string id)
    {
        lookup ??= new Dictionary<string, DialogueNode>();
        return lookup.ContainsKey(id) ? lookup[id] : null;
    }
}
