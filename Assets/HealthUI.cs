using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heartPrefab; // Prefab for the heart icon
    public Sprite fullHeartSprite; // Sprite for a full heart
    public Sprite emptyHeartSprite; // Sprite for an empty heart
    
    private List<Image> hearts = new List<Image>(); // List to hold the heart icons
    
    public void SetMaxHearts(int maxHearts)
    {
        // Clear existing hearts
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();

        // Create new hearts
        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform);
            newHeart.sprite = fullHeartSprite;
            newHeart.color = Color.red;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite; // Set to full heart
                hearts[i].color = Color.red; // Set color to red
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite; // Set to empty heart
                hearts[i].color = Color.white; // Set color to white
            }
        }
    }
}
