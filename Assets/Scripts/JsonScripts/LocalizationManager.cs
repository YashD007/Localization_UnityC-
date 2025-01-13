using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.Net.NetworkInformation;

public class LocalizationManager : MonoBehaviour
{
    [Header ("Important string")]
    private const string FILENAME_PREFIX = "text_";
    private const string FILE_EXTENSION = ".json";
    private string FULL_NAME_TEXT_FILE;

    public string FULL_PATH_TEXT_FILE { get; private set; }

    private string URL;
    private string LANGUAGE_CHOOSE = "EN";
    private string LOADED_JSON_TEXT ="";

    [Header ("Important bool")]
    private bool _IsReady = false;
    private bool _IsFileFound = false;
    private bool _IsTryChangeLangRunTime = false;   

    [Header ("Json Variable")]
    private Dictionary<string, string> _localizedDictionary;
    private LocalizationData _loadedData;



    #region Instance Function
    private static LocalizationManager LocalizationManagerInstance;
    public static LocalizationManager Instance
    {
        get
        {
            if (LocalizationManagerInstance == null)
            {
                LocalizationManagerInstance = FindObjectOfType(typeof(LocalizationManager)) as LocalizationManager;
        }
        return LocalizationManagerInstance;
        }
    }
    #endregion Instance Function


     IEnumerator Start()
    {
        LANGUAGE_CHOOSE = LocaleHelper.GetSupportedLanguageCode();
        FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOOSE.ToLower() + FILE_EXTENSION;

        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE); 

        yield return StartCoroutine(LoadJsonLanguageData());
        _IsReady = true;
    }

    IEnumerator LoadJsonLanguageData()
    {
        CheckFileExist();
        yield return new WaitUntil(() => _IsFileFound);
        _loadedData = JsonUtility.FromJson<LocalizationData>(LOADED_JSON_TEXT);
        _localizedDictionary = new Dictionary<string, string>(_loadedData.items.Count);
        _loadedData.items.ForEach(item => 
        {
            try
            {
                _localizedDictionary.Add(item.key, item.value);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });


    }

    private void CheckFileExist()
    {
        if (File.Exists(FULL_PATH_TEXT_FILE))
        {
            _IsFileFound = true;
            LOADED_JSON_TEXT = File.ReadAllText(FULL_PATH_TEXT_FILE);
        }
        else
        {
            _IsFileFound = false;
            Debug.LogError("File not found");
        }
        
    }
}
