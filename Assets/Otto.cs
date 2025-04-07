using System;
using UnityEngine;

public class Otto : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer; //this is a variable that stores the sprite renderer of the player, what you see on the screen
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    
    private void changeSprite(String path)
    {
        _spriteRenderer.sprite = Resources.Load<Sprite>(path);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
