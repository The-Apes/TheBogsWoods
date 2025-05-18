using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class CGImage : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Sprite[] cgs;
    private void Start()
    {
        _image = GetComponent<Image>();
        DialogueSystem.onDialogueNextLine += OnDialogueNextLine;
        
    }
    
    private void OnDialogueNextLine(string EventName)
    {
        int cgIndex;
        if (EventName.ToLower().Contains("cg"))
        {
            cgIndex = int.Parse(EventName.Substring(EventName.Length - 1, 1));
           _image.sprite = cgs[cgIndex];
        }
    }
}
