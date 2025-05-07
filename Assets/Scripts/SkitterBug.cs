using UnityEngine;

public class SkitterBug : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the SkitterBug moves
    public float slowDownDuration = 3f; // Time before the SkitterBug slows down
    public float slowSpeed = 1f; // Speed when the SkitterBug slows down
    public float detectionRange = 3f; // Range to detect incoming attacks

    private Rigidbody2D _rb;
    private bool _isAvoiding = false;
    private bool _isSlowingDown = false;
    private float _avoidTimer = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isAvoiding) return;
        _avoidTimer += Time.deltaTime;

        // Slow down after the specified duration
        if (!(_avoidTimer >= slowDownDuration)) return;
        _isAvoiding = false;
        _isSlowingDown = true;
        _rb.linearVelocity = Vector2.zero; // Stop movement before slowing down
    }

    private void FixedUpdate()
    {
        if (_isSlowingDown)
        {
            // Move slowly to simulate an opening
            _rb.linearVelocity = Vector2.zero; // Optional: Add slight movement if needed
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("HitBox") || _isSlowingDown) return;
        Vector2 avoidDirection = (transform.position - other.transform.position).normalized;
        _rb.linearVelocity = avoidDirection * moveSpeed;
        _isAvoiding = true;
        _avoidTimer = 0f; 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HitBox"))
        {
            _rb.linearVelocity = Vector2.zero; // Stop moving when out of range
        }
    }
}
