using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // Takip edeceði hedefi belirtmek için
    public Vector3 target_offset; //hangi açýdan takip edileceðine dair bir pozisyon belirlemek için
    public bool EndGame; // Oyun sonuna gelindi mi
    public GameObject LastPosition; //Kameranýn oyun sonunda gideceði posizyon
    public static Camera current;

    void Start()
    {
        target_offset = transform.position - target.position; // Kamera pozisyonu ile hedef pozisyon arasýndaki mesafe
    }

    //karakter hareketleri update metodu içerisinde yer alacaðý ve kamera hareketlerinin daha yumuþak olmasý için lateupdate kullanmak daha mantýklý
    private void LateUpdate()
    {
        if (!EndGame) //karakter takibi devam edecek
        {
            transform.position = Vector3.Lerp(transform.position, target.position + target_offset, .125f); // kamera posziyonundan,target pozisyonuna+aradaki mesafe,belirtilen hýzda 
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, LastPosition.transform.position, .015f);
        }



    }
}
