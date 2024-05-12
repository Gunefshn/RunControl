using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gunef;
using TMPro;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public UnityEngine.UI.Button[] Buttons;
    public int Level;
    MemoryManager _MemoryManager = new MemoryManager();
    DataManager _DataManager = new DataManager();
    public GameObject ExitPanel;
    public Sprite LockedButton;
    public AudioSource ButtonSound;

    public List<LanguageDataMainObject> _LanguageDataMainObject = new List<LanguageDataMainObject>();
    List<LanguageDataMainObject> _LanguageReadData = new List<LanguageDataMainObject>();
    public TextMeshProUGUI[] TextObjects;
    public GameObject LoadingPanel;
    public Slider LoadingSlider;

    private void Start()
    {
        //_MemoryManager.SaveData_String("Language", "TR");
        _DataManager.Language_Load();
        _LanguageReadData = _DataManager.LanguageDataTransferList();
        _LanguageDataMainObject.Add(_LanguageReadData[2]);
        LanguagePreferenceManagement();

        ButtonSound.volume = _MemoryManager.ReadData_Float("MenuFX");
        int CurrentLevel = _MemoryManager.ReadData_Int("LastLevel") - 4;  //level 1 index 5 geldiði için level deðerleri ile örtüþmesi için -4

        int Index = 1;
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (Index <= CurrentLevel)
            {
                Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = Index.ToString();
                int SceneIndex = Index + 4;
                Buttons[i].onClick.AddListener(delegate { LoadScene(SceneIndex); });

            }
            else
            {
                Buttons[i].GetComponent<UnityEngine.UI.Image>().sprite = LockedButton;
                //Buttons[i].interactable = false; // butonlarýn týklanabilirliðini kapatma bu yöntemde butonlarda soluklaþtýrma olur 
                Buttons[i].enabled = false; // buton özelliði kapatýlabilir.
            }
            Index++;
        }
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
        //SceneManager.LoadScene(Index);
        StartCoroutine(LoadAsync(Index));
    }
    IEnumerator LoadAsync(int SceneIndex)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(SceneIndex);
        LoadingPanel.SetActive(true);

        while (!Operation.isDone) //sahne yüklemesi tamamlanmadý ise
        {
            float progress = Mathf.Clamp01(Operation.progress / .9f);
            LoadingSlider.value = progress;
            yield return null;
        }
    }
    public void Back()
    {
        ButtonSound.Play();
        SceneManager.LoadScene(0); // geri butonu ana menüye geri dönmesi için
    }
    /*public void LoadScene()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name); //seçilmiþ objenin name deðerini +4 ile sahne indexi olarak yaz 
        SceneManager.LoadScene(int.Parse(EventSystem.current.currentSelectedGameObject.name)+4);
        // yada name yerine GetComponentInChildren<TextNMeshProUGUI>().text ile de yapýlabilir.
    }
    */

}
