using System;
using UnityEngine;
using System.Collections;

public class EnemyHitFeedback : MonoBehaviour
{
    private SpriteRenderer sr;
    public Material whiteFlashMat;
    private Material originalMat;
    private Rigidbody2D rb;

    public float knockbackForce = 5f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        originalMat = sr.material;
    }

    public void OnHit(Vector2 hitDirection)
    {
        StartCoroutine(Flash());
        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator Flash()
    {
        sr.material = whiteFlashMat;
        yield return new WaitForSeconds(0.05f);
        sr.material = originalMat;
    }
}