using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueNode> Nodes = new();

    private Dictionary<string, DialogueNode> _lookup;

    private void OnEnable() => BuildNodeDictionary();
    private void OnValidate() => BuildNodeDictionary();

    private void BuildNodeDictionary()
    {
        _lookup = new Dictionary<string, DialogueNode>();

        foreach (var node in Nodes)
        {
            if (node == null) continue;
            if (string.IsNullOrWhiteSpace(node.NodeId)) continue;

            if (_lookup.ContainsKey(node.NodeId))
            {
                Debug.LogWarning($"DialogueDatabase: Duplicate NodeId '{node.NodeId}'. Skipping duplicate.");
                continue;
            }

            _lookup.Add(node.NodeId, node);
        }
    }

    public DialogueNode GetNode(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return null;

        if (_lookup == null)
            BuildNodeDictionary();

        _lookup.TryGetValue(id, out var node);
        return node;
    }
}
