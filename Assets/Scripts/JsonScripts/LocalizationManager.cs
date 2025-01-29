using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.Net.NetworkInformation;
using Unity.VisualScripting;

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
            GetUrlFileText();
            StartCoroutine(CopyFileFromWeb(URL));
        }
        else
        {
            LoadFileContent();
        }
        
    }

    private void GetUrlFileText()
    {
        switch (LANGUAGE_CHOOSE)
        {
           case LocaleApplication.EN:
                URL = "https://github.com/YashD007/Localization_UnityC-/blob/main/Assets/Resources/Json.EN.Txt";
                break;
                case LocaleApplication.PT:
                URL = "https://github.com/YashD007/Localization_UnityC-/blob/main/Assets/Resources/Json.PT.Txt";
                break;
                default:
                URL = "https://github.com/YashD007/Localization_UnityC-/blob/main/Assets/Resources/Json.EN.Txt";
                break;
        }
    }

    IEnumerator CopyFileFromWeb(string localurl)
    {
        UnityWebRequest www = UnityWebRequest.Get(localurl);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            Debug.LogWarning("we try a Second Attempt");
            CopyFileFromResources();
            yield break;
        }

        LOADED_JSON_TEXT= www.downloadHandler.text;
        Debug.Log("We Copy the file on a string");
        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
        Debug.Log("We Write on The Streaming Assets Folder");
        StartCoroutine (WaitCreationFile());
        
    }

    private void LoadFileContent() 
    {
        
    }

    private void CopyFileFromResources()
    {
        
    }

    IEnumerator WaitCreationFile()
    {

        FileInfo  myFile = new FileInfo(FULL_PATH_TEXT_FILE);
        float timeOut = 0.0f;

        while(timeOut < 5.0f && !IsFileFinishCreate(myFile))
        {
            timeOut += Time.deltaTime;
            yield return null;
        }
        Debug.Log("File Created Successfully");
    }

    private bool IsFileFinishCreate(FileInfo file)
    {

        FileStream stream = null;
        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            _IsFileFound = true;
            Debug.Log("File Found Successfully");
            return true;
        }
        finally
        {
            if(stream != null)
            {
                stream.Close();
            }
        }
        //File Not found
        _IsFileFound = false;
        return true;
    }    
     
}
