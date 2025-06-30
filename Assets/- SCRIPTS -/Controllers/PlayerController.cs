using System;
using Enums;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    //public Transform movePoint;
    //public LayerMask stopsPlayerMovement;
    public PlayerInputActions playerInputActions;

    [Header("Camera Relative Movement")]
    public bool movingRelativeToCamera;
    private Vector3 cameraForwardVector;
    private Vector3 cameraRightVector;


    private PlayerInput _playerInput;
    
    //private float collisionRadius;
    private Rigidbody _rb;
    private InputAction _move;
    private Vector2 _moveVector;
    private Animator _animator;

    [Header("Player Sprites")]
    [SerializeField]
    private SpriteRenderer headRenderer;
    [SerializeField]
    private SpriteRenderer torsoRenderer;
    [SerializeField]
    private SpriteRenderer legsRenderer;

    //public bool usingRBVelocity;

    void OnEnable() {
        
        _move = playerInputActions.Player.Move;
        _move.Enable();
    }

    void OnDisable() {
        _move.Disable();
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        if (movingRelativeToCamera)
        {
            //Calculate forward and left/right based on camera position

            cameraForwardVector = Camera.main.transform.forward;
            cameraRightVector = Camera.main.transform.right;

            cameraForwardVector.y = 0;
            cameraRightVector.y = 0;

            cameraForwardVector = cameraForwardVector.normalized;
            cameraRightVector = cameraRightVector.normalized;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // FixedUpdate is called once every 0.02 seconds
    void FixedUpdate() {

        Vector3 movement = new Vector3(_moveVector.x, 0f, _moveVector.y) * moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + movement);

    }

    // FixedUpdate is called once every 0.02 seconds
    void Update()
    {

        //if player is currently in control of the character, make it move
        if (_playerInput.currentActionMap.name == "Player")
        {

            if (movingRelativeToCamera)
            {
                Vector2 input = _move.ReadValue<Vector2>();

                // Multiply player input by the forward and left/right camera vectors
                Vector3 move = cameraRightVector * input.x + cameraForwardVector * input.y;

                _moveVector = new Vector2(move.x, move.z);
            }

            else
            {
                //Read player input
                _moveVector = _move.ReadValue<Vector2>();
            }
        }

        //if player is NOT currently in control of the character, make it stop
        else
        {
            _moveVector = new Vector2(0f, 0f);
        }
        
        //Player animations
        if (_moveVector.x != 0f || _moveVector.y != 0f) {
            _animator.SetBool("Moving", true);
        }
        else {
            _animator.SetBool("Moving", false);
        }
    }

    public void updatePlayerSprite(PickupType type, Sprite partSprite) {
        switch (type) {
            case PickupType.head:
                headRenderer.sprite = partSprite;
                break;
            case PickupType.torso:
                torsoRenderer.sprite = partSprite;
                break;
            case PickupType.legs:
                legsRenderer.sprite = partSprite;
                break;
        }
    }
}

