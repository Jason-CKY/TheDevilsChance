using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed = 10;
    public float turningSpeed = 60;
    

    Animator anim;
    int jumpHash = Animator.StringToHash("Jump");

    void Start() {
        anim = GetComponent<Animator>();
    }
 
    void Update() {
        float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);
         
        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger(jumpHash);
            
        }

        float move = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.W))
        {
            anim.SetFloat("Speed", move);
            
        }
    }


}
