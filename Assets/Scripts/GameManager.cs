using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gunef;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    //public GameObject Target;
    //public GameObject Spawn; // Karakterlerin doðma pozisyonlarý ayarlandýðý için gerek kalmadý
    //public GameObject Destination;
    public static int CharacterCount = 1; //Anlýk karakter sayýsý
    public List<GameObject> Characters;
    public List<GameObject> BornEffects;
    public List<GameObject> DyingEffects;
    public List<GameObject> CharacterBlotEffects;


    [Header("LEVEL DATAS")]
    public List<GameObject> Enemys;
    public int NumberOfEnemy;
    public GameObject _MainCharacter;
    public bool GameOver;
    public bool EndGame; //Oyun sonu trigger tetiklendi mi
    [Header("Hats")]
    public GameObject[] Hats;
    [Header("Sticks")]
    public GameObject[] Sticks;
    [Header("Materials")]
    public Material[] Materials;
    public SkinnedMeshRenderer _Renderer;
    public Material DefaultMaterial;


    Mathematical_Operations _Mathematical_Operations = new Mathematical_Operations();
    MemoryManager _MemoryManager = new MemoryManager();
    DataManager _DataManager = new DataManager();

    Scene _Scene;
    [Header("General")]
    public AudioSource[] Sounds;
    public GameObject[] ProcessPanel;
    public Slider GameSoundSetting;

    public List<LanguageDataMainObject> _LanguageDataMainObject = new List<LanguageDataMainObject>();
    List<LanguageDataMainObject> _LanguageReadData = new List<LanguageDataMainObject>();
    public TextMeshProUGUI[] TextObjects;
    [Header("Loading Datas")]
    public GameObject LoadingPanel;
    public Slider LoadingSlider;

    private void Awake()
    {
        Sounds[0].volume = _MemoryManager.ReadData_Float("GameSound");
        GameSoundSetting.value = _MemoryManager.ReadData_Float("GameSound");
        Sounds[1].volume = _MemoryManager.ReadData_Float("MenuFX");
        Destroy(GameObject.FindWithTag("MenuSound"));
        CheckItem();
    }

    void Start()
    {
        CreateEnemy();
        
       _Scene= SceneManager.GetActiveScene(); // aktif olan sahnenin deðelerini alma 

        _DataManager.Language_Load();
        _LanguageReadData = _DataManager.LanguageDataTransferList();
        _LanguageDataMainObject.Add(_LanguageReadData[5]);
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

    public void CreateEnemy()
    {
        for (int i = 0; i < NumberOfEnemy; i++)
        {
            Enemys[i].SetActive(true);
        }
    }

    public void TriggerEnemy()
    {
        foreach (var item in Enemys) //Düþmanlar listesi içerisindeki
        {
            if (item.activeInHierarchy)//Aktif olan düþmanlar ile
            {
                item.GetComponent<Enemy>().TriggerAnimation();
            }
        }
        EndGame = true;
        FightState();
    }

    void Update()
    {   /*
       if(Input.GetKeyDown(KeyCode.A)) {
            //Instantiate(PrefabCharacter,Spawn.transform.position,Quaternion.identity); //oluþturmasý istenen,oluþturulacak nokta,herhangi bir rotation
            foreach(var item in Characters) {
                 //ilk olarak obje durumu kontrolü
                 if(!item.activeInHierarchy)
                 {
                     //ilk olarak pozisyon verilir daha sonra aktif edilir.
                     item.transform.position = Spawn.transform.position; // Aktif olmayan ilk objeyi spawn noktasýnda oluþturma
                     item.SetActive(true);// Aktif olmayan ilk objeyi aktif et
                     InstantCharacterCount++;
                     break; //sadece ilk pasif karakterde yapmasý tüm listeye uygulamamasý için break 
                 }
             }

         }
            */

    }

    void FightState()
    {
        if (EndGame) //Son tetikteyici tetiklenmiþ ise
        {
            if (CharacterCount == 1 || NumberOfEnemy == 0) //Oyun sonu
            {
                GameOver = true;
                foreach (var item in Enemys)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool("Fight", false);
                    }
                }
                foreach (var item in Characters)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool("Fight", false);
                    }
                }
                _MainCharacter.GetComponent<Animator>().SetBool("Fight", false);
                if (CharacterCount < NumberOfEnemy || CharacterCount == NumberOfEnemy)
                {
                    //Debug.Log("GAME OVER");
                    ProcessPanel[3].SetActive(true);
                }
                else
                {
                    if (CharacterCount > 5)
                    {
                        if (_Scene.buildIndex == _MemoryManager.ReadData_Int("LastLevel"))//açýk olan sahne son level sahneme eþit ise arttýrma yapýlacak
                        {
                            _MemoryManager.SaveData_Int("Puan", _MemoryManager.ReadData_Int("Puan") + 600);
                            _MemoryManager.SaveData_Int("LastLevel", _MemoryManager.ReadData_Int("LastLevel") + 1);
                        } 

                    }
                    else
                    {
                        if (_Scene.buildIndex == _MemoryManager.ReadData_Int("LastLevel"))
                        {

                            _MemoryManager.SaveData_Int("Puan", _MemoryManager.ReadData_Int("Puan") + 200);
                            _MemoryManager.SaveData_Int("LastLevel", _MemoryManager.ReadData_Int("LastLevel") + 1);
                        }
                    }
                    //Debug.Log("Win");
                    ProcessPanel[2].SetActive(true);
                }
            }
        }

    }

    // !!!! Kod girinti çýkýntýlarýný ayarlamak için ctrl+k+f
    /* Ýþlemleri tag ve deðerleri ile yaptýðýmýz için burdaki kalabalýk sadeleþtirildi.
    public void CharacterManager(string data, Transform position)
    {
        switch (data)
        {
            case "x2":
                int number = 0;

                foreach (var item in Characters)
                {
                    if (number < CharacterCount)//Döngü  count belirleme
                    {
                        if (!item.activeInHierarchy)
                        {

                            item.transform.position = position.position; //dýþardan gelen pozisyon ne ise o pozisyonda karakter oluþturmasý saðlanacak
                            item.SetActive(true);
                            number++;

                        }

                    }
                    else
                    {
                        number = 0;
                        break;
                    }


                }
                CharacterCount *= 2;
                break;

            case "+3":
                int number2 = 0;

                foreach (var item in Characters)
                {
                    if (number2 <3)//+3 blok olduðu için döngü count 3 
                    {
                        if (!item.activeInHierarchy)
                        {

                            item.transform.position = position.position;
                            item.SetActive(true);
                            number2++;

                        }

                    }
                    else
                    {
                        number = 0;
                        break;
                    }


                }
                CharacterCount += 3;
                break;

            case "-4":

                if (CharacterCount < 4)
                {
                    foreach (var item in Characters)
                    {
                        item.transform.position = Vector3.zero; //pozisyon sýfýrlama
                        item.SetActive(false); //Eðer anlýk karakter sayýmýz 4 ten küçük ve -4 bloðundan geçiþ olursa karakterler içerisindeki bütün objeler false olacak
                    }
                    CharacterCount = 1;
                }
                else
                {

                    int number3 = 0;

                    foreach (var item in Characters)
                    {
                        if (number3 != 4)
                        {
                            if (item.activeInHierarchy) //Aktif olan karakterler arasýndan bulunmasý gerekiyor
                            {

                                item.transform.position = Vector3.zero;
                                item.SetActive(false);
                                number3++;

                            }
                        }
                        else
                        {
                            number = 0;
                            break;
                        }
                    }
                    CharacterCount -= 4;
                }
                    break;
            case "/2":

                if (CharacterCount <= 2)
                {   //Anlýk karakter sayým 2 veya 2 den küçük ise hepsini pasifleþtirmek gerekiyor.
                    foreach (var item in Characters)
                    {
                        item.transform.position = Vector3.zero; 
                        item.SetActive(false); 
                    }
                    CharacterCount = 1;
                }
                else
                {
                    int division = CharacterCount / 2; // Döngü count belirleme


                    int number3 = 0;

                    foreach (var item in Characters)
                    {
                        if (number3 != division)
                        {
                            if (item.activeInHierarchy) //Aktif olan karakterler arasýndan bulunmasý gerekiyor
                            {

                                item.transform.position = Vector3.zero;
                                item.SetActive(false);
                                number3++;

                            }
                        }
                        else
                        {
                            number = 0;
                            break;
                        }
                    }
                    if(CharacterCount % 2 == 0)
                    {
                        CharacterCount /= 2;
                    }
                    else
                    {  //anlýk karakter sayýsý tek ise mod 1 oluyor bu yüzden karakter sayýsý +1 dememiz gerekiyor.
                        CharacterCount /= 2;
                        CharacterCount++;
                    }
                }
                break;

        }
    }*/

    public void CharacterManager(string processType, int incomingNumber, Transform position)
    {
        switch (processType)
        {
            case "Multiply":
                _Mathematical_Operations.Multiply(incomingNumber, Characters, position, BornEffects);
                break;

            case "Add":
                _Mathematical_Operations.Add(incomingNumber, Characters, position, BornEffects);
                break;

            case "Subtract":
                _Mathematical_Operations.Subtract(incomingNumber, Characters, DyingEffects);
                break;

            case "Divide":
                _Mathematical_Operations.Divide(incomingNumber, Characters, DyingEffects);
                break;

        }
    }

    public void CreateDyingEffect(Vector3 Position, bool Hammer = false, bool State = false)
    {
        foreach (var item in DyingEffects)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true); //ilk olarak ölme efekti aktif edilir.
                item.transform.position = Position;//daha sonra efekt konumu bizim karakter pozisyonumuzda oluþturulur.
                item.GetComponent<ParticleSystem>().Play();//oluþturulan particle effekt oynatýlýr.Oluþturulan particle effect kendini disable ettiði için ekstra bir þey yapmaya gerek yok.
                item.GetComponent<AudioSource>().Play();
                if (!State)
                {
                    CharacterCount--; // Anlýk karakter sayýsýný düþürme
                }
                else
                {
                    NumberOfEnemy--; //Düþman sayýsýný düþürme
                }

                break;
            }
        }

        if (Hammer)
        {
            Vector3 newPos = new Vector3(Position.x, .005f, Position.z);
            foreach (var item in CharacterBlotEffects)
            {
                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);
                    item.transform.position = newPos;
                    break;
                }
            }
        }
        if (!GameOver)
        {
            FightState();
        }
    }

    public void CheckItem()
    {   
        if(_MemoryManager.ReadData_Int("ActiveHat") != -1){
            Hats[_MemoryManager.ReadData_Int("ActiveHat")].SetActive(true);
        }
        if(_MemoryManager.ReadData_Int("ActiveStick") != -1)
        {
            Sticks[_MemoryManager.ReadData_Int("ActiveStick")].SetActive(true);
        }
        if (_MemoryManager.ReadData_Int("ActiveMaterial") != -1)
        {
            Material[] mats = _Renderer.materials;
            mats[0] = Materials[_MemoryManager.ReadData_Int("ActiveMaterial")];
            _Renderer.materials = mats;
        }
        else
        {
            Material[] mats = _Renderer.materials;
            mats[0] = DefaultMaterial;
            _Renderer.materials = mats;
        }
    }
    public void ExitProcess(string state)
    {
        Sounds[1].Play();
        Time.timeScale = 0; // oyunu durdurma iþlemi
        if (state == "Stop")
        {
            ProcessPanel[0].SetActive(true);
        }
        else if (state == "Resume")
        {
            ProcessPanel[0].SetActive(false);
            Time.timeScale = 1; //oyunu devam ettirme
        }
        else if(state =="Replay")
        {
           //Sahne yeniden yüklenecek
           SceneManager.LoadScene(_Scene.buildIndex);
            Time.timeScale = 1;
        }
        else if (state == "MainMenu")
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
    }
    public void Settings(string state)
    {
        if(state == "Set")
        {
            ProcessPanel[1].SetActive(true);
            Time.timeScale = 0;
        }
        else if( state == "Exit")
        {
            ProcessPanel[1].SetActive(false);
            Time.timeScale=1;
        } 
    }
    public void SetSound()
    {
        _MemoryManager.SaveData_Float("GameSound", GameSoundSetting.value);
        Sounds[0].volume = GameSoundSetting.value;
    }
    public void NextLevel()
    {
        
        StartCoroutine(LoadAsync(_Scene.buildIndex + 1));
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
}
