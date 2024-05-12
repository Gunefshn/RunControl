using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gunef;
using TMPro;
using UnityEngine.UI;
using System;


public class MainMenu_Manager : MonoBehaviour
{
    MemoryManager _MemoryManager = new MemoryManager();
    DataManager _DataManager = new DataManager();
    public GameObject ExitPanel;
    public List<ItemInformation> _DefaultItemInformation = new List<ItemInformation>();
    public List<LanguageDataMainObject> _DefaultLanguageDatas = new List<LanguageDataMainObject>();
    public AudioSource ButtonSound;

    public List<LanguageDataMainObject> _LanguageDataMainObject = new List<LanguageDataMainObject>();
    List<LanguageDataMainObject> _LanguageReadData = new List<LanguageDataMainObject>();
    public TextMeshProUGUI[] TextObjects;
    public GameObject LoadingPanel;
    public Slider LoadingSlider;

    void Start()
    {
        _MemoryManager.CheckAndIdentify();
        _DataManager.FirstFileCreate(_DefaultItemInformation, _DefaultLanguageDatas);
        ButtonSound.volume = _MemoryManager.ReadData_Float("MenuFX");
        // _MemoryManager.SaveData_String("Language", "TR");
        _DataManager.Language_Load();
        _LanguageReadData = _DataManager.LanguageDataTransferList();
        _LanguageDataMainObject.Add(_LanguageReadData[0]);
        LanguagePreferenceManagement();

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

    public void LoadScene(int Index)
    {
        ButtonSound.Play();
        SceneManager.LoadScene(Index);
    }

    public void Play()
    {
        ButtonSound.Play();
        // SceneManager.LoadScene(_MemoryManager.ReadData_Int("LastLevel"));
        StartCoroutine(LoadAsync(_MemoryManager.ReadData_Int("LastLevel")));

    }

    IEnumerator LoadAsync(int SceneIndex)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(SceneIndex);
        LoadingPanel.SetActive(true);

        while (!Operation.isDone) //sahne yüklemesi tamamlanmadý ise
        {
            float progress = Mathf.Clamp01(Operation.progress / .9f);
            LoadingSlider.value =progress;
            yield return null;
        }
    }

    public void ExitProcess(string state)
    {
        ButtonSound.Play();
        if (state == "Yes")
        {
            Application.Quit();
        }
        else if (state == "Exit")
        {
            ExitPanel.SetActive(true);
        }
        else
        {
            ExitPanel.SetActive(false);
        }
    }
}
