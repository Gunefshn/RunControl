using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : MonoBehaviour
{
    public GameManager _GameManager;
    public Camera _Camera;
    public bool EndGame; //Sona gelindi mi
    public GameObject LastPosition;
    public Slider _Slider;
    public GameObject WayPoint;


    private void Start()
    {
        float difference = Vector3.Distance(transform.position, WayPoint.transform.position);
        _Slider.maxValue = difference; // Oyun baþlar baþlamaz slider max deðeri baþlangýç mesafesine eþitlenir.
    }

    //Mouse hareketlerini rahat algýlayabilmesi için,gecikmeleri engellemek için
    private void FixedUpdate()
    {
        if (!EndGame)//Oyun sonuna gelinmediyse devam et
        {
            transform.Translate(Vector3.forward * .5f * Time.deltaTime);

        }

    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            //karakter sona gelmediði müddetçe aradaki mesafe farký hesaplanýyor olmasý gerekiyor bu yüzden else kýsmýnda
            if (EndGame)//Oyun sonuna gelinmiþ ise
            {
                transform.position = Vector3.Lerp(transform.position, LastPosition.transform.position, .015f);
                if (_Slider.value != 0)
                {
                    _Slider.value -= .005f;
                }
            }

            else
            {
                float difference = Vector3.Distance(transform.position, WayPoint.transform.position); //karakter pozisyonum ile son tetikleyicim arasýndaki farký hesapla
                                                                                                      // Debug.Log(difference);
                _Slider.value = difference;

                if (Input.GetKey(KeyCode.Mouse0)) // Mouse sol tuþuna týklanýldýðýnda
                {   //Mouse un ekranýn sol/sað konumunda olup olmadýðýný sorgulamak için
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z), .3f);
                    }
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z), .3f);
                    }
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        /* Artýk sadece karakter tagý ve name ile iþlem yapýlacaðý için bu þekilde yakalamaya gerek kalmadý
        if (other.name == "x2" || other.name== "+3" || other.name == "-4" || other.name == "/2")
        {
            _GameManager.CharacterManager(other.name,other.transform);
            //Debug.Log("Enter");
        }
        */


        if (other.CompareTag("Add") || other.CompareTag("Multiply") || other.CompareTag("Subtract") || other.CompareTag("Divide"))
        {
            int number = int.Parse(other.name); //objenin ismini sayýya çevirerek fonksiyonumuza gönderdik
            _GameManager.CharacterManager(other.tag, number, other.transform);

        }
        else if (other.CompareTag("EndTrigger"))
        {
            //karakter savaþ alanýna geldiði anda kontrolün býrakýlmasý ve kameranýn açýsýnýn deðiþmesi gerekiyor
            _Camera.EndGame = true;
            _GameManager.TriggerEnemy();
            EndGame = true;
        }
        else if (other.CompareTag("EmptyCharacter"))
        { //Ana karakter boþ karaktere çarptýðý anda karakter listesine alýnacak ve anlýk karakter sayýsý arttýrýlacak çarpýþma sonunda da tag ý deðiþmeli
            _GameManager.Characters.Add(other.gameObject);
         
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Karakter engellere temas ettiði zaman
        if (collision.gameObject.CompareTag("Stick") || collision.gameObject.CompareTag("PinBox") || collision.gameObject.CompareTag("FanPin"))
        {
            if(transform.position.x > 0) //karakter pozisyonu 0dan büyük ise
            {
                transform.position = new Vector3(transform.position.x - .2f, transform.position.y, transform.position.z); //Karakter herhangi bir yerde takýlý kaldýðý zaman x posizyonunda -.2f ile pozisyon güncelle
            }
            else
            {
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z); //Karakter herhangi bir yerde takýlý kaldýðý zaman x posizyonunda +.2f ile pozisyon güncelle
            }
            
        }
    }
}