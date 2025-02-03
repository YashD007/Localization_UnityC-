using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private LanguageData currentLanguageData;
    private int currentDialogueIndex = 0; // Track the current dialogue index
    private string currentLanguage = "english"; // Track the current language

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load a language file by name (e.g., "english")
    public void LoadLanguage(string language)
    {
        string filePath = $"Languages/{language}";
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

        if (jsonFile != null)
        {
            currentLanguageData = JsonUtility.FromJson<LanguageData>(jsonFile.text);
            Debug.Log($"Loaded language: {language}");
            //currentDialogueIndex = 0; // Reset index when loading a new language
            currentLanguage = language; // Update the current language
        }
        else
        {
            Debug.LogError($"Language file not found: {language}");
        }
    }

    // Get the next dialogue in sequence
    public string GetNextDialogue()
    {
        if (currentLanguageData == null || currentLanguageData.texts == null)
        {
            Debug.LogError("No language data loaded.");
            return null;
        }

        if (currentDialogueIndex < currentLanguageData.texts.Count)
        {
            DialogueEntry entry = currentLanguageData.texts[currentDialogueIndex];
            currentDialogueIndex++; // Move to the next dialogue
            return entry.Dialogue;
        }
        else
        {
            Debug.Log("No more dialogues available.");
            return null;
        }
    }

    // Reset the dialogue index (optional)
    public void ResetDialogueIndex()
    {
        currentDialogueIndex = 0;
    }

    // Get the current language
    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }
}