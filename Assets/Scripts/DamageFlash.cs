using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference: Create A DAMAGE FLASH Effect for Sprites | Unity Tutorial
// by Sasquatch B Studios - https://www.youtube.com/watch?v=UwcF84JFx0g
public class DamageFlash : MonoBehaviour
{
  [SerializeField] private Color flashColor = Color.white;
  [SerializeField] private float flashTime = 0.2f;
  
  private SpriteRenderer[] _spriteRenderers;
  private Material[] _materials;
  
  private Coroutine _damageFlashCoroutine;

  private void Awake()
  {
    _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    Init();
  }

  private void Init()
  {
    _materials = new Material[_spriteRenderers.Length];
    for (int i = 0; i < _spriteRenderers.Length; i++)
    {
      _materials[i] = _spriteRenderers[i].material;
    }
  }

  public void CallDamageFlash()
  {
    _damageFlashCoroutine = StartCoroutine(DamageFlasher());
  }

  private IEnumerator DamageFlasher()
  {
    //set color
    SetFlashColor();
    //lerp flash
    float currentFlashAmount = 0f;
    float elapsedTime = 0f;
    while (elapsedTime < flashTime)
    {
      //iterate elapsed time
      elapsedTime += Time.deltaTime;
      
      //lerp flash time
      currentFlashAmount = Mathf.Lerp(1f,0f,(elapsedTime / flashTime));
      SetFlashAmount(currentFlashAmount);

      yield return null;
    }
  }

  private void SetFlashColor()
  {
    for (int i = 0; i < _materials.Length; i++)
    {
      _materials[i].SetColor("_FlashColor", flashColor);
    }
  }

  private void SetFlashAmount(float amount)
  {
    //set the flash amount
    for (int i = 0; i < _materials.Length; i++)
    {
      _materials[i].SetFloat("_FlashAmount", amount);
    }
  }
}
