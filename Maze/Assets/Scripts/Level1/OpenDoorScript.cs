using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour {

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hero"))
        {
           if( collision.GetComponent<HeroMovement>().HasKey == true )
            {
                anim.SetBool("Key", true);
                EndGameManager.EndGame();
                Debug.Log("Otwarte");
            }
           else
            {
                Debug.Log("Zamkniete");
            }
        }
    }


}
