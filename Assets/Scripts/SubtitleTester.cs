using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubtitleTester : MonoBehaviour
{
    public TMP_Text subtitleText; // Reference to the subtitle text UI
    public Button nextButton; // Reference to the button
    public TMP_Dropdown languageDropdown; // Reference to the language dropdown

    private void Start()
    {
        // Load the default language
        LocalizationManager.Instance.LoadLanguage("english");

        // Add listener to the button
        nextButton.onClick.AddListener(ShowNextDialogue);

        // Add listener to the dropdown
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Optionally, reset the dialogue index when starting
        LocalizationManager.Instance.ResetDialogueIndex();
    }

    private void ShowNextDialogue()
    {
        string dialogue = LocalizationManager.Instance.GetNextDialogue();
        if (dialogue != null)
        {
            subtitleText.text = dialogue;
        }
        else
        {
            subtitleText.text = "No more dialogues.";
        }
    }

    private void OnLanguageChanged(int index)
    {
        string selectedLanguage = languageDropdown.options[index].text.ToLower();
        LocalizationManager.Instance.LoadLanguage(selectedLanguage);
        //LocalizationManager.Instance.ResetDialogueIndex(); // Reset index when changing language
        ShowNextDialogue(); // Show the first dialogue in the new language
    }
}