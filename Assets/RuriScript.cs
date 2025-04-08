using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerScript : MonoBehaviour
{
    // hey bro's, anything in double slashes are comments, they are not read by the computer, they are just for you to read
    // so you can understand me code good sirs, I need to practice coding for others to understand.
    
    private Transform _playerTransform; //this is a variable that stores the transform of the player
    private SpriteRenderer _spriteRenderer; //this is a variable that stores the sprite renderer of the player, what you see on the screen
    private AudioSource _audioSource;
    
    [SerializeField] private GameObject ridingOttoPrefab; //this is a variable that stores the otto game object
    private GameObject _ridingOtto; //this is a variable that stores the otto game object, this is the one that will be used in the game
    [SerializeField] private GameObject ottoPrefab; //this is a variable that stores the otto game object
    private GameObject _otto; //this is a variable that stores the otto game object, this is the one that will be used in the game
    
    private bool _running = false; //this is a variable that stores if the player is running or not
    private bool controlling = true;
    private bool ottoInRange = false; //this is a variable that stores if the player is in range of the otto
    public bool hasOtto; //this is a variable that stores if the player has otto or not
    //it's public because this is also checked when the game starts, so you can set it in the inspector
    
    [SerializeField] private float walkSpeed = 1.5f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    [SerializeField] private float runSpeed = 3f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves

    private Vector2 _moveInput;
    
    public enum Direction { Up, Down, Left, Right }
    public static Direction CurrentDirection = Direction.Down;
    //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    
    private void Awake()
    {
        _playerTransform = this.transform;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteRenderer.sprite;

        //_audioSource = this.GetComponent<AudioSource>(); sounds maybe
        if (hasOtto)
        {
         AddOtto();
        }

    }

    private void AddOtto()
    {
        GameObject socket = GameObject.Find("OttoSocket");
        _ridingOtto = Instantiate(ridingOttoPrefab, socket.transform); //creates a new game object
        
        _ridingOtto.transform.localPosition = new Vector3(0, 0.17f, 0); //this is the position of the otto game object relative to the player
        _ridingOtto.transform.localRotation = Quaternion.identity; //Identity just means zero rotation essentially.
    }
    private void RemoveOtto()   
    {
        Destroy(_ridingOtto); //destroys the otto game object
        _ridingOtto = null; //sets the otto game object to null, basically like when we instantiated it
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           ottoInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ottoInRange = false;
        }
    }
    
 private void OnAttack(InputAction.CallbackContext context)
 {
     Debug.Log("Attack");
 } 
    private void FixedUpdate() 
    {
        //move the player
        float speed = _running ? runSpeed : walkSpeed; //if the player is running, set the speed to runSpeed, else set it to walkSpeed
        _playerTransform.position += new Vector3(_moveInput.x, _moveInput.y, 0) * (Time.deltaTime * speed);
        //update the sprite
        if (_moveInput.x == 0 && _moveInput.y == 0) return; //if the player is moving
        if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y)) //if the player is moving more in the x direction than the y direction
        {
            if (_moveInput.x > 0) //if the player is moving right
            {
                CurrentDirection = Direction.Right;
            }
            else
            {
                CurrentDirection = Direction.Left;
            }
        } else if(_moveInput.y > 0)
        {
            CurrentDirection = Direction.Up;
        }
        else
        {
            CurrentDirection = Direction.Down;
        }
    }

    public void MoveInput(InputAction.CallbackContext context) //Called by input system
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void Attack(InputAction.CallbackContext context) //Called by input system
    {
        if (context.started)
            Debug.Log("Attack");
    }
    public void SwitchCharacter(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (hasOtto)
        {
            RemoveOtto();
            hasOtto = false;
            _otto = Instantiate(ottoPrefab, new Vector3(_playerTransform.position.x,_playerTransform.position.y+0.75f,_playerTransform.position.z), Quaternion.identity);
            controlling = false;
            print("Otto Dismount");
        } else if (ottoInRange && !hasOtto){
            Destroy(_otto);
            hasOtto = true;
            AddOtto();
            controlling = true;
            print("Otto Mount");
        }
        else
        {
            print("otto not in range");
        }
    }

    public void run(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _running = true;
        }
        if (context.canceled)
        {
            _running = false;
        }
    }
}
