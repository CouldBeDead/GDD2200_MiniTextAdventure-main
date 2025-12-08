using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Character Profile")]
public class CharacterProfile : ScriptableObject
{
    public string SpeakerName;

    [Header("UI Colors")]
    public Color SpeakerNameColor = Color.white;
    public Color DialogueTextColor = Color.white;
}
