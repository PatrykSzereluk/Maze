using UnityEngine;
using UnityEngine.UI;

public class FloorBehaviour : MonoBehaviour {

    private bool Visited;
    private Animator anim;
    

    void Start () {

        Visited = false;
        anim = GetComponent<Animator>();

	}
	
    private void OnTriggerExit2D(Collider2D collision)
    {
            Visited = true;
            anim.SetBool("Visited", Visited);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (Visited == true)
        {
            Debug.Log("GameOver");
            EndGameManager.EndGame();
        }
    }
}
