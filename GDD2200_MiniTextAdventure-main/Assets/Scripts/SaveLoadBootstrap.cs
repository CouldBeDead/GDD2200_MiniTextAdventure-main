using System.Collections.Generic;
using UnityEngine;

public class SaveLoadBootstrap : MonoBehaviour
{
    public FlagManager FlagManager;
    public DialogueManager DialogueManager;

    private void Awake()
    {
        if (FlagManager == null)
            FlagManager = FindObjectOfType<FlagManager>();

        if (DialogueManager == null)
            DialogueManager = FindObjectOfType<DialogueManager>();

        if (FlagManager == null || DialogueManager == null)
        {
            Debug.LogError("[SaveLoadBootstrap] Missing FlagManager or DialogueManager reference.");
            return;
        }

        // Try to load flags from JSON
        if (!JsonSaveSystem.TryLoad(out var loadedFlags))
        {
            Debug.Log("[SaveLoadBootstrap] No save file found. Starting fresh.");
            return; // DialogueManager.Start() will do its normal StartNodeId flow
        }

        // Restore flags
        FlagManager.LoadFlags(loadedFlags);

        // Default start node
        string startNode = DialogueManager.StartNodeId;

        // Find the "latest" checkpoint flag: flags that start with "cp_"
        string bestCheckpointFlag = null;

        foreach (var flag in loadedFlags)
        {
            if (string.IsNullOrEmpty(flag)) continue;
            if (!flag.StartsWith("cp_")) continue;

            // Pick the lexicographically largest cp_ flag as "latest"
            if (bestCheckpointFlag == null ||
                string.CompareOrdinal(flag, bestCheckpointFlag) > 0)
            {
                bestCheckpointFlag = flag;
            }
        }

        if (!string.IsNullOrEmpty(bestCheckpointFlag))
        {
            // Strip "cp_" prefix to get nodeId
            startNode = bestCheckpointFlag.Substring(3);
            Debug.Log($"[SaveLoadBootstrap] Resuming from checkpoint flag '{bestCheckpointFlag}' → node '{startNode}'.");
        }
        else
        {
            Debug.Log("[SaveLoadBootstrap] No checkpoint flags found. Starting at default StartNodeId.");
        }

        // Prevent DialogueManager.Start() from auto-starting again
        DialogueManager.StartedFromSave = true;
        DialogueManager.GoToNode(startNode);
    }
}
