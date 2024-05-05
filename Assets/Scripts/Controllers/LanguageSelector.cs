using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    private bool active = false;
    public TMP_Dropdown dropdownLanguageSelector;

    private void Start() 
    {
        int ID = PlayerPrefs.GetInt("LanguageKey", 0);
        dropdownLanguageSelector.value = PlayerPrefs.GetInt("dropdownLanguageValue", 0);
        ChangeLanguage(ID);
    }

    public void DropdownMenu()
    {
        ChangeLanguage(dropdownLanguageSelector.value);
        PlayerPrefs.SetInt("dropdownLanguageValue", dropdownLanguageSelector.value);
    }

    private void ChangeLanguage(int localeID)
    {
        if(active == true)
            return;
        StartCoroutine(SetLanguage(localeID));
    }

    IEnumerator SetLanguage(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LanguageKey", localeID);
        active = false;
    }
}
