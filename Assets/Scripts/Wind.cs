using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Karakterlerin obje ile etkileþimi sürdüðü müddetçe uygulamak gerekiyor.
        if (other.CompareTag("SubCharacter"))
        {
            // Sadece alt karakterlere bu gücün uygulanmasýý istiyoruz.
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-5, 0 - 0),ForceMode.Impulse);
        }

    }
}
