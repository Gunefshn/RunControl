using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class EmptyCharacter : MonoBehaviour
{
    public SkinnedMeshRenderer _Renderer;
    public Material ReplacedMaterial;
    public NavMeshAgent _NavMesh;
    public Animator _Animator;
    public GameObject Target;
    public GameManager _GameManager;
    bool Contact;






    private void LateUpdate()
    {
        if (Contact) // Eðer EmptyCharacter e temas var ise ana karakterin destination a göre konum alacak.
        {
            _NavMesh.SetDestination(Target.transform.position);
        }

    }
    Vector3 GetPosition()
    {

        return new Vector3(transform.position.x, .23f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SubCharacter") || other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("EmptyCharacter"))
            {
                // Karakterim veya alt karakterlerden biri çarptýðý anda material deðiþtir animasyonu tetsikle ve contact true yaparak destinatin ý hedef al.
                ChangeMaterialAndTriggerAnimation();
                Contact = true;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (other.CompareTag("PinBox"))
        {
            _GameManager.CreateDyingEffect(GetPosition());
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
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            _GameManager.CreateDyingEffect(GetPosition(), false, false);
            gameObject.SetActive(false);
        }

    }

    void ChangeMaterialAndTriggerAnimation()
    {
        Material[] mats = _Renderer.materials;
        mats[0] = ReplacedMaterial;
        _Renderer.materials = mats;
        _Animator.SetBool("Fight", true);

        gameObject.tag = "SubCharacter";
        GameManager.CharacterCount++;
        Debug.Log(GameManager.CharacterCount);
    }
}
