using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopsPlayerMovement;

    private PlayerInput playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {

        //Makes player move from current position to a new one at a speed determined by moveSpeed
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //If player is already (or close enough to) the new position, accept new input
        if(Vector3.Distance(transform.position, movePoint.position) <= 0.05f) {

            
            //Read Joystick inputs
            Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

            if(input.x > 0.5f || input.x < -0.5f) Mathf.Round(input.x); else {input.x = 0;}
            if(input.y > 0.5f || input.y < -0.5f) Mathf.Round(input.y); else {input.y = 0;}

            Debug.Log(input);

            //Gets player horizontal inputs and creates a new position for the movePoint
            if(Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f || Math.Abs(input.x) == 1f) {

                Debug.Log("got here");
                //Checks whether the player would be moving into an obstacle
                if(Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.5f, stopsPlayerMovement).Length == 0) {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }


            //Gets player vertical inputs and creates a new position for the movePoint
            if(Math.Abs(Input.GetAxisRaw("Vertical")) == 1f) {
                //Checks whether the player would be moving into an obstacle
                if(Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.5f, stopsPlayerMovement).Length == 0) {
                       movePoint.position += new Vector3(0f, 0f , Input.GetAxisRaw("Vertical"));
                }
            }
        }
    }
}

