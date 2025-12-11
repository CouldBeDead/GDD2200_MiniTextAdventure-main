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

    private void OnEnable()
    {
        DM.OnDialogueUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        DM.OnDialogueUpdated -= UpdateUI;
    }
    
    private void UpdateUI(string speaker, string dialogue, List<DialogueChoice> choices)
    {
        SpeakerTextDisplay.text = speaker;
        DialogueTextDisplay.text = dialogue;

        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i < choices.Count)
            {
                Buttons[i].gameObject.SetActive(true);
                ButtonLabels[i].text = choices[i].ChoiceText;
            }
            else
            {
                Buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnChoiceClicked(int index)
    {
        DM.SelectChoice(index);
        // reset the button
        EventSystem.current.SetSelectedGameObject(null);
    }
}