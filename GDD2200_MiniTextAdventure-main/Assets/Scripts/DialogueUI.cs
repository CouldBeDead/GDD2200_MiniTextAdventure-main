using UnityEngine;
using TMPro;
using System.Collections;
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

    [Header("Typing Effect")]
    public float TypingSpeed = 0.03f;

    private Coroutine typingCoroutine;

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

        // Apply speaker color
        Color speakerColor = GetSpeakerColor(speaker);
        SpeakerTextDisplay.color = speakerColor;
        DialogueTextDisplay.color = speakerColor;

        // Stop previous typing if needed
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));

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

    private IEnumerator TypeDialogue(string dialogue)
    {
        DialogueTextDisplay.text = "";

        foreach (char c in dialogue)
        {
            DialogueTextDisplay.text += c;
            yield return new WaitForSeconds(TypingSpeed);
        }
    }

    private Color GetSpeakerColor(string speaker)
    {
        switch (speaker)
        {
            case "Narrator":
                return Color.blue;

            case "Blockman":
                return Color.green;

            case "Jamie Olive Oil":
                return Color.yellow;

            case "Muffled Voice":
                return Color.gray;

            case "Goat Man":
                return new Color(0.6f, 0.3f, 0.8f); // Purple

            default:
                return Color.white;
        }
    }

    public void OnChoiceClicked(int index)
    {
        DM.SelectChoice(index);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
