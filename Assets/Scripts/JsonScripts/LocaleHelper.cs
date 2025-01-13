using UnityEngine;
public static class LocaleHelper
{
   public static string GetSupportedLanguageCode()
   {
    SystemLanguage lang = Application.systemLanguage;
    switch (lang)
    {
        case SystemLanguage.English:
            return LocaleApplication.EN;
        case SystemLanguage.Portuguese:
            return LocaleApplication.PT;
        default:
            return GetDefaultSupportedLanguageCode();
    }
   }

   static string GetDefaultSupportedLanguageCode()
   {
       return LocaleApplication.EN;
   }
}
