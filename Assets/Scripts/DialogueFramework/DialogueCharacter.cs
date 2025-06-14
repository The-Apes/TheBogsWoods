using UnityEngine;

namespace DialogueFramework
{
    [CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "New Dialogue Character")]
    public class DialogueCharacter : ScriptableObject
    {
        public string characterName; 
        public Sprite characterSprite; // Sprite representing the character
        public Color color = Color.white;
        [Header("Voice")]
        public AudioClip[] blipSound;
        public float frequency = 0.15f;
        public float volume = 0.75f;
        public float pitch = 1f; 
        // Audio Blip for the character's voice
    }
    [System.Serializable]
    public class DialogueCharacterOverride
    {
        public string characterName; 
        public Sprite characterSprite; // Sprite representing the character
        public Color color;
        [Header("Voice")]
        public AudioClip[] blipSound;
        public float frequency;
        public float volume;
        public float pitch; 
        // Audio Blip for the character's voice
    }
}