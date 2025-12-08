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

    [Header("Typing")]
    public float TypeSpeed = 0.03f;  // seconds per letter
    private Coroutine typingRoutine;
    private bool isTyping = false;
    private string fullText = "";

    private int _currentChoiceCount = 0;

    private void Awake()
    {
        // Automatically wire buttons to correct indices
        if (Buttons != null)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                int capturedIndex = i;
                var button = Buttons[i];
                if (button == null) continue;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnChoiceClicked(capturedIndex));
            }
        }
    }

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
        _currentChoiceCount = choices?.Count ?? 0;

        // Set speaker name text
        SpeakerTextDisplay.text = speaker;

        // 🔹 Apply speaker-specific colors (Narrator / Block Man / Jamie Olive Oil)
        ApplySpeakerColors(speaker);

        // 🔹 Hide all buttons while text is typing
        if (Buttons != null)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i] != null)
                    Buttons[i].gameObject.SetActive(false);
            }
        }

        // 🔹 Start typewriter animation
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(dialogue));

        // 🔹 Show choices AFTER typing finishes
        StartCoroutine(UpdateChoicesAfterTyping(choices));
    }

    // Speaker-based color rules
    private void ApplySpeakerColors(string speaker)
    {
        if (string.IsNullOrEmpty(speaker))
        {
            SpeakerTextDisplay.color = Color.white;
            DialogueTextDisplay.color = Color.white;
            return;
        }

        switch (speaker.ToLower())
        {
            case "narrator":
                SpeakerTextDisplay.color = new Color(0.3f, 0.6f, 1f);   // blue
                DialogueTextDisplay.color = new Color(0.5f, 0.75f, 1f); // light blue
                break;

            case "block man":
            case "blockman": // allow both spellings
                SpeakerTextDisplay.color = new Color(0.1f, 0.8f, 0.1f); // green
                DialogueTextDisplay.color = new Color(0.6f, 1f, 0.6f);  // light green
                break;

            case "jamie olive oil":
                SpeakerTextDisplay.color = new Color(1f, 0.85f, 0f);    // yellow-gold
                DialogueTextDisplay.color = new Color(1f, 0.95f, 0.5f); // soft yellow
                break;

            default:
                SpeakerTextDisplay.color = Color.white;
                DialogueTextDisplay.color = Color.white;
                break;
        }
    }

    private IEnumerator TypeText(string dialogue)
    {
        isTyping = true;
        fullText = dialogue ?? string.Empty;
        DialogueTextDisplay.text = "";

        foreach (char c in fullText)
        {
            DialogueTextDisplay.text += c;
            yield return new WaitForSeconds(TypeSpeed);
        }

        isTyping = false;
    }

    private IEnumerator UpdateChoicesAfterTyping(List<DialogueChoice> choices)
    {
        // Wait until typing finishes
        while (isTyping)
            yield return null;

        if (choices == null)
            choices = new List<DialogueChoice>();

        // After typing finishes, show choices normally
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i < choices.Count)
            {
                Buttons[i].gameObject.SetActive(true);
                ButtonLabels[i].text = choices[i].ChoiceText;
                Buttons[i].interactable = true;
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

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
