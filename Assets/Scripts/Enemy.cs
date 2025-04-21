using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float chaseSpeed = 2f; // Speed at which the enemy chases the player
    public float detectionRange = 5f; // Range within which the enemy detects the player
    private GameObject _chaseTarget;

    private Rigidbody2D _rb;
    [SerializeField] private Collider2D detectionZone;
    


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_chaseTarget)
        {
            Vector2 direction = (_chaseTarget.transform.position - transform.position).normalized; // Direction towards the player
            _rb.linearVelocity = direction.normalized * chaseSpeed; // Move the enemy towards the player
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) //when entering 2D sphere
    {
        if (detectionZone.IsTouching(other)){
            if (other.isTrigger) return; //if the object that entered my sphere is a trigger, return
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return; //if whatever entered my sphere is a part of the player layer mask
            if (other.gameObject.name == "Ruri" || other.gameObject.name.Contains("Otto"))
            {
                _chaseTarget = other.gameObject; //make the object that entered my sphere. my target
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!detectionZone.IsTouching(other)) {
            if (other.isTrigger) return; //if the object that entered my sphere is a trigger, return
            if(other.gameObject == _chaseTarget) //if the object that exited my sphere is my target
            {
                _chaseTarget = null; //make my target null
                _rb.linearVelocity = Vector2.zero;
            }
        }
    }
}