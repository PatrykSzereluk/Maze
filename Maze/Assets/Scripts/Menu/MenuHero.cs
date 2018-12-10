using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHero : MonoBehaviour {


    [Header("Sprites")]
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private bool block;


    private void Update()
    {
        if(!block)
        {
            StartCoroutine(WalkoAroundMenu());
        }
    }

    private void WalkoRight()
    {
        transform.Translate(1, 0, 0);
    }

    private void WalkoDown()
    {
        transform.Translate(0, -1, 0);
    }

    private void WalkLeft()
    {
        transform.Translate(-1, 0, 0);
    }

    private void WalkUp()
    {
        transform.Translate(0, 1, 0);
    }


    IEnumerator WalkoAroundMenu()
    {
        block = true;

        GetComponent<SpriteRenderer>().sprite = up;
        for (int i = 0; i < 4; i++)
        {
            WalkUp();
            yield return new WaitForSeconds(.4f);
        }
        
        GetComponent<SpriteRenderer>().sprite = left;

        for (int i = 0; i < 5; i++)
        {
            WalkLeft();
            yield return new WaitForSeconds(.4f);
        }
       
        GetComponent<SpriteRenderer>().sprite = down;

        for (int i = 0; i < 4; i++)
        {
            WalkoDown();
            yield return new WaitForSeconds(.4f);
        }
        GetComponent<SpriteRenderer>().sprite = right;

        for (int i = 0; i < 5; i++)
        {
            WalkoRight();
            yield return new WaitForSeconds(.4f);
        }

        block = false;
    }



}
