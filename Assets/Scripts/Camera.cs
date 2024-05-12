using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // Takip edece�i hedefi belirtmek i�in
    public Vector3 target_offset; //hangi a��dan takip edilece�ine dair bir pozisyon belirlemek i�in
    public bool EndGame; // Oyun sonuna gelindi mi
    public GameObject LastPosition; //Kameran�n oyun sonunda gidece�i posizyon
    public static Camera current;

    void Start()
    {
        target_offset = transform.position - target.position; // Kamera pozisyonu ile hedef pozisyon aras�ndaki mesafe
    }

    //karakter hareketleri update metodu i�erisinde yer alaca�� ve kamera hareketlerinin daha yumu�ak olmas� i�in lateupdate kullanmak daha mant�kl�
    private void LateUpdate()
    {
        if (!EndGame) //karakter takibi devam edecek
        {
            transform.position = Vector3.Lerp(transform.position, target.position + target_offset, .125f); // kamera posziyonundan,target pozisyonuna+aradaki mesafe,belirtilen h�zda 
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, LastPosition.transform.position, .015f);
        }



    }
}
