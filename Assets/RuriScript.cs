using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // hey bro's, anything in double slashes are comments, they are not read by the computer, they are just for you to read
    // so you can understand me code good sirs, I need to practice coding for others to understand.
    
    private Transform _playerTransform; //this is a variable that stores the transform of the player
    private SpriteRenderer _spriteRenderer; //this is a variable that stores the sprite renderer of the player, what you see on the screen
    private AudioSource _audioSource;
    
    private InputSystem_Actions _inputActions; //this is a variable that stores the input actions of the player
    
    private InputAction _moveAction; //this is a variable that stores the move action of the player
    private InputAction _switchAction;  //unused for now
    
    public GameObject ottoPrefab; //this is a variable that stores the otto game object
    private GameObject _otto; //this is a variable that stores the otto game object, this is the one that will be used in the game
    public bool hasOtto = false; //this is a variable that stores if the player has otto or not
    //it's public because this is also checked when the game starts, so you can set it in the inspector
    
    public float movementSpeed = 2f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    
    public enum Direction { Up, Down, Left, Right }
    public static Direction CurrentDirection = Direction.Down;
    //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    //Awake Is Called before start
    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _playerTransform = this.transform;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteRenderer.sprite;
        _audioSource = this.GetComponent<AudioSource>();
        if (hasOtto == true)
        {
         addOtto();
        }

    }

    private void addOtto()
    {
        _otto = Instantiate(ottoPrefab, _playerTransform); //creates a new game object
        _otto.transform.localPosition = new Vector3(0, 0.43f, 0); //this is the position of the otto game object relative to the player
        _otto.transform.localRotation = Quaternion.identity; //Identity just means zero rotation essentially.
    }
    private void removeOtto()
    {
        Destroy(_otto); //destroys the otto game object
        _otto = null; //sets the otto game object to null, basically like when we instantiated it
    }

    private void changeSprite(String path)
    {
        _spriteRenderer.sprite = Resources.Load<Sprite>(path);
    }

    private void OnEnable()
    {
        _moveAction = _inputActions.Player.Move;
        _moveAction.Enable();
        
        _inputActions.Player.Attack.performed += OnAttack;
        _inputActions.Player.Attack.Enable();
        
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //start isn't used because we're using awake instead
    }
 private void OnAttack(InputAction.CallbackContext context)
 {
     Debug.Log("Attack");
 } 
    private void FixedUpdate() 
    {
        //move the player
        Vector2 moveDir = _moveAction.ReadValue<Vector2>();
        _playerTransform.position += (new Vector3(moveDir.x, moveDir.y, 0) * Time.deltaTime)* movementSpeed;
        
        //update the sprite
        if (moveDir.x != 0 || moveDir.y != 0){ //if the player is moving
         if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y)) //if the player is moving more in the x direction than the y direction
         {
             if (moveDir.x > 0) //if the player is moving right
             {
                 changeSprite("Sprites/Ruri/Ruri Right");
                 CurrentDirection = Direction.Right;
             }
             else
             {
                 changeSprite("Sprites/Ruri/Ruri Left");
                 CurrentDirection = Direction.Left;
             }
         } else if(moveDir.y > 0)
         {
             changeSprite("Sprites/Ruri/Ruri Up");
             CurrentDirection = Direction.Up;
         }
         else
         {
             changeSprite("Sprites/Ruri/Ruri Down");
             CurrentDirection = Direction.Down;
         }
        
        }
    
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
