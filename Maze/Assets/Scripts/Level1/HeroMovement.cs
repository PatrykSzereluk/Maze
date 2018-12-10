using System.Collections;
using UnityEngine;

public class HeroMovement : MonoBehaviour {


    [HideInInspector]
    public bool HasKey;

    [Header("Checkers")]
    public Transform left;
    public Transform right;
    public Transform down;
    public Transform up;
    public LayerMask layerMaskWall;
    public float radious_wall_check = 0.1f;

    [Header("Sprites")]
    public Sprite left1;
    public Sprite right1;
    public Sprite down1;
    public Sprite Up1;

    private bool canWalkLeft;
    private bool canWalkRight;
    private bool canWalkUp;
    private bool canWalkDown;

    void Start () {
       
        HasKey = false;
        
    }
	
	void Update () {
        canWalkUp = Physics2D.OverlapCircle(up.position, radious_wall_check, layerMaskWall);
        canWalkDown = Physics2D.OverlapCircle(down.position, radious_wall_check, layerMaskWall);
        canWalkRight = Physics2D.OverlapCircle(right.position, radious_wall_check, layerMaskWall);
        canWalkLeft = Physics2D.OverlapCircle(left.position, radious_wall_check, layerMaskWall);


        if (Input.GetKeyDown(KeyCode.W) && !canWalkUp)
            {

                transform.Translate(0, 1, 0);
                GetComponent<SpriteRenderer>().sprite = Up1;
            }
            else if(Input.GetKeyDown(KeyCode.S) && !canWalkDown)
            {
    
                transform.Translate(0, -1, 0);
                GetComponent<SpriteRenderer>().sprite = down1;
            }
            else if (Input.GetKeyDown(KeyCode.D) && !canWalkRight)
            {

                transform.Translate(1, 0, 0);
                GetComponent<SpriteRenderer>().sprite = right1;
            }
            else if (Input.GetKeyDown(KeyCode.A) && !canWalkLeft)
            {

            transform.Translate(-1, 0, 0);
            GetComponent<SpriteRenderer>().sprite = left1;
            }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Key"))
        {
            HasKey = true;
            Destroy(collision.gameObject);
        }
    }


}
