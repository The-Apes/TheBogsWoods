using System.Collections;
using UnityEngine;

// Reference: Create A DAMAGE FLASH Effect for Sprites | Unity Tutorial
// by Sasquatch B Studios - https://www.youtube.com/watch?v=UwcF84JFx0g
public class DashFlash : MonoBehaviour
{
  [SerializeField] private Color flashColor = Color.white;
  
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

  public void CallDashFlash(float duration)
  {
    _damageFlashCoroutine = StartCoroutine(DashFlasher(duration));
    
  }

  private IEnumerator DashFlasher(float duration)
  {
   SetFlashAmount(0.3f);
   yield return new WaitForSeconds(duration);
   SetFlashAmount(0f);
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
