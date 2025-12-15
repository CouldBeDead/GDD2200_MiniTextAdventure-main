using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Data")]
    public DialogueDatabase Database;
    public FlagManager FlagManager;
    public string StartNodeId;

    [Header("Scenes")]
    public string MenuSceneName = "MainMenu";

    public delegate void DialogueUpdated(string speakerName, string dialogueText, List<DialogueChoice> choices);
    public event DialogueUpdated OnDialogueUpdated;

    // Set true when loading a save so Start() doesn't override the loaded node
    public bool SuppressAutoStart = false;

    private DialogueNode _currentDialogueNode;

    private void Start()
    {
        if (!SuppressAutoStart)
            GoToNode(StartNodeId);
    }

    private void ReloadScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private bool IsChoiceAvailable(DialogueChoice choice)
    {
        if (choice == null) return false;

        // Required flags must all be present
        foreach (var required in choice.RequiredFlags)
            if (!FlagManager.HasFlag(required)) return false;

        // Forbidden flags must all be absent
        foreach (var forbidden in choice.ForbiddenFlags)
            if (FlagManager.HasFlag(forbidden)) return false;

        return true;
    }

    private List<DialogueChoice> FilterChoices(List<DialogueChoice> choices)
    {
        var result = new List<DialogueChoice>();
        if (choices == null) return result;

        foreach (var choice in choices)
            if (IsChoiceAvailable(choice))
                result.Add(choice);

        return result;
    }

    public void SelectChoice(int index)
    {
        if (_currentDialogueNode == null) return;

        var filtered = FilterChoices(_currentDialogueNode.Choices);
        if (index < 0 || index >= filtered.Count)
        {
            Debug.LogWarning($"DialogueManager.SelectChoice: Invalid index {index}.");
            return;
        }

        var choice = filtered[index];

        // Grant flags (auto-saves via FlagManager when newly added)
        if (choice.GrantFlags != null)
        {
            foreach (var flag in choice.GrantFlags)
                FlagManager.AddFlag(flag);
        }

        // Reload current scene
        if (choice.ReloadScene)
        {
            ReloadScene();
            return;
        }

        // Go to next node
        if (!string.IsNullOrWhiteSpace(choice.NextNodeId))
        {
            GoToNode(choice.NextNodeId);
            return;
        }

        // No next node -> end
        OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", null);
    }

    public void GoToNode(string nodeId)
    {
        if (Database == null)
        {
            Debug.LogError("DialogueManager: Database not assigned.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nodeId))
        {
            OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", null);
            return;
        }

        _currentDialogueNode = Database.GetNode(nodeId);

        if (_currentDialogueNode == null)
        {
            OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", null);
            return;
        }

        // Special node type: return to menu
        if (_currentDialogueNode.NodeType == DialogueNodeType.ReturnToMenu)
        {
            SceneManager.LoadScene(MenuSceneName);
            return;
        }

        var filtered = FilterChoices(_currentDialogueNode.Choices);
        OnDialogueUpdated?.Invoke(_currentDialogueNode.SpeakerName, _currentDialogueNode.DialogueText, filtered);
    }

    public string GetCurrentNodeId()
    {
        return _currentDialogueNode != null ? _currentDialogueNode.NodeId : "";
    }
}
