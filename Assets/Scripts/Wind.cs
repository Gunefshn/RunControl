using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Karakterlerin obje ile etkile�imi s�rd��� m�ddet�e uygulamak gerekiyor.
        if (other.CompareTag("SubCharacter"))
        {
            // Sadece alt karakterlere bu g�c�n uygulanmas�� istiyoruz.
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-5, 0 - 0),ForceMode.Impulse);
        }

    }
}
