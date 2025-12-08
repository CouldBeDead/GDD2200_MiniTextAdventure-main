using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Scene Change")]
public class SceneChange : ScriptableObject
{
    [Header("Scene Info")]
    public string SceneName;  // exact name from Build Settings

    [Header("Flags")]
    public bool SaveFlagsBeforeLeaving = true;
}