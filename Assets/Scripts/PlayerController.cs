using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    //public Transform movePoint;
    public LayerMask stopsPlayerMovement;
    public PlayerInputActions playerInputActions;
    [SerializeField]
    private float collisionRadius;
    private Rigidbody _rb;
    private InputAction _move;
    private Vector2 _moveVector;

    void OnEnable() {
        
        _move = playerInputActions.Player.Move;
        _move.Enable();
    }

    void OnDisable() {
        _move.Disable();
    }

    void Awake() {
        playerInputActions = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //moveInput = GetComponent<PlayerInput>().actions["move"];
        //movePoint.parent = null;
    }

    // FixedUpdate is called once every 0.02 seconds
    void FixedUpdate() {
        _rb.linearVelocity = new Vector3(_moveVector.x, 0f, _moveVector.y) * moveSpeed;
    }

    // FixedUpdate is called once every 0.02 seconds
    void Update()
    {
        //Read player input
        _moveVector = _move.ReadValue<Vector2>();
        //if(Physics.OverlapSphere(this.transform.position + new Vector3(moveVector.x, 0f, moveVector.y), collisionRadius, stopsPlayerMovement).Length == 0) {
            //this.transform.Translate(new Vector3(_moveVector.x, 0f, _moveVector.y) * moveSpeed * Time.deltaTime);
            //this.transform.position += new Vector3(moveVector.x * moveSpeed * Time.deltaTime, 0f, moveVector.y * moveSpeed * Time.deltaTime);
        //}

        
        
    

        /*
        //Makes player move from current position to a new one at a speed determined by moveSpeed
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //If player is already (or close enough to) the new position, accept new input
        if(Vector3.Distance(transform.position, movePoint.position) <= 0.05f) {

            //Read player input
            Vector2 movePlayer = moveInput.ReadValue<Vector2>();

            //Proccess Horizontal Movement
            if(Math.Abs(movePlayer.x) > joystickDeadzoneValue) {
                
                if(movePlayer.x < 0) movePlayer.x = -1f;
                else if(movePlayer.y > 0) movePlayer.y = 1f;

                //Checks whether the player would be moving into an obstacle
                if(Physics.OverlapSphere(movePoint.position + new Vector3(movePlayer.x, 0f, 0f), 0.5f, stopsPlayerMovement).Length == 0) {
                    movePoint.position += new Vector3(movePlayer.x, 0f, 0f);
                }
            }

            //Process Vertical Movement, accounting for deadzone
            if(Math.Abs(movePlayer.y) > joystickDeadzoneValue) {


                if(movePlayer.y < 0) movePlayer.y = -1f;
                else if(movePlayer.y > 0) movePlayer.y = 1f;              
                
                //Checks whether the player would be moving into an obstacle
                if(Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, movePlayer.y), 0.5f, stopsPlayerMovement).Length == 0) {
                       movePoint.position += new Vector3(0f, 0f , movePlayer.y);
                }                
            }            

            /*
            //Gets player horizontal inputs and creates a new position for the movePoint
            if(Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f) {

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
        */
    }
}

