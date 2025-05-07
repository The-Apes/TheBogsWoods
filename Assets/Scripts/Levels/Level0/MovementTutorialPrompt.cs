using TMPro;
using UnityEngine;

public class MovementTutorialPrompt : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderer;
    private TextMeshPro[] textMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvents.onDialogueEnd += OnDialogueEnd;
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
    private void OnDialogueEnd(string dialogueName)
    {
        if (dialogueName == "LevelStartDialogue")
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
    }
    private void OnAreaChange(string areaName)
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameEvents.onDialogueEnd -= OnDialogueEnd;
        GameEvents.onAreaChange -= OnAreaChange;
    }
}
