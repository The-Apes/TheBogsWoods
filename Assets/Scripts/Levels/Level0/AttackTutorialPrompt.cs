using Managers;
using TMPro;
using UnityEngine;

namespace Levels.Level0
{
    public class AttackTutorialPrompt : MonoBehaviour
    {
        private SpriteRenderer[] spriteRenderer;
        private TextMeshPro[] textMesh;
        private void Start()
        {
            GameEvents.onAreaChange += OnAreaChange;
        
            spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
            textMesh = GetComponentsInChildren<TextMeshPro>();
            foreach (SpriteRenderer sprite in spriteRenderer){
                sprite.enabled = false;
            }
            foreach (TextMeshPro text in textMesh)
            {
                text.enabled = false;
            }
        
        }
        private void OnAreaChange(string areaName)
        {
            if (textMesh[0].enabled) Destroy(gameObject);
        }
        public void ShowPrompt()
        {
            foreach (SpriteRenderer sprite in spriteRenderer)
            {
                sprite.enabled = true;
            }
            foreach (TextMeshPro text in textMesh)
            {
                text.enabled = true;
            }
        }
        private void OnDisable()
        {
            GameEvents.onAreaChange -= OnAreaChange;
        }
    }
}
