using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{
    private static GameObject instance;
    public AudioSource audio;
    private void Start()
    {   
        //objenin sahneler arasýnda devam edebilmesi için
       
        audio.volume = PlayerPrefs.GetFloat("MenuSound");
        DontDestroyOnLoad(gameObject); // objeyi kybetmemesi için

        if(instance == null)
        {
            instance = gameObject;   
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        audio.volume = PlayerPrefs.GetFloat("MenuSound");
    }
}
