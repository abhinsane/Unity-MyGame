using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public Rigidbody2D theRB;
  
  public float moveSpeed;

  public Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")) * moveSpeed;
        
        myAnim.SetFloat("moveX",theRB.velocity.x);
        myAnim.SetFloat("moveY",theRB.velocity.y);
    }
}