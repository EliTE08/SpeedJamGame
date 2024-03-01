using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask groundLayer;

    float vert, horiz;

    [SerializeField] bool canSwing;
    [SerializeField] bool isGrounded;

    Rigidbody2D myRigidbody2D;
    SpriteRenderer mySpriteRenderer;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        canSwing = true;
        isGrounded = true;
    }

    void Update()
    {
        isGrounded = myRigidbody2D.IsTouchingLayers(groundLayer);

        movePlayer();

        if(Input.GetButtonDown("Jump") && canSwing){
            StartCoroutine(Swing());
        }
    }

    private void Jump(){
        isGrounded = false;
        myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpForce);
    }



    private void movePlayer(){
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical"); 


        if(vert > 0 && isGrounded && canSwing){
            Jump();
        }
        if(horiz < 0){
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(horiz >= 0){
            transform.localScale = new Vector3(1, 1, 1);
        }

        myRigidbody2D.velocity = new Vector2(horiz * moveSpeed, myRigidbody2D.velocity.y); 
    }

    private IEnumerator Swing(){
        //Put Swing logic in here
        yield return null;
    }
}
