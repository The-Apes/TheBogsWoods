using System;
using System.Collections;
using Managers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class RuriMovement : MonoBehaviour
    {
        public static RuriMovement instance;
    
        private RuriAttack _ruriAttack;
        private PlayerHealth _playerHealth;
    
        private Animator _animator;
 
        [NonSerialized] public GameObject Otto; 
        [NonSerialized] public GameObject RidingOtto; 

    
        private Transform _playerTransform; 
        private AudioSource _audioSource;
        private CinemachineCamera _camera;
        private Rigidbody2D _rb;
    
        [SerializeField] private GameObject ridingOttoPrefab;
        [SerializeField] private GameObject ottoPrefab; 
        [SerializeField] private GameObject fairy;
    
    
        private bool _running;
        private bool _dashing;
        public bool controlling = true;
        private bool _ottoInRange; 
    
        public bool ottoMounted; 

        public bool hasWeapon = true;
        public bool hasOtto = true;
        public bool hasFairy;
    
        [SerializeField] private float walkSpeed = 1.5f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
        [SerializeField] private float runSpeed = 3f; //this is a variable that stores the movement speed of the player, this is the speed at which the player moves
        private Vector2 _moveInput;
        [SerializeField] private Collider2D hurtBox;
    
        [Header("Sounds")]
        [SerializeField] private AudioClip footstepSFX;
        [SerializeField] private AudioClip dashSFX;

        public enum Direction { Up, Right, Down, Left }
        //private enum CharacterState { Moving, Attacking, Dashing, Hit } - Idk if we need this, i'll stick to bools but if it gets intense im pulling this out
        private static Direction _attackDirection = Direction.Down;
        private static Direction _movingDirection = Direction.Down;
        //^^ an enum variable is a variable that can only have a set of predefined values, in this case it can only be: Up, Down, Left, Right
    
    
        private void Awake()
        {
            if (instance == null)
                instance = this;
            _playerTransform = transform;
            _camera = FindFirstObjectByType<CinemachineCamera>();
            _ruriAttack = GetComponent<RuriAttack>();
            _playerHealth = GetComponent<PlayerHealth>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();

            if (hasOtto)
            {
                AddOtto();
            }

            fairy.GetComponent<SpriteRenderer>().enabled = hasFairy;
        
            //makes here start the game facing down
            _animator.SetFloat("AimX", 0f);
            _animator.SetFloat("AimY", -1f);
            _animator.SetFloat("BodyX", 0f);
            _animator.SetFloat("BodyY", -1f);

        }

        public void AddStar()
        {
            hasFairy = true;
            fairy.GetComponent<SpriteRenderer>().enabled = hasFairy;
        }

        public void RemoveStar()
        {
            hasFairy = false;
            fairy.GetComponent<SpriteRenderer>().enabled = hasFairy;
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
    
        // ReSharper disable once UnusedMember.Local
        private void PlaySound(AudioClip soundClip)
        {
            AudioManager.instance.PlaySFXAt(soundClip, transform);
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
            if (controlling) //if the player is not controlling ruri, return_
            {
                if (_ruriAttack.isAttacking) return;
                if (_dashing) return;
                float
                    speed = _running
                        ? runSpeed
                        : walkSpeed; //this is a ternary; if the player is running, set the speed to runSpeed, else set it to walkSpeed
                _playerTransform.position += new Vector3(_moveInput.x, _moveInput.y, 0) * (Time.deltaTime * speed);


                UpdateDirection();
                UpdateAttackDirection();
                _animator.SetBool("Moving", _moveInput != Vector2.zero);

                _animator.SetInteger("Direction", (int)_movingDirection);
                //sss

                if (_ruriAttack.isAttacking) return;
                SetLookDirection();
            }
            else
            {
                _animator.SetBool("Moving", false);
                _attackDirection = _movingDirection;
                SetLookDirection();
            }

            //move the player
        }

        private void SetLookDirection()
        {
            switch (_attackDirection)
            {
                case Direction.Up:
                    _animator.SetFloat("AimX", 0f);
                    _animator.SetFloat("AimY", 1f);
                    break;
                case Direction.Down:
                    _animator.SetFloat("AimX", 0f);
                    _animator.SetFloat("AimY", -1f);
                    break;
                case Direction.Left:
                    _animator.SetFloat("AimX", -1f);
                    _animator.SetFloat("AimY", 0f);
                    break;
                case Direction.Right:
                    _animator.SetFloat("AimX", 1f);
                    _animator.SetFloat("AimY", 0f);
                    break;
            }
        }

        private void UpdateDirection()
        {
            if(_moveInput.Equals(Vector2.zero)) return;
            if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y))
            {
                _movingDirection = _moveInput.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                _movingDirection = _moveInput.y > 0 ? Direction.Up : Direction.Down;
            }
        
            switch (_movingDirection)
            {
                case Direction.Up:
                    _animator.SetFloat("BodyX", 0f);
                    _animator.SetFloat("BodyY", 1f);
                    break;
                case Direction.Down:
                    _animator.SetFloat("BodyX", 0f);
                    _animator.SetFloat("BodyY", -1f);
                    break;
                case Direction.Left:
                    _animator.SetFloat("BodyX", -1f);
                    _animator.SetFloat("BodyY", 0f);
                    break;
                case Direction.Right:
                    _animator.SetFloat("BodyX", 1f);
                    _animator.SetFloat("BodyY", 0f);
                    break;
            }
        }
    
    
        private void UpdateAttackDirection()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // to do make camera manager class
            Vector3 mouseDirection = mouseWorldPos - _playerTransform.position;
            mouseDirection.z = 0;

            // Cardinal direction logic
            if (Mathf.Abs(mouseDirection.x) > Mathf.Abs(mouseDirection.y))
            {
                _attackDirection = mouseDirection.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                _attackDirection = mouseDirection.y > 0 ? Direction.Up : Direction.Down;
            }
        
        }

        public void Death()
        {
            controlling = false;
            _animator.SetTrigger("Death");
        }
        public void ApplyForceInDirection(Direction direction, float knockbackForce)
        {
            Vector2 knockbackDirection = Vector2.zero;
            switch (direction)
            {
                case Direction.Up:
                    knockbackDirection = Vector2.up;
                    break;
                case Direction.Right:
                    knockbackDirection = Vector2.right;
                    break;
                case Direction.Down:
                    knockbackDirection = Vector2.down;
                    break;
                case Direction.Left:
                    knockbackDirection = Vector2.left;
                    break;
            }
            _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        public void ApplyForce(Vector2 direction, float force)
        {
            _rb.AddForce(direction * force, ForceMode2D.Impulse);
        }

        IEnumerator DashTimer(float duration)
        {
            _dashing = true;
            yield return new WaitForSeconds(duration);
            _dashing = false;
        }
        public void AttackMove()
        {
            ApplyForceInDirection(_attackDirection, 2f);
        }

        public void Footstep()
        {
            AudioManager.instance.PlaySFX(footstepSFX, 0.5f);
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
        public void Dash(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if(!controlling || _ruriAttack.isAttacking|| _dashing) return;
                if(_moveInput.Equals(Vector2.zero)) return;
                ApplyForce(_moveInput, 20f);
                _animator.SetTrigger("Dash"); 
                _animator.SetBool("Moving", false);
                _playerHealth.BecomeInvulnerable(0.3f);
                StartCoroutine(DashTimer(0.3f));
                GetComponent<DashFlash>().CallDashFlash(0.3f);
                AudioManager.instance.PlaySFX(dashSFX);
            
            }
        }
        #endregion
    }
}
