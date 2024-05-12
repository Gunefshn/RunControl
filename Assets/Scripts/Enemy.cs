using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject AttackTarget;
    public NavMeshAgent _navMesh;
    public Animator _Animator;
    public GameManager _GameManager;
    bool IsAttackStarted;


    public void TriggerAnimation()
    {
        _Animator.SetBool("Fight", true);
        IsAttackStarted = true;
    }

    void LateUpdate()
    {
        if (IsAttackStarted)
        {
            _navMesh.SetDestination(AttackTarget.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SubCharacter"))
        {
            Vector3 newPos = new Vector3(transform.position.x, .23f, transform.position.z);
            _GameManager.CreateDyingEffect(newPos, false, true);
            gameObject.SetActive(false);
        }
    }
}
