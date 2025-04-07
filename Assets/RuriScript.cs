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
    
    [SerializeField] private GameObject ottoPrefab; //this is a variable that stores the otto game object
    private GameObject _otto; //this is a variable that stores the otto game object, this is the one that will be used in the game
    public bool hasOtto = false; //this is a variable that stores if the player has otto or not
    //it's public because this is also checked when the game starts, so you can set it in the inspector
    
    [SerializeField] private float movementSpeed = 1.5f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    private Vector2 _moveInput;
    
    public enum Direction { Up, Down, Left, Right }
    public static Direction CurrentDirection = Direction.Down;
    //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    //Awake Is Called before start
    private void Awake()
    {
        _playerTransform = this.transform;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteRenderer.sprite;
        //_audioSource = this.GetComponent<AudioSource>(); sounds maybe
        if (hasOtto == true)
        {
         addOtto();
        }

    }
//Hey CoPilot, how do I get "ottoSocket?" it's a gameObject that is a child of the player
    //I want to set the position of otto to be the same as ottoSocket show me the code in the next comment below
    //
    private void addOtto()
    {
        GameObject Socket = GameObject.Find("OttoSocket");
        _otto = Instantiate(ottoPrefab, Socket.transform); //creates a new game object
        
        _otto.transform.localPosition = new Vector3(0, 0.17f, 0); //this is the position of the otto game object relative to the player
        _otto.transform.localRotation = Quaternion.identity; //Identity just means zero rotation essentially.
    }
    private void removeOtto()   
    {
        Destroy(_otto); //destroys the otto game object
        _otto = null; //sets the otto game object to null, basically like when we instantiated it
    }

    private void changeSprite(String path)
    {
       
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
        _playerTransform.position += (new Vector3(_moveInput.x, _moveInput.y, 0) * Time.deltaTime)* movementSpeed;
        
        //update the sprite
        if (_moveInput.x != 0 || _moveInput.y != 0){ //if the player is moving
         if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y)) //if the player is moving more in the x direction than the y direction
         {
             if (_moveInput.x > 0) //if the player is moving right
             {
                 changeSprite("Sprites/Ruri/Ruri Right");
                 CurrentDirection = Direction.Right;
             }
             else
             {
                 changeSprite("Sprites/Ruri/Ruri Left");
                 CurrentDirection = Direction.Left;
             }
         } else if(_moveInput.y > 0)
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

    public void MoveInput(InputAction.CallbackContext context) //Called by input system
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void Attack() //Called by input system
    {
        Debug.Log("Attack");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
