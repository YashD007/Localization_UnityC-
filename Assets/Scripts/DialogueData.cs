using System.Collections.Generic;

[System.Serializable]
public class DialogueEntry
{
    public int Sr; // Unique identifier for the dialogue
    public string CutsceneInGame; // Scene or context for the dialogue
    public string Speaker; // Name of the speaker
    public string Action; // Optional action (e.g., "laughs")
    public string Dialogue; // The actual dialogue text
}

[System.Serializable]
public class LanguageData
{
    public List<DialogueEntry> texts; // List of all dialogue entries
}