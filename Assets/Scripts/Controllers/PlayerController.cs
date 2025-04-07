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

    private PlayerInput _playerInput;
    
    //private float collisionRadius;
    private Rigidbody _rb;
    private InputAction _move;
    private Vector2 _moveVector;
    private Animator _animator;
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

    void Awake() {
        playerInputActions = new PlayerInputActions();
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // FixedUpdate is called once every 0.02 seconds
    void FixedUpdate() {

        transform.position += new Vector3(_moveVector.x, 0f, _moveVector.y) * moveSpeed * Time.deltaTime;

    }

    // FixedUpdate is called once every 0.02 seconds
    void Update()
    {

        if(_playerInput.currentActionMap.name == "Player") {
            //Read player input
            _moveVector = _move.ReadValue<Vector2>();
        }
        else {
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

