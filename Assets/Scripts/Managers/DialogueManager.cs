
using DialogueFramework;
using UnityEngine;

//reference https://www.youtube.com/watch?v=DOP_G5bsySA
namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        
        public GameObject dialogueUI;
        public GameObject canvas;
        
        private DialogueSystem _dialogueSystem; 
        
        private DialogueLine _currentLine;
    
        private Dialogue _currentDialogue;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject); 
            }
        }
     
        public void StartDialogue(DialogueAsset dialogueAsset)
        {
            HandleUI();
            _dialogueSystem.StartDialogue(dialogueAsset);
            
        }
        public void StartDialogue(Dialogue dialogue)
        {
            HandleUI();
            _dialogueSystem.StartDialogue(dialogue);
            
        }
        
        private void HandleUI()
        {
            if (_dialogueSystem) return;
            _dialogueSystem = FindFirstObjectByType<DialogueSystem>();
            if (_dialogueSystem) return;
            if (!FindFirstObjectByType<Canvas>())
            {
                Instantiate(canvas);
            }
            _dialogueSystem = Instantiate(dialogueUI, FindFirstObjectByType<Canvas>().transform).GetComponent<DialogueSystem>();
        }
        
    }
}
