using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public DialogueManager DM;
    public TextMeshProUGUI SpeakerTextDisplay;
    public TextMeshProUGUI DialogueTextDisplay;
    public List<Button> Buttons;
    public List<TextMeshProUGUI> ButtonLabels;

    // Track how many choices we currently have
    private int _currentChoiceCount = 0;

    private void OnEnable()
    {
        if (DM != null)
            DM.OnDialogueUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        if (DM != null)
            DM.OnDialogueUpdated -= UpdateUI;
    }

    private void UpdateUI(string speaker, string dialogue, List<DialogueChoice> choices)
    {
        SpeakerTextDisplay.text = speaker;
        DialogueTextDisplay.text = dialogue;

        // Handle null list safely
        if (choices == null)
            choices = new List<DialogueChoice>();

        _currentChoiceCount = choices.Count;

        // First, reset all buttons: hide + remove listeners
        for (int i = 0; i < Buttons.Count; i++)
        {
            var btn = Buttons[i];
            if (btn == null) continue;

            btn.onClick.RemoveAllListeners();
            btn.gameObject.SetActive(false);
        }

        // Now enable only as many buttons as there are choices
        for (int i = 0; i < Buttons.Count && i < choices.Count; i++)
        {
            var btn = Buttons[i];
            var label = ButtonLabels[i];

            btn.gameObject.SetActive(true);
            label.text = choices[i].ChoiceText;

            int capturedIndex = i; // capture for the lambda
            btn.onClick.AddListener(() => OnChoiceClicked(capturedIndex));
        }
    }

    public void OnChoiceClicked(int index)
    {
        // Guard: if choices changed (e.g., flags filtered) between update & click
        if (index < 0 || index >= _currentChoiceCount)
        {
            Debug.LogWarning($"[DialogueUI] Ignoring click: index {index} out of range (choiceCount = {_currentChoiceCount})");
            return;
        }

        if (DM == null)
        {
            Debug.LogError("[DialogueUI] DialogueManager reference is null.");
            return;
        }

        DM.SelectChoice(index);

        // reset the button selection so keyboard/gamepad focus doesn't stick
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
