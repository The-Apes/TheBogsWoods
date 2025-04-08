using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class OttoScript : MonoBehaviour
{
    // hey bro's, anything in double slashes are comments, they are not read by the computer, they are just for you to read
    // so you can understand me code good sirs, I need to practice coding for others to understand.
    
    private Transform _playerTransform; //this is a variable that stores the transform of the player
    private SpriteRenderer _spriteRenderer; //this is a variable that stores the sprite renderer of the player, what you see on the screen
    
    
    //it's public because this is also checked when the game starts, so you can set it in the inspector
    private bool _running = false;
    [SerializeField] private float walkSpeed = 4f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    [SerializeField] private float runSpeed = 6f;
    private Vector2 _moveInput;
    
    //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    
    private void Awake()
    {
        _playerTransform = this.transform;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteRenderer.sprite;
        //_audioSource = this.GetComponent<AudioSource>(); sounds maybe
       

    }

  
    
 private void OnAttack(InputAction.CallbackContext context)
 {
     Debug.Log("Attack");
 } 
    private void FixedUpdate() 
    {
        //move the player
        float speed = _running ? runSpeed : walkSpeed;
        _playerTransform.position += new Vector3(_moveInput.x, _moveInput.y, 0) * (Time.deltaTime * speed);
        
        //update the sprite
      
    }

    public void MoveInput(InputAction.CallbackContext context) //Called by input system
    {
        _moveInput = context.ReadValue<Vector2>();
        print(_moveInput);
    }
    public void Run(InputAction.CallbackContext context){
        if (context.performed)
        {
            _running = true;
        }
        else if (context.canceled)
        {
            _running = false;
        }
    }
    public void Attack(InputAction.CallbackContext context) //Called by input system
    {
        Debug.Log("Attack");
    }
}
