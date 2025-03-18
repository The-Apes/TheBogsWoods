using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Transform playerTransform;
    
    private DefaultInputActions _defaultInputActions;
    private InputAction _moveAction;
    private InputAction _switchAction;
    private void Awake()
    {
        _defaultInputActions = new DefaultInputActions();
    }
    private void OnEnable()
    {
        _moveAction = _defaultInputActions.Player.Move;
        _defaultInputActions.Enable();
    }
    private void OnDisable()
    {
        _defaultInputActions.Disable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 moveDir = _moveAction.ReadValue<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
