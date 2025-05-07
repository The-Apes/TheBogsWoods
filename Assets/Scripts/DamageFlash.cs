using System.Collections;
using UnityEngine;

// Reference: Create A DAMAGE FLASH Effect for Sprites | Unity Tutorial
// by Sasquatch B Studios - https://www.youtube.com/watch?v=UwcF84JFx0g
public class DamageFlash : MonoBehaviour
{
  [SerializeField] private Color flashColor = Color.white;
  [SerializeField] private float flashTime = 0.2f;
  
  private SpriteRenderer[] _spriteRenderers;
  private Material[] _materials;
  
  // ReSharper disable once NotAccessedField.Local
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
    float elapsedTime = 0f;
    while (elapsedTime < flashTime)
    {
      //iterate elapsed time
      elapsedTime += Time.deltaTime;
      
      //lerp flash time
      float currentFlashAmount = Mathf.Lerp(1f,0f,(elapsedTime / flashTime));
      SetFlashAmount(currentFlashAmount);

      yield return null;
    }
  }

  private void SetFlashColor()
  {
    foreach (var mat in _materials)
    {
      // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
      mat.SetColor("_FlashColor", flashColor);
    }
  }

  private void SetFlashAmount(float amount)
  {
    //set the flash amount
    foreach (var mat in _materials)
    {
      // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
      mat.SetFloat("_FlashAmount", amount);
    }
  }
}
