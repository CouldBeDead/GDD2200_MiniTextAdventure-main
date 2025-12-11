using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Data")] 
    public DialogueDatabase Database;
    public FlagManager FlagManager;
    public string StartNodeId;

    public delegate void DialogueUpdated(string speakerName, string dialogueText, List<DialogueChoice> choices);
    public event DialogueUpdated OnDialogueUpdated;
    
    private DialogueNode _currentDialogueNode;

    private void Start()
    {
        GoToNode(StartNodeId);
    }

    private void ReloadScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private bool IsChoiceAvailable(DialogueChoice choice)
    {
        foreach (var required in choice.RequiredFlags)
        {
            if (!FlagManager.HasFlag(required)) return false;
        }

        foreach (var forbidden in choice.ForbiddenFlags)
        {
            if (FlagManager.HasFlag(forbidden)) return false;
        }
        
        return true;
    }

    private List<DialogueChoice> FilterChoices(List<DialogueChoice> choices)
    {
        var result = new List<DialogueChoice>();

        foreach (var choice in choices)
        {
            if (IsChoiceAvailable(choice))
            {
                result.Add(choice);
            }
        }
        
        return result;
    }

    public void SelectChoice(int index)
    {
        var filtered = FilterChoices(_currentDialogueNode.Choices);
        var choice = filtered[index];

        foreach (var flag in choice.GrantFlags)
        {
            FlagManager.AddFlag(flag);
        }

        if (choice.ReloadScene)
        {
            ReloadScene();
            return;
        }

        GoToNode(choice.NextNodeId);
    }

    public void GoToNode(string nodeId)
    {
        _currentDialogueNode = Database.GetNode(nodeId);

        if (_currentDialogueNode == null)
        {
            OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", null);
            return;
        }
        
        var filtered = FilterChoices(_currentDialogueNode.Choices);
        {
            OnDialogueUpdated?.Invoke(_currentDialogueNode.SpeakerName, _currentDialogueNode.DialogueText, filtered);
        }
    }
}