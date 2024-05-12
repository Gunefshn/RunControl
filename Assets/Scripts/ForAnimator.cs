using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAnimator : MonoBehaviour
{
    public Animator _Animator;
    public void Passive()
    {
        _Animator.SetBool("ok", false);
    }
}
