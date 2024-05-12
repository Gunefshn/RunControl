using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sub_character : MonoBehaviour
{

    NavMeshAgent _NavMesh;
    public GameManager _GameManager;
    public GameObject Target;


    void Start()
    {
        _NavMesh = GetComponent<NavMeshAgent>();

    }

    private void LateUpdate()
    {
        _NavMesh.SetDestination(Target.transform.position); // AI ýn varýþ noktasýný takip etmesi içn 
    }

    Vector3 GetPosition()
    {

        return new Vector3(transform.position.x, .23f, transform.position.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PinBox"))
        {
            _GameManager.CreateDyingEffect(GetPosition());
            // transform.position = Vector3.zero; // bazý durumlarda yeniden oluþturma sýrasýnda bir hata almamak için pozisyon yok etmeden önce sýfýrlanýr.
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Saw"))
        {
            _GameManager.CreateDyingEffect(GetPosition());
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("FanPin"))
        {
            _GameManager.CreateDyingEffect(GetPosition());
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Hammer"))
        {
            _GameManager.CreateDyingEffect(GetPosition(), true);
            // GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CreateCharacterBlot(newPos);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            _GameManager.CreateDyingEffect(GetPosition(), false, false);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("EmptyCharacter"))
        { //Alt karakter boþ karaktere çarptýðý anda karakter listesine alýnacak ve anlýk karakter sayýsý arttýrýlacak çarpýþma sonunda da tag ý deðiþmeli
            _GameManager.Characters.Add(other.gameObject);
            

        }
    }
}
