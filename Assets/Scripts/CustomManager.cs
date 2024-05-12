using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gunef;
using UnityEngine.SceneManagement;
using System;


public class CustomManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public GameObject[] ProcessPanel;
    public GameObject ProcessCanvas;
    public GameObject[] GeneralPanels;
    public Button[] ProcessButton;
    int ActiveProcessPanelIndex;

    [Header("Hats")]
    public GameObject[] Hats;
    public Button[] HatButtons; 
    public TextMeshProUGUI HatText;
    [Header("Sticks")]
    public GameObject[] Sticks;
    public Button[] StickButtons;
    public TextMeshProUGUI StickText;
    [Header("Materials")]
    public Material[] Materials;
    public Material DefaultMaterial;
    public Button[] MaterialsButtons;
    public TextMeshProUGUI MaterialText;
    public SkinnedMeshRenderer _Renderer;

    int HatIndex= -1;
    int StickIndex = -1;
    int MaterialIndex = -1;

    MemoryManager _MemoryManager = new MemoryManager();
    DataManager     _DataManager = new DataManager();

    [Header("General Datas")]
    public Animator Saved_Animator;
    public AudioSource[] Sounds;
    public List<ItemInformation> _ItemInformation = new List<ItemInformation>();
    public List<LanguageDataMainObject> _LanguageDataMainObject = new List<LanguageDataMainObject>();
    List<LanguageDataMainObject> _LanguageReadData = new List<LanguageDataMainObject>();
    public TextMeshProUGUI[] TextObjects;

    string BuyingText;
    string ItemText;

    private void Start()
    {
        // Debug.Log(Application.persistentDataPath); // dosya dizinimi
        ScoreText.text = _MemoryManager.ReadData_Int("Score").ToString();
        _MemoryManager.SaveData_String("Language", "EN");
        _DataManager.Load();
        _ItemInformation = _DataManager.TransferList();

        CheckStatus(0, true);
        CheckStatus(1, true);
        CheckStatus(2, true);

        foreach (var item in Sounds)
        {
            item.volume = _MemoryManager.ReadData_Float("MenuFX");
        }
        _DataManager.Language_Load();
        _LanguageReadData = _DataManager.LanguageDataTransferList();
        _LanguageDataMainObject.Add(_LanguageReadData[1]);
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
            BuyingText = _LanguageDataMainObject[0].LanguageData_TR[5].Text;
            ItemText = _LanguageDataMainObject[0].LanguageData_TR[4].Text;
        }
        else // Dil tercihi ingilizce ise
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = _LanguageDataMainObject[0].LanguageData_EN[i].Text;
            }
            BuyingText = _LanguageDataMainObject[0].LanguageData_EN[5].Text;
            ItemText = _LanguageDataMainObject[0].LanguageData_EN[4].Text;
        }
    }


    public void CheckStatus(int Section,bool Process=false)
    {
        if (Section == 0)
        {
            if (_MemoryManager.ReadData_Int("ActiveHat") == -1) //Varsayýlan deðer olarak -1 verdik þapka olmama durumu
            {
                foreach (var item in Hats)
                {
                    item.SetActive(false);
                }
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;  // eðer aktif þapka ItemText ise satýn alma ve kaydetme butonlarý pasif olmalý
                ProcessButton[1].interactable = false;
                if (!Process)
                {
                    HatIndex = -1;
                    HatText.text = ItemText;
                }
            }
            else
            {
                foreach (var item in Hats)
                {
                    item.SetActive(false);
                }
                //daha önce bir þapka seçilmiþ ise
                HatIndex = _MemoryManager.ReadData_Int("ActiveHat");
                Hats[HatIndex].SetActive(true);
                HatText.text = _ItemInformation[HatIndex].Item_Name;
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;
                ProcessButton[1].interactable = true;
            }

        }
        else if(Section == 1)
        {
            if (_MemoryManager.ReadData_Int("ActiveStick") == -1) //Varsayýlan deðer olarak -1 olmama durumu
            {
                foreach (var item in Sticks)
                {
                    item.SetActive(false);
                }
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;  
                ProcessButton[1].interactable = false;
                if (!Process)
                {
                    StickIndex = -1;
                    StickText.text = ItemText;
                }

            }
            else
            {
                foreach (var item in Sticks)
                {
                    item.SetActive(false);
                }
                StickIndex = _MemoryManager.ReadData_Int("ActiveStick");
                Sticks[StickIndex].SetActive(true);
                StickText.text = _ItemInformation[StickIndex+3].Item_Name;
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;
                ProcessButton[1].interactable = true;
            }
        }
        else
        {
            if (_MemoryManager.ReadData_Int("ActiveMaterial") == -1) //Varsayýlan deðer olarak -1 verdik þapka olmama durumu
            {
                if (!Process)
                {
                    TextObjects[5].text = BuyingText ;
                    MaterialIndex = -1;
                    MaterialText.text = ItemText;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = false;
                }
                else
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = DefaultMaterial;
                    _Renderer.materials = mats;
                    TextObjects[5].text = BuyingText ;
                }
            }
            else
            {
                
                //daha önce bir materyal seçilmiþ ise
                MaterialIndex = _MemoryManager.ReadData_Int("ActiveMaterial");
                Material[] mats = _Renderer.materials;
                mats[0] = Materials[MaterialIndex];
                _Renderer.materials = mats;
                MaterialText.text = _ItemInformation[MaterialIndex+6].Item_Name;
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;
                ProcessButton[1].interactable = true;
            }
        }
    }

    public void Buy()
    {
        Sounds[1].Play();
        if (ActiveProcessPanelIndex != -1)
        {
            switch (ActiveProcessPanelIndex)
            {
                case 0:
                    BuyingResult(HatIndex);
                    break;
                case 1:
                    int Index = StickIndex + 3;
                    BuyingResult(Index);
                    break;
                case 2:
                    int Index2 = MaterialIndex + 6;
                    BuyingResult(Index2);
                    break;
            }

        }

    }
    public void Save()
    {
        Sounds[2].Play();
        if (ActiveProcessPanelIndex != -1)
        {
            switch (ActiveProcessPanelIndex)
            {
                case 0:
                    SavingResult("ActiveHat", HatIndex);
                    break;
                case 1:
                    SavingResult("ActiveStick", StickIndex);
                    break;
                case 2:
                    SavingResult("ActiveMaterial", MaterialIndex);
                    break;
            }
        }
    }

    public void HatDirectionButton(string process)
    {
        Sounds[0].Play();
        if (process == "forward")
        {
            if (HatIndex == -1)
            {
                HatIndex = 0;
                Hats[HatIndex].SetActive(true);
                HatText.text=_ItemInformation[HatIndex].Item_Name;
                if (!_ItemInformation[HatIndex].Purchase_Status)
                { // Satýn alma durumu false ise satýn al butonu aktif
                    TextObjects[5].text = _ItemInformation[HatIndex].Score +" " + BuyingText ;
                    ProcessButton[1].interactable = false;
                    if (_MemoryManager.ReadData_Int("Score")< _ItemInformation[HatIndex].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                    { 
                        ProcessButton[0].interactable = false;
                    }
                    else //puan yeterli ise satýn al aktif 
                    {
                        ProcessButton[0].interactable = true;
                    }
                }
                else// obje daha önce satýn alýnmýþ ise
                {
                    TextObjects[5].text =BuyingText ;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = true;
                }
            }
            else
            {
                Hats[HatIndex].SetActive(false); //Öncelikle mevcut þapka false edilir
                HatIndex++;
                Hats[HatIndex].SetActive(true);
                HatText.text = _ItemInformation[HatIndex].Item_Name;
                if (!_ItemInformation[HatIndex].Purchase_Status)
                { // Satýn alma durumu false ise satýn al butonu aktif
                    TextObjects[5].text = _ItemInformation[HatIndex].Score + " " + BuyingText ;
                    ProcessButton[1].interactable = false;
                    if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[HatIndex].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                    {
                        ProcessButton[0].interactable = false;
                    }
                    else //puan yeterli ise satýn al aktif 
                    {
                        ProcessButton[0].interactable = true;
                    }
                }
                else// obje daha önce satýn alýnmýþ ise
                {
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = true;
                }
            }
            if (HatIndex == Hats.Length - 1)
            {
                //ileri butonunda sona gelindi
                HatButtons[1].interactable = false; // buton týklanýlabilirliðini kapat
            }
            else
            {
                HatButtons[1].interactable = true;
            }
            if(HatIndex != -1)
            {
                HatButtons[0].interactable = true;
            }
        }
        else //back button
        {
            if (HatIndex != -1)
            {
                //sona gelinmediyse
                Hats[HatIndex].SetActive(false);
                HatIndex--;
                if (HatIndex != -1)
                {
                    Hats[HatIndex].SetActive(true);
                    HatButtons[0].interactable = true;
                    HatText.text = _ItemInformation[HatIndex].Item_Name;
                    if (!_ItemInformation[HatIndex].Purchase_Status)
                    { // Satýn alma durumu false ise satýn al butonu aktif
                        TextObjects[5].text = _ItemInformation[HatIndex].Score + " " + BuyingText ;
                        ProcessButton[1].interactable = false;
                        if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[HatIndex].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                        {
                            ProcessButton[0].interactable = false;
                        }
                        else //puan yeterli ise satýn al aktif 
                        {
                            ProcessButton[0].interactable = true;
                        }
                    }
                    else// obje daha önce satýn alýnmýþ ise
                    {
                        TextObjects[5].text = BuyingText ;
                        ProcessButton[0].interactable = false;
                        ProcessButton[1].interactable = true;
                    }
                }
                else
                {
                    HatButtons[0].interactable = false;
                    HatText.text = ItemText;
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                }
            }
            else
            {
                //sona gelindiyse back button false olmalý
                HatButtons[0].interactable = false;
                HatText.text = ItemText;
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;
            }
            if (HatIndex != Hats.Length - 1)
            {
                HatButtons[1].interactable = true;
            }
        }
    }
    public void StickDirectionButton(string process)
    {
        Sounds[0].Play();
        if (process == "forward")
        {
            if (StickIndex == -1)
            {
                StickIndex = 0;
                Sticks[StickIndex].SetActive(true);
                StickText.text = _ItemInformation[StickIndex+3].Item_Name;

                if (!_ItemInformation[StickIndex+3].Purchase_Status)
                { // Satýn alma durumu false ise satýn al butonu aktif
                    TextObjects[5].text = _ItemInformation[StickIndex+3].Score + " " + BuyingText ;
                    ProcessButton[1].interactable = false;
                    if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[StickIndex+3].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                    {
                        ProcessButton[0].interactable = false;
                    }
                    else //puan yeterli ise satýn al aktif 
                    {
                        ProcessButton[0].interactable = true;
                    }
                }
                else// obje daha önce satýn alýnmýþ ise
                {
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = true;
                }
            }
            else
            {
                Sticks[StickIndex].SetActive(false); //Öncelikle mevcut þapka false edilir
                StickIndex++;
                Sticks[StickIndex].SetActive(true);
                StickText.text = _ItemInformation[StickIndex+3].Item_Name;
                if (!_ItemInformation[StickIndex].Purchase_Status)
                { // Satýn alma durumu false ise satýn al butonu aktif
                    TextObjects[5].text = _ItemInformation[StickIndex].Score + " " + BuyingText ;
                    ProcessButton[1].interactable = false;
                    if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[StickIndex + 3].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                    {
                        ProcessButton[0].interactable = false;
                    }
                    else //puan yeterli ise satýn al aktif 
                    {
                        ProcessButton[0].interactable = true;
                    }
                }
                else// obje daha önce satýn alýnmýþ ise
                {
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = true;
                }
            }
            if (StickIndex == Sticks.Length - 1)
            {
                //ileri butonunda sona gelindi
                StickButtons[1].interactable = false; // buton týklanýlabilirliðini kapat
            }
            else
            {
                StickButtons[1].interactable = true;
            }
            if (StickIndex != -1)
            {
                StickButtons[0].interactable = true;
            }
        }
        else //back button
        {
            if (StickIndex != -1)
            {
                //sona gelinmediyse
                Sticks[StickIndex].SetActive(false);
                StickIndex--;
                if (StickIndex != -1)
                {
                    Sticks[StickIndex].SetActive(true);
                    StickButtons[0].interactable = true;
                    StickText.text = _ItemInformation[StickIndex + 3].Item_Name;
                    if (!_ItemInformation[StickIndex].Purchase_Status)
                    { // Satýn alma durumu false ise satýn al butonu aktif
                        TextObjects[5].text = _ItemInformation[StickIndex].Score + " " + BuyingText ;
                        ProcessButton[1].interactable = false;
                        if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[StickIndex + 3].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                        {
                            ProcessButton[0].interactable = false;
                        }
                        else //puan yeterli ise satýn al aktif 
                        {
                            ProcessButton[0].interactable = true;
                        }
                    }
                    else// obje daha önce satýn alýnmýþ ise
                    {
                        TextObjects[5].text = BuyingText ;
                        ProcessButton[0].interactable = false;
                        ProcessButton[1].interactable = true;
                    }
                }
                else
                {
                    StickButtons[0].interactable = false;
                    StickText.text = ItemText;
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                }
            }
            else
            {
                //sona gelindiyse back button false olmalý
                StickButtons[0].interactable = false;
                StickText.text = ItemText;
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;
            }
            if (StickIndex != Sticks.Length - 1)
            {
                StickButtons[1].interactable = true;
            }
        }
    }
    public void MaterialDirectionButton(string process)
    {
        Sounds[0].Play();
        if (process == "forward")
        {
            if (MaterialIndex == -1)
            {
                MaterialIndex = 0;
                Material[] mats = _Renderer.materials;
                mats[0] = Materials[MaterialIndex];
                _Renderer.materials = mats; 
                MaterialText.text = _ItemInformation[MaterialIndex + 6].Item_Name;

                if (!_ItemInformation[MaterialIndex+6].Purchase_Status)
                { // Satýn alma durumu false ise satýn al butonu aktif
                    TextObjects[5].text = _ItemInformation[MaterialIndex+6].Score + " " + BuyingText ;
                    ProcessButton[1].interactable = false;
                    if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[MaterialIndex+6].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                    {
                        ProcessButton[0].interactable = false;
                    }
                    else //puan yeterli ise satýn al aktif 
                    {
                        ProcessButton[0].interactable = true;
                    }
                }
                else// obje daha önce satýn alýnmýþ ise
                {
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = true;
                }
            }
            else
            {
                MaterialIndex++;
                Material[] mats = _Renderer.materials;
                mats[0] = Materials[MaterialIndex];
                _Renderer.materials = mats;
                MaterialText.text = _ItemInformation[MaterialIndex + 6].Item_Name;

                if (!_ItemInformation[MaterialIndex + 6].Purchase_Status)
                { // Satýn alma durumu false ise satýn al butonu aktif
                    TextObjects[5].text = _ItemInformation[MaterialIndex + 6].Score + " " + BuyingText ;
                    ProcessButton[1].interactable = false;
                    if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[MaterialIndex + 6].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                    {
                        ProcessButton[0].interactable = false;
                    }
                    else //puan yeterli ise satýn al aktif 
                    {
                        ProcessButton[0].interactable = true;
                    }
                }
                else// obje daha önce satýn alýnmýþ ise
                {
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                    ProcessButton[1].interactable = true;
                }
            }
            if (MaterialIndex == Materials.Length - 1)
            {
                //ileri butonunda sona gelindi
               MaterialsButtons[1].interactable = false; // buton týklanýlabilirliðini kapat
            }
            else
            {
               MaterialsButtons[1].interactable = true;
            }
            if (MaterialIndex != -1)
            {
               MaterialsButtons[0].interactable = true;
            }
        }
        else //back button
        {
            if (MaterialIndex != -1)
            {
                //sona gelinmediyse
                MaterialIndex--;
                if (MaterialIndex != -1)
                {
                   
                    Material[] mats = _Renderer.materials;
                    mats[0] = Materials[MaterialIndex];
                    _Renderer.materials = mats;
                    MaterialsButtons[0].interactable = true;
                    MaterialText.text = _ItemInformation[MaterialIndex + 6].Item_Name;

                    if (!_ItemInformation[MaterialIndex + 6].Purchase_Status)
                    { // Satýn alma durumu false ise satýn al butonu aktif
                        TextObjects[5].text = _ItemInformation[MaterialIndex + 6].Score + " " + BuyingText ;
                        ProcessButton[1].interactable = false;
                        if (_MemoryManager.ReadData_Int("Score") < _ItemInformation[MaterialIndex + 6].Score) // eðer puanýmýz satýn alma puanýndan küçükse
                        {
                            ProcessButton[0].interactable = false;
                        }
                        else //puan yeterli ise satýn al aktif 
                        {
                            ProcessButton[0].interactable = true;
                        }
                    }
                    else// obje daha önce satýn alýnmýþ ise
                    {
                        TextObjects[5].text = BuyingText ;
                        ProcessButton[0].interactable = false;
                        ProcessButton[1].interactable = true;
                    }
                }
                else
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = DefaultMaterial;
                    _Renderer.materials = mats;

                    MaterialsButtons[0].interactable = false;
                    MaterialText.text = ItemText;
                    TextObjects[5].text = BuyingText ;
                    ProcessButton[0].interactable = false;
                }
            }
            else
            {
                Material[] mats = _Renderer.materials;
                mats[0] = DefaultMaterial;
                _Renderer.materials = mats;
                //sona gelindiyse back button false olmalý
                MaterialsButtons[0].interactable = false;
                MaterialText.text = ItemText;
                TextObjects[5].text = BuyingText ;
                ProcessButton[0].interactable = false;
            }
            if (MaterialIndex != Materials.Length - 1)
            {
               MaterialsButtons[1].interactable = true;
            }
        }
    }

    public void ShowProcessPanel(int Index)
    {
        Sounds[0].Play();
        CheckStatus(Index);
        GeneralPanels[0].SetActive(true);
        ActiveProcessPanelIndex =Index;
        ProcessPanel[Index].SetActive(true);
        GeneralPanels[1].SetActive(true);
        ProcessCanvas.SetActive(false);
       
       
    }
    public void Back()
    {
        Sounds[0].Play();
        GeneralPanels[0].SetActive(false);
        ProcessCanvas.SetActive(true);
        GeneralPanels[1].SetActive(false);
        ProcessPanel[ActiveProcessPanelIndex].SetActive(false);
        CheckStatus(ActiveProcessPanelIndex,true);
        ActiveProcessPanelIndex = -1;
       
    }
    public void BackToMainMenu()
    {
        Sounds[0].Play();
        _DataManager.Save(_ItemInformation);
        SceneManager.LoadScene(0);
    }


    //........................................................
    void BuyingResult(int Index)
    {
        _ItemInformation[Index].Purchase_Status = true;
        _MemoryManager.SaveData_Int("Score", _MemoryManager.ReadData_Int("Score") - _ItemInformation[Index].Score); // Satýn alma durumu sonrasý puan güncelleme
        TextObjects[5].text = BuyingText ;
        ProcessButton[0].interactable = false;
        ProcessButton[1].interactable = true;
        ScoreText.text = _MemoryManager.ReadData_Int("Score").ToString();
    }
    void SavingResult(string key, int Index)
    {
        _MemoryManager.SaveData_Int(key, Index);
        ProcessButton[1].interactable = false;
        if (!Saved_Animator.GetBool("ok"))
        {
            Saved_Animator.SetBool("ok", true);
        }
    }
}
