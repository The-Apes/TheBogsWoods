using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class OttoScript : MonoBehaviour
    {
        private Transform _playerTransform; 
        private SpriteRenderer _spriteRenderer; 
    
        [SerializeField] private float walkSpeed = 4f; 
        [SerializeField] private float runSpeed = 6f;
    
        private bool _running;
        private Vector2 _moveInput;
        public Transform lookDir;
        private enum Direction { Up, Down, Left, Right }
        private static Direction _currentDirection = Direction.Down;
    
        private void Awake()
        {
            _playerTransform = this.transform;
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _spriteRenderer.sprite;
        }
    
        private void OnAttack(InputAction.CallbackContext context) {
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
        }
    
        private void UpdateDirection()
        {
            if (_moveInput is { x: 0, y: 0 }) return; 
        
            //if the player is moving more in the x direction than the y direction
            if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y)) 
            {
                _currentDirection = _moveInput.x > 0 ? Direction.Right : Direction.Left; 
            } 
            else 
            {
                _currentDirection = _moveInput.y > 0 ?  Direction.Up : Direction.Down; 
            } 
        }

        public void MoveInput(InputAction.CallbackContext context) 
        {
            _moveInput = context.ReadValue<Vector2>();
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
    
        public void Attack(InputAction.CallbackContext context) 
        {
            if (context.started){GetComponent<OttoShoot>().ShootInput = true;}
            if (context.canceled){GetComponent<OttoShoot>().ShootInput = false;}
        }
    }
}
