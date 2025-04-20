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

    public Transform lookDir;
    private enum Direction { Up, Down, Left, Right }
    private static Direction _currentDirection = Direction.Down;
    
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
        UpdateDirection();
        lookDir.rotation = _currentDirection switch
        {
            Direction.Up => Quaternion.Euler(0, 0, 180),
            Direction.Down => Quaternion.Euler(0, 0, 0),
            Direction.Left => Quaternion.Euler(0, 0, 270),
            Direction.Right => Quaternion.Euler(0, 0, 90),
            _ => lookDir.rotation
        };
        //update the sprite
      
    }
    private void UpdateDirection()
    {
        if (_moveInput.x == 0 && _moveInput.y == 0) return; //if the player is moving (return makes the code stop executing, which will ignore the rest of the code below)
        if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y)) //if the player is moving more in the x direction than the y direction
        {
            _currentDirection = _moveInput.x > 0 ? Direction.Right : Direction.Left; //another ternary, if the player is moving right
        } else 
        {
            _currentDirection = _moveInput.y > 0 ?  Direction.Up : Direction.Down; //if the player is moving up
        } 
    }

    public void MoveInput(InputAction.CallbackContext context) //Called by input system
    {
        _moveInput = context.ReadValue<Vector2>();
        //print(_moveInput);
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
        if (context.started){GetComponent<OttoShoot>().ShootInput = true;}
        if (context.canceled){GetComponent<OttoShoot>().ShootInput = false;}

        
    }
}
