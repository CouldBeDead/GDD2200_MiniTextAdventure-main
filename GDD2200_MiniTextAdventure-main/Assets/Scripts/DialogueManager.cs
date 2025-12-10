using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public DialogueDatabase Database;
    public FlagManager FlagManager;
    public string StartNodeId;

    public event System.Action<string,string,List<DialogueChoice>> OnDialogueUpdated;

    private DialogueNode currentNode;

    public string CurrentNodeId => currentNode != null ? currentNode.NodeId : null;

    public bool StartedFromSave = false;

    private void Start()
    {
        if (!StartedFromSave)
            GoToNode(StartNodeId);
    }

    public void GoToNode(string nodeId)
    {
        currentNode = Database.GetNode(nodeId);

        if (currentNode == null)
        {
            Debug.LogWarning($"Dialogue node '{nodeId}' not found.");
            OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", new List<DialogueChoice>());
            return;
        }

        List<DialogueChoice> filteredChoices = FilterChoices(currentNode.Choices);

        OnDialogueUpdated?.Invoke(
            currentNode.SpeakerName,
            currentNode.DialogueText,
            filteredChoices
        );
    }

    private List<DialogueChoice> FilterChoices(List<DialogueChoice> all)
    {
        List<DialogueChoice> outList = new List<DialogueChoice>();

        foreach (var c in all)
        {
            if (IsAvailable(c))
                outList.Add(c);
        }

        return outList;
    }

    private bool IsAvailable(DialogueChoice choice)
    {
        foreach (var req in choice.RequiredFlags)
            if (!FlagManager.HasFlag(req))
                return false;

        foreach (var forb in choice.ForbiddenFlags)
            if (FlagManager.HasFlag(forb))
                return false;

        return true;
    }

    public void SelectChoice(DialogueChoice choice)
    {
        // Grant flags
        foreach (var flag in choice.GrantFlags)
            FlagManager.AddFlag(flag);

        // Scene change?
        if (choice.SceneChangeAsset != null)
        {
            var s = choice.SceneChangeAsset;

            if (s.SaveFlagsBeforeLeaving)
                JsonSaveSystem.Save(FlagManager, CurrentNodeId);

            SceneManager.LoadScene(s.SceneName);
            return;
        }

        // Continue dialogue
        GoToNode(choice.NextNodeId);
    }
}
