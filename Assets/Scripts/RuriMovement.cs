using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class RuriMovement : MonoBehaviour
{
    public static RuriMovement instance;
    
    private RuriAttack _ruriAttack;
 
    [NonSerialized] public GameObject Otto; 
    [NonSerialized] public GameObject RidingOtto; 

    
    private Transform _playerTransform; 
    private AudioSource _audioSource;
    private CinemachineCamera _camera;
    
    [SerializeField] private GameObject ridingOttoPrefab;
    [SerializeField] private GameObject ottoPrefab; 
    
    
    private bool _running; 
    public bool controlling = true;
    private bool _ottoInRange; 
    
    public bool ottoMounted; 

    public bool hasWeapon = true;
    public bool hasOtto = true;

    
    
    [SerializeField] private float walkSpeed = 1.5f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    [SerializeField] private float runSpeed = 3f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
    public Transform lookDir; //this is a variable that stores the transform of the player, this is the one that will be used to rotate the player
    private Vector2 _moveInput;
    [SerializeField] private Collider2D hurtBox;

    private enum Direction { Up, Down, Left, Right }
    private static Direction _currentDirection = Direction.Down;
    //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        _playerTransform = transform;
        _camera = FindFirstObjectByType<CinemachineCamera>();
        _ruriAttack = GetComponent<RuriAttack>();

        if (hasOtto)
        {
         AddOtto();
        }

    }

    public void AddOtto()
    {
        GameObject socket = GameObject.Find("OttoSocket");
        RidingOtto = Instantiate(ridingOttoPrefab, socket.transform); //creates a new game object
        RidingOtto.transform.localPosition = new Vector3(0, -0.56f, 0); //this is the position of the otto game object relative to the player
        RidingOtto.transform.localRotation = Quaternion.identity; //Identity just means zero rotation essentially.
    }
    private void RemoveOtto()   
    {
        Destroy(RidingOtto); //destroys the otto game object
        RidingOtto = null; //sets the otto game object to null, basically like when we instantiated it
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           _ottoInRange = true;
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
        if (!controlling) return; //if the player is not controlling the otto, return_
       // UpdateAttackTimer();
       
       //move the player
        float speed = _running ? runSpeed : walkSpeed; //this is a ternary; if the player is running, set the speed to runSpeed, else set it to walkSpeed
        _playerTransform.position += new Vector3(_moveInput.x, _moveInput.y, 0) * (Time.deltaTime * speed);
        
       UpdateDirection();

       if (_ruriAttack.isAttacking) return;
       lookDir.rotation = _currentDirection switch
       {
           Direction.Up => Quaternion.Euler(0, 0, 180),
           Direction.Down => Quaternion.Euler(0, 0, 0),
           Direction.Left => Quaternion.Euler(0, 0, 270),
           Direction.Right => Quaternion.Euler(0, 0, 90),
           _ => lookDir.rotation
       };
    }

    private void UpdateDirection()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // to do make camera manager class
        Vector3 direction = mouseWorldPos - _playerTransform.position;
        direction.z = 0;

        // Cardinal direction logic
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            _currentDirection = direction.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            _currentDirection = direction.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    #region InputAction
    public void MoveInput(InputAction.CallbackContext context) //Called by input system
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void SwitchCharacter(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!hasOtto) return;
        if (ottoMounted)
        {
            RemoveOtto();
            ottoMounted = false;
            Otto = Instantiate(ottoPrefab, new Vector3(_playerTransform.position.x,_playerTransform.position.y+0.75f,_playerTransform.position.z), Quaternion.identity);
            _camera.Follow = Otto.transform;
            controlling = false;
        } else if (_ottoInRange){
            Destroy(Otto);
            ottoMounted = true;
            AddOtto();
            controlling = true;
            _camera.Follow = _playerTransform;
        }
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
    #endregion
}
