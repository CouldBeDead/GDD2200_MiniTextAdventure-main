using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Data")] 
    public DialogueDatabase Database;
    public FlagManager FlagManager;
    public string StartNodeId;

    // Event signature matches DialogueUI.UpdateUI
    public delegate void DialogueUpdated(string speakerName, string dialogueText, List<DialogueChoice> choices);
    public event DialogueUpdated OnDialogueUpdated;
    
    private DialogueNode _currentDialogueNode;
    private List<DialogueChoice> _currentFilteredChoices;   // shared filtered choices

   public bool StartedFromSave = false;

    private void Start()
    {
        // Only auto-start if not overridden by save system
        if (!StartedFromSave)
        {
            GoToNode(StartNodeId);
        }
    }

    private void ReloadScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        Debug.Log("[DialogueManager] Reloading scene: " + currentScene.name);
        SceneManager.LoadScene(currentScene.name);
    }

    private bool IsChoiceAvailable(DialogueChoice choice)
    {
        if (choice == null) return false;

        if (choice.RequiredFlags != null)
        {
            foreach (var required in choice.RequiredFlags)
            {
                if (!FlagManager.HasFlag(required))
                    return false;
            }
        }

        if (choice.ForbiddenFlags != null)
        {
            foreach (var forbidden in choice.ForbiddenFlags)
            {
                if (FlagManager.HasFlag(forbidden))
                    return false;
            }
        }
        
        return true;
    }

    private List<DialogueChoice> FilterChoices(List<DialogueChoice> choices)
    {
        var result = new List<DialogueChoice>();

        if (choices == null)
            return result;

        foreach (var choice in choices)
        {
            if (IsChoiceAvailable(choice))
                result.Add(choice);
        }
        
        return result;
    }

    public void SelectChoice(int index)
    {
        if (_currentFilteredChoices == null || index < 0 || index >= _currentFilteredChoices.Count)
            return;

        var choice = _currentFilteredChoices[index];

        // Grant flags from choice
        foreach (var flag in choice.GrantFlags)
            FlagManager.AddFlag(flag);

        // If a SceneChange asset exists, handle scene transition
        if (choice.SceneChangeAsset != null)
        {
            var scene = choice.SceneChangeAsset;

            // Save flags if needed
            if (scene.SaveFlagsBeforeLeaving)
            {
                var flags = FlagManager.GetAllFlags();
                FlagSaveSystem.SaveFlags(flags);
            }

            SceneManager.LoadScene(scene.SceneName);
            return;
        }

        // Otherwise follow dialogue as usual
        GoToNode(choice.NextNodeId);
    }

    public void GoToNode(string nodeId)
    {
        _currentDialogueNode = Database.GetNode(nodeId);

        if (_currentDialogueNode == null)
        {
            _currentFilteredChoices = new List<DialogueChoice>();
            OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", _currentFilteredChoices);
            return;
        }

        _currentFilteredChoices = FilterChoices(_currentDialogueNode.Choices);

        OnDialogueUpdated?.Invoke(
            _currentDialogueNode.SpeakerName,
            _currentDialogueNode.DialogueText,
            _currentFilteredChoices
        );
    }
}