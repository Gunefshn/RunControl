using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public Animator _animator;
    public float WaitingTime;
    public BoxCollider _Wind;

    public void AnimationState(string state)
    {
        if(state == "true")
        {
            _animator.SetBool("Run", true);
            _Wind.enabled = true;
        }
        else
        {
            _animator.SetBool("Run", false);
            StartCoroutine(TriggerAnimation());
            _Wind.enabled = false;        }
       
    }
    IEnumerator TriggerAnimation()
    {
        yield return new WaitForSeconds(WaitingTime);
        AnimationState("true");
    }
}
