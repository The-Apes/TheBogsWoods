using System;
using System.Collections;
using Managers;
using Player;
using UnityEngine;

public class HealthPotion : FakeHeightObject
{
    private Animator _animator;
    
    [SerializeField] private GameObject body;
    
    [SerializeField] private float interactableHeight = 0.4f;
    
    [SerializeField] private Collider2D col;
    [SerializeField] private Collider2D triggerCol;

    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip collideSound;


    private bool _active = true;
    
    private void Awake(){
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other){
            
        if (!other.gameObject.CompareTag("Player")) return;
       TryCollect();
  
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.instance.PlaySFXAt(collideSound, transform, 0.7f, 0.25f);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(!_active) return;
        col.enabled = !(body.transform.localPosition.y > interactableHeight);
        triggerCol.enabled = !(body.transform.localPosition.y > interactableHeight);
    }

    private void TryCollect()
    {
        var playerHealth = RuriMovement.instance.gameObject.GetComponent<PlayerHealth>();
        
        if(playerHealth.currentHealth >= playerHealth.maxHealth) return;
        
        playerHealth.Heal();
        _animator.SetTrigger("Collect");
        AudioManager.instance.PlaySFXAt(collectSound, transform);
        isGrounded = true;
        Stick();
        _active = false;
        col.enabled = false;
        triggerCol.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(WaitForAnimation());
    }
    IEnumerator WaitForAnimation()
    {
        float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        Destroy(gameObject);
    }


}
