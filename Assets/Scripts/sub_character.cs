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
        _NavMesh.SetDestination(Target.transform.position); // AI �n var�� noktas�n� takip etmesi i�n 
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
            // transform.position = Vector3.zero; // baz� durumlarda yeniden olu�turma s�ras�nda bir hata almamak i�in pozisyon yok etmeden �nce s�f�rlan�r.
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
        { //Alt karakter bo� karaktere �arpt��� anda karakter listesine al�nacak ve anl�k karakter say�s� artt�r�lacak �arp��ma sonunda da tag � de�i�meli
            _GameManager.Characters.Add(other.gameObject);
            

        }
    }
}
