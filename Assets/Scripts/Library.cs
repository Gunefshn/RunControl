using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gunef
{
    public class Mathematical_Operations
    {
        public void Multiply(int incomingNumber, List<GameObject> Characters, Transform position, List<GameObject> BornEffects)
        {
            int Count = (GameManager.CharacterCount * incomingNumber) - GameManager.CharacterCount;
            //                  10                * 4            - 10             =30
            //                  6                  *5             -6              =24
            int number = 0;

            foreach (var item in Characters)
            {
                if (number < Count)
                {
                    if (!item.activeInHierarchy)
                    {
                        foreach (var item2 in BornEffects)
                        {
                            if (!item2.activeInHierarchy)
                            {


                                item2.SetActive(true);
                                item2.transform.position = position.position; // Effect karakter olu�turma pozisyonu ile ayn� yerde olu�mal�
                                item2.GetComponent<ParticleSystem>().Play();
                                item2.GetComponent<AudioSource>().Play();
                                break;
                            }
                        }

                        item.transform.position = position.position; //d��ardan gelen pozisyon ne ise o pozisyonda karakter olu�turmas� sa�lanacak
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
            GameManager.CharacterCount *= incomingNumber;

        }
        public void Add(int incomingNumber, List<GameObject> Characters, Transform position, List<GameObject> BornEffects)
        {
            int number2 = 0;

            foreach (var item in Characters)
            {
                if (number2 < incomingNumber)
                {
                    if (!item.activeInHierarchy)
                    {
                        foreach (var item2 in BornEffects)
                        {
                            if (!item2.activeInHierarchy)
                            {


                                item2.SetActive(true);
                                item2.transform.position = position.position; // Effect karakter olu�turma pozisyonu ile ayn� yerde olu�mal�
                                item2.GetComponent<ParticleSystem>().Play();
                                item2.GetComponent<AudioSource>().Play();
                                break;
                            }
                        }

                        item.transform.position = position.position;
                        item.SetActive(true);
                        number2++;

                    }

                }
                else
                {
                    number2 = 0;
                    break;
                }


            }
            GameManager.CharacterCount += incomingNumber;

        }
        public void Subtract(int incomingNumber, List<GameObject> Characters, List<GameObject> DyingEffects)
        {
            if (GameManager.CharacterCount < incomingNumber)
            {   //E�er anl�k karakter say�m�z gelen say�m�zdan k���k ise
                foreach (var item in Characters)
                {
                    foreach (var item2 in DyingEffects)
                    {
                        if (!item2.activeInHierarchy)
                        {
                            Vector3 newPos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);//efekt olu�turma s�ras�nda y ekseninde olu�an g�r�nt�den dolay�

                            item2.SetActive(true);
                            item2.transform.position = newPos;
                            item2.GetComponent<ParticleSystem>().Play();
                            item2.GetComponent<AudioSource>().Play();

                            break;
                        }
                    }
                    item.transform.position = Vector3.zero; //pozisyon s�f�rlama
                    item.SetActive(false);
                }
                GameManager.CharacterCount = 1;
            }
            else
            {

                int number3 = 0;

                foreach (var item in Characters)
                {
                    if (number3 != incomingNumber)
                    {
                        if (item.activeInHierarchy) //Aktif olan karakterler aras�ndan bulunmas� gerekiyor
                        {
                            foreach (var item2 in DyingEffects)
                            {
                                if (!item2.activeInHierarchy)
                                {
                                    Vector3 newPos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);//efekt olu�turma s�ras�nda y ekseninde olu�an g�r�nt�den dolay�

                                    item2.SetActive(true);
                                    item2.transform.position = newPos;
                                    item2.GetComponent<ParticleSystem>().Play();
                                    item2.GetComponent<AudioSource>().Play();
                                    break;
                                }
                            }

                            item.transform.position = Vector3.zero;
                            item.SetActive(false);
                            number3++;

                        }
                    }
                    else
                    {
                        number3 = 0;
                        break;
                    }
                }
                GameManager.CharacterCount -= incomingNumber;
            }

        }
        public void Divide(int incomingNumber, List<GameObject> Characters, List<GameObject> DyingEffects)
        {
            if (GameManager.CharacterCount <= incomingNumber)
            {
                foreach (var item in Characters)
                {
                    foreach (var item2 in DyingEffects)
                    {
                        if (!item2.activeInHierarchy)
                        {
                            Vector3 newPos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);//efekt olu�turma s�ras�nda y ekseninde olu�an g�r�nt�den dolay�

                            item2.SetActive(true);
                            item2.transform.position = newPos;
                            item2.GetComponent<ParticleSystem>().Play();
                            item2.GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                    item.transform.position = Vector3.zero;
                    item.SetActive(false);
                }
                GameManager.CharacterCount = 1;
            }
            else
            {
                int division = GameManager.CharacterCount / incomingNumber;

                int number3 = 0;

                foreach (var item in Characters)
                {
                    if (number3 != division)
                    {
                        if (item.activeInHierarchy) //Aktif olan karakterler aras�ndan bulunmas� gerekiyor
                        {

                            foreach (var item2 in DyingEffects)
                            {
                                if (!item2.activeInHierarchy)
                                {
                                    Vector3 newPos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);//efekt olu�turma s�ras�nda y ekseninde olu�an g�r�nt�den dolay�

                                    item2.SetActive(true);
                                    item2.transform.position = newPos;
                                    item2.GetComponent<ParticleSystem>().Play();
                                    item2.GetComponent<AudioSource>().Play();
                                    break;
                                }
                            }
                            item.transform.position = Vector3.zero;
                            item.SetActive(false);
                            number3++;

                        }
                    }
                    else
                    {
                        number3 = 0;
                        break;
                    }
                }
                //Modlar oyundan dolay� 2 ve 3 e b�l�nme olarak hesapland�.
                if (GameManager.CharacterCount % incomingNumber == 0) //Tam b�l�nme durumu
                {
                    GameManager.CharacterCount /= incomingNumber;
                }
                else if (GameManager.CharacterCount % incomingNumber == 1)
                { //Kalan�n 1 olmas� durumu
                    GameManager.CharacterCount /= incomingNumber;
                    GameManager.CharacterCount++;
                }
                else if (GameManager.CharacterCount % incomingNumber == 2)//Kalan�n 2 olmas� durumu
                {
                    GameManager.CharacterCount /= incomingNumber;
                    GameManager.CharacterCount += 2;
                }
            }

        }


    }

    public class MemoryManager
    {
        public void SaveData_String(string Key, string value)
        {
            PlayerPrefs.SetString(Key, value);
            PlayerPrefs.Save();
        }
        public void SaveData_Int(string Key, int value)
        {
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
        }
        public void SaveData_Float(string Key, float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
        }

        public string ReadData_String(string Key)
        {
            return PlayerPrefs.GetString(Key);
        }
        public int ReadData_Int(string Key)
        {
            return PlayerPrefs.GetInt(Key);
        }
        public float ReadData_Float(string Key)
        {
            return PlayerPrefs.GetFloat(Key);
        }

        public void CheckAndIdentify()
        {
            if (!PlayerPrefs.HasKey("LastLevel"))
            {
                //sistemde b�yle bir anahtar yok ise oyun ilk defa a��lm��t�r.
                PlayerPrefs.SetInt("LastLevel", 5); // 5 verilme sebebi ilk level id 5 te 
                PlayerPrefs.SetInt("Score", 100); // varsay�lan puan� 110 dan ba�l�yor.
                PlayerPrefs.SetInt("ActiveHat" ,-1);
                PlayerPrefs.SetInt("ActiveStick", -1);
                PlayerPrefs.SetInt("ActiveMaterial", -1);
                PlayerPrefs.SetFloat("MenuSound", 1);
                PlayerPrefs.SetFloat("MenuFX", 1);
                PlayerPrefs.SetFloat("GameSound", 1);
                PlayerPrefs.SetString("Language","TR");

            }
        }
    }

    [Serializable]  // class � liste olarak kullanabilmek i�in
    public class ItemInformation
    {
        public int GrupIndex;
        public int Item_Index;
        public string Item_Name;
        public int Score;
        public bool Purchase_Status;
    }

    public class DataManager
    {
        List<ItemInformation> _ItemInformation2;
        public void Save(List<ItemInformation> _ItemInformation)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenWrite(Application.persistentDataPath + "/ItemData.gd");
            bf.Serialize(file, _ItemInformation);
            file.Close();
        }

        public void Load()
        {
            // veri okumadan �nce dosya varl��� kontrol edilmeli
            if (File.Exists(Application.persistentDataPath + "/ItemData.gd"))
            {
                //Dosya var ise
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/ItemData.gd", FileMode.Open);
                _ItemInformation2 = (List<ItemInformation>)bf.Deserialize(file);
                file.Close();
            }
        }

        public List<ItemInformation> TransferList()
        {
            return _ItemInformation2;
        }

        public void FirstFileCreate(List<ItemInformation> _ItemInformation, List<LanguageDataMainObject> _LanguageDatas)
        {
            if (!File.Exists(Application.persistentDataPath + "/ItemData.gd")) // Kod blo�unun sadece bir kere �al��mas� i�in 
            {

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/ItemData.gd");
                bf.Serialize(file, _ItemInformation);
                file.Close();
            }
            if (!File.Exists(Application.persistentDataPath + "/LanguageData.gd")) // Kod blo�unun sadece bir kere �al��mas� i�in 
            {

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/LanguageData.gd");
                bf.Serialize(file, _LanguageDatas);
                file.Close();
            }
        }

        List<LanguageDataMainObject> _LanguageDatas2;
        public void Language_Load()
        {
            // veri okumadan �nce dosya varl��� kontrol edilmeli
            if (File.Exists(Application.persistentDataPath + "/LanguageData.gd"))
            {
                //Dosya var ise
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/LanguageData.gd", FileMode.Open);
                _LanguageDatas2 = (List<LanguageDataMainObject>)bf.Deserialize(file);
                file.Close();
            }
        }
        public List<LanguageDataMainObject> LanguageDataTransferList()
        {
            return _LanguageDatas2;
        }

    }

    // Language Management
    [Serializable]  
    public class LanguageDataMainObject
    {
        public List<LanguageData_TR> LanguageData_TR = new List<LanguageData_TR>();
        public List<LanguageData_TR> LanguageData_EN = new List<LanguageData_TR>();
    }
    [Serializable]
    public class LanguageData_TR
    {
        public string Text;
    }

}

