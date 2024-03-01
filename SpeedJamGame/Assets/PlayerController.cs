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
    [SerializeField] GameObject targetGameObj;
    [SerializeField] float radius;

    Rigidbody2D myRigidbody2D;
    SpriteRenderer mySpriteRenderer;
    Vector2 target;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        canSwing = true;
        isGrounded = true;
        target = new Vector2(targetGameObj.transform.position.x, targetGameObj.transform.position.y);
    }

    void Update()
    {
        isGrounded = myRigidbody2D.IsTouchingLayers(groundLayer);
        canSwing = inRange(target);

        movePlayer();

        if(Input.GetButtonDown("Jump") && canSwing){
            Swing();
        }
    }

    private void Jump(){
        isGrounded = false;
        myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpForce);
    }



    private void movePlayer(){
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical"); 


        if(vert > 0 && isGrounded){
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

    private void Swing(){
        Debug.Log("test");
    }

    private bool inRange(Vector2 target){
        Vector2 playerPos = new Vector2(myRigidbody2D.transform.position.x, myRigidbody2D.transform.position.y);
        return radius >= Vector2.Distance(playerPos, target);
    }
}
