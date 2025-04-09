using System.Collections;
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
    private PlayerInput _playerInput;
    private CinemachineCamera _camera;
    
    [SerializeField] private GameObject ridingOttoPrefab; //this is a variable that stores the otto game object
    private GameObject _ridingOtto; //this is a variable that stores the otto game object, this is the one that will be used in the game
    [SerializeField] private GameObject ottoPrefab; //this is a variable that stores the otto game object
    private GameObject _otto; //this is a variable that stores the otto game object, this is the one that will be used in the game
    
    private bool _running; //this is a variable that stores if the player is running or not
    private bool _controlling = true;
    private bool _ottoInRange; //this is a variable that stores if the player is in range of the otto
    public bool hasOtto; //this is a variable that stores if the player has otto or not

    private bool _isAttacking;
    private float _attackTimer;
    private float _attackDuration = 0.3f;
    
    [SerializeField] private float walkSpeed = 1.5f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    [SerializeField] private float runSpeed = 3f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    [SerializeField] Transform _lookDir;
    private Vector2 _moveInput;
    [SerializeField] private Collider2D hitBox;
    [SerializeField] private Collider2D hurtBox;
    
    public enum Direction { Up, Down, Left, Right }
    public static Direction CurrentDirection = Direction.Down;
    //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    
    private void Awake()
    {
        _playerTransform = this.transform;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _camera = FindFirstObjectByType<CinemachineCamera>();
        hitBox.gameObject.SetActive(false);
        _playerInput = this.GetComponent<PlayerInput>();
    

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
           _ottoInRange = true;
        }

        if (collision.IsTouching(hurtBox) && collision.gameObject.CompareTag("HitBox"))
        {
            print("Ow! I just took "+collision.gameObject.GetComponent<HitBox>().damage+" damage!");
        }
        if (collision.IsTouching(hitBox) && collision.gameObject.CompareTag("HurtBox")){
            print("I just hit "+collision.gameObject.name+" for "+hitBox.GetComponent<HitBox>().damage+" damage!");
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _ottoInRange = false;
        }
    }
    
    private void FixedUpdate() 
    {
        
        if (!_controlling) return; //if the player is not controlling the otto, return_
       // UpdateAttackTimer();
       
       //move the player
        float speed = _running ? runSpeed : walkSpeed; //this is a ternary; if the player is running, set the speed to runSpeed, else set it to walkSpeed
        _playerTransform.position += new Vector3(_moveInput.x, _moveInput.y, 0) * (Time.deltaTime * speed);
        
       UpdateDirection();

       switch (CurrentDirection)
       {
           case Direction.Up:
               _lookDir.rotation = Quaternion.Euler(0, 0, 180);
               break;
           case Direction.Down:
               _lookDir.rotation = Quaternion.Euler(0, 0, 0);
               break;
           case Direction.Left:
               _lookDir.rotation = Quaternion.Euler(0, 0, 270);
               break;
           case Direction.Right:
               _lookDir.rotation = Quaternion.Euler(0, 0, 90);
               break;
       }
       
    }
    private void UpdateAttackTimer()
    {
        if (_isAttacking)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackDuration)
            {
                hitBox.gameObject.SetActive(false);
                _isAttacking = false;
                _attackTimer = 0f;
                print("Attack Ended");
            }
        }
    }
    private void UpdateDirection()
    {
        if (_moveInput.x == 0 && _moveInput.y == 0) return; //if the player is moving (return makes the code stop executing, which will ignore the rest of the code below)
        if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y)) //if the player is moving more in the x direction than the y direction
        {
            CurrentDirection = _moveInput.x > 0 ? Direction.Right : Direction.Left; //another ternary, if the player is moving right
        } else 
        {
            CurrentDirection = _moveInput.y > 0 ?  Direction.Up : Direction.Down; //if the player is moving up
        } 
    }
    public void MoveInput(InputAction.CallbackContext context) //Called by input system
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void Attack(InputAction.CallbackContext context) //Called by input system
    {
        if (!context.started) return;
        if (_isAttacking) return;
        StartCoroutine(PerformAttack());
        // hitBox.gameObject.SetActive(true);
        // _isAttacking = true;
        // print("Attack");
    }
    
    public void SwitchCharacter(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (hasOtto)
        {
            RemoveOtto();
            hasOtto = false;
            _otto = Instantiate(ottoPrefab, new Vector3(_playerTransform.position.x,_playerTransform.position.y+0.75f,_playerTransform.position.z), Quaternion.identity);
           // _otto.GetComponent<PlayerInput>().SwitchCurrentControlScheme(_playerInput.currentControlScheme, _playerInput.devices.ToArray());
            _camera.Follow = _otto.transform;
            _controlling = false;
            print("Otto Dismount");
        } else if (_ottoInRange){
            Destroy(_otto);
            hasOtto = true;
            AddOtto();
            _controlling = true;
            _camera.Follow = _playerTransform;
            print("Otto Mount");
        }
   
    }
    private IEnumerator PerformAttack()
    {
        
        hitBox.gameObject.SetActive(true);
        _isAttacking = true;
        print("Attack");

        yield return new WaitForSeconds(_attackDuration);

        hitBox.gameObject.SetActive(false);
        _isAttacking = false;
        print("Attack Ended");
    }
    

    public void Run(InputAction.CallbackContext context)
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
