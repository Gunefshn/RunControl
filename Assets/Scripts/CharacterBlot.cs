using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlot : MonoBehaviour
{
    IEnumerator Start()
    {
        //Hammer efektinde 5 saniye sonra efektin tekrar kaybolmsý içn
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
    
}
