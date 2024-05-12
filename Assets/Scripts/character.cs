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
        _Slider.maxValue = difference; // Oyun ba�lar ba�lamaz slider max de�eri ba�lang�� mesafesine e�itlenir.
    }

    //Mouse hareketlerini rahat alg�layabilmesi i�in,gecikmeleri engellemek i�in
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
            //karakter sona gelmedi�i m�ddet�e aradaki mesafe fark� hesaplan�yor olmas� gerekiyor bu y�zden else k�sm�nda
            if (EndGame)//Oyun sonuna gelinmi� ise
            {
                transform.position = Vector3.Lerp(transform.position, LastPosition.transform.position, .015f);
                if (_Slider.value != 0)
                {
                    _Slider.value -= .005f;
                }
            }

            else
            {
                float difference = Vector3.Distance(transform.position, WayPoint.transform.position); //karakter pozisyonum ile son tetikleyicim aras�ndaki fark� hesapla
                                                                                                      // Debug.Log(difference);
                _Slider.value = difference;

                if (Input.GetKey(KeyCode.Mouse0)) // Mouse sol tu�una t�klan�ld���nda
                {   //Mouse un ekran�n sol/sa� konumunda olup olmad���n� sorgulamak i�in
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
        /* Art�k sadece karakter tag� ve name ile i�lem yap�laca�� i�in bu �ekilde yakalamaya gerek kalmad�
        if (other.name == "x2" || other.name== "+3" || other.name == "-4" || other.name == "/2")
        {
            _GameManager.CharacterManager(other.name,other.transform);
            //Debug.Log("Enter");
        }
        */


        if (other.CompareTag("Add") || other.CompareTag("Multiply") || other.CompareTag("Subtract") || other.CompareTag("Divide"))
        {
            int number = int.Parse(other.name); //objenin ismini say�ya �evirerek fonksiyonumuza g�nderdik
            _GameManager.CharacterManager(other.tag, number, other.transform);

        }
        else if (other.CompareTag("EndTrigger"))
        {
            //karakter sava� alan�na geldi�i anda kontrol�n b�rak�lmas� ve kameran�n a��s�n�n de�i�mesi gerekiyor
            _Camera.EndGame = true;
            _GameManager.TriggerEnemy();
            EndGame = true;
        }
        else if (other.CompareTag("EmptyCharacter"))
        { //Ana karakter bo� karaktere �arpt��� anda karakter listesine al�nacak ve anl�k karakter say�s� artt�r�lacak �arp��ma sonunda da tag � de�i�meli
            _GameManager.Characters.Add(other.gameObject);
         
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Karakter engellere temas etti�i zaman
        if (collision.gameObject.CompareTag("Stick") || collision.gameObject.CompareTag("PinBox") || collision.gameObject.CompareTag("FanPin"))
        {
            if(transform.position.x > 0) //karakter pozisyonu 0dan b�y�k ise
            {
                transform.position = new Vector3(transform.position.x - .2f, transform.position.y, transform.position.z); //Karakter herhangi bir yerde tak�l� kald��� zaman x posizyonunda -.2f ile pozisyon g�ncelle
            }
            else
            {
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z); //Karakter herhangi bir yerde tak�l� kald��� zaman x posizyonunda +.2f ile pozisyon g�ncelle
            }
            
        }
    }
}