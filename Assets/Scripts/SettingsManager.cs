using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gunef;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public AudioSource ButtonSound;
    public Slider MenuSound;
    public Slider MenuFX;
    public Slider GameSound;
    MemoryManager _MemoryManager = new MemoryManager();
    DataManager _DataManager = new DataManager();

    public List<LanguageDataMainObject> _LanguageDataMainObject = new List<LanguageDataMainObject>();
    List<LanguageDataMainObject> _LanguageReadData = new List<LanguageDataMainObject>();
    public TextMeshProUGUI[] TextObjects;
    [Header("Language Prefence Objects")]
    public TextMeshProUGUI LanguageText;
    public Button[] LanguageButton;
#pragma warning disable IDE0052 // Okunmamýþ özel üyeleri kaldýr
    int CurentLanguageIndex;
#pragma warning restore IDE0052 // Okunmamýþ özel üyeleri kaldýr

    void Start()
    {   
        ButtonSound.volume = _MemoryManager.ReadData_Float("MenuFX");
        MenuSound.value = _MemoryManager.ReadData_Float("MenuSound");
        MenuFX.value = _MemoryManager.ReadData_Float("MenuFX");
        GameSound.value = _MemoryManager.ReadData_Float("GameSound");

        //_MemoryManager.SaveData_String("Language", "TR");
        _DataManager.Language_Load();
        _LanguageReadData = _DataManager.LanguageDataTransferList();
        _LanguageDataMainObject.Add(_LanguageReadData[4]);
        LanguagePreferenceManagement();
        CheckLanguageState();

    }
    private void LanguagePreferenceManagement()
    {
        if (_MemoryManager.ReadData_String("Language") == "TR") // Dil tercihi türkçe ise
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = _LanguageDataMainObject[0].LanguageData_TR[i].Text;
            }
        }
        else // Dil tercihi ingilizce ise
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = _LanguageDataMainObject[0].LanguageData_EN[i].Text;
            }
        }
    }
    public void AdjustSound(string Settings)
    {
        switch (Settings)
        {
            case "MenuSound":
               // Debug.Log("Menu sound " + MenuSound.value);
                _MemoryManager.SaveData_Float("MenuSound",MenuSound.value);
                break;
            case "MenuFX":
                //Debug.Log("Menu fx " + MenuFX.value);
                _MemoryManager.SaveData_Float("MenuFX", MenuFX.value);
                break;
            case "GameSound":
                // Debug.Log("Game sound " + GameSound.value);
                _MemoryManager.SaveData_Float("GameSound", GameSound.value);
                break;
        }
    }
    public void BackToMainMenu()
    {
        ButtonSound.Play();
        SceneManager.LoadScene(0);

    }

    void CheckLanguageState()
    {
        if (_MemoryManager.ReadData_String("Language") == "TR"){
            CurentLanguageIndex = 0;
            LanguageButton[0].interactable = false;
            LanguageText.text = "TÜRKÇE";
            LanguageButton[1].interactable = true;
        }
        else // dil ingilizce ise
        {
            CurentLanguageIndex = 1;
            LanguageButton[0].interactable = true;
            LanguageText.text = "ENGLISH";
            LanguageButton[1].interactable = false;
        }
        
    }
    public void ChangeLanguage(string direction)
    {
        if(direction == "forward")
        {
            CurentLanguageIndex = 1;
            LanguageText.text = "ENGLISH";
            LanguageButton[1].interactable = false;
            LanguageButton[0].interactable = true;
            _MemoryManager.SaveData_String("Language", "EN");
            LanguagePreferenceManagement();
        }
        else if(direction == "back")
        {
            CurentLanguageIndex = 0;
            LanguageText.text = "TÜRKÇE";
            LanguageButton[0].interactable = false;
            LanguageButton[1].interactable = true;
            _MemoryManager.SaveData_String("Language", "TR");
            LanguagePreferenceManagement();
        }
        ButtonSound.Play();
    }
}
