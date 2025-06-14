using DialogueFramework;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Intro
{
    public class CgImage : MonoBehaviour
    {
        private Image _image;
        private Color _unblackColor;
        [SerializeField] private Sprite[] cgs;
        [SerializeField] private Sprite cg0;
        [SerializeField] private Sprite cg1;
        [SerializeField] private Sprite cg2;
        [SerializeField] private Sprite cg3;
        [SerializeField] private Sprite cg4;
        [SerializeField] private Sprite cg5;
        [SerializeField] private Sprite cg6;
        [SerializeField] private Sprite cg7;
        [SerializeField] private Sprite cg8;
        [SerializeField] private Sprite cg9;
        [SerializeField] private Sprite cg10;
        [SerializeField] private Sprite cg11;
        [SerializeField] private Sprite cg12;
        private void Start()
        {
            _image = GetComponent<Image>();
            DialogueSystem.onDialogueNextLine += OnDialogueNextLine;
        
        }
    
        private void OnDialogueNextLine(string eventName)
        {
        
            switch (eventName)
            {
                case "cg0":
                    _image.sprite = cg0;
                    _image.color = Color.white;
                    break;
                case "cg1":
                    _image.sprite = cg1;
                    _image.color = Color.white;
                    break;
                case "cg2":
                    _image.sprite = cg2;
                    _image.color = Color.white;
                    break;
                case "cg3":
                    _image.sprite = cg3;
                    _image.color = Color.white;
                    break;
                case "cg4":
                    _image.sprite = cg4;
                    _image.color = Color.white;
                    break;
                case "cg5":
                    _image.sprite = cg5;
                    _image.color = Color.white;
                    break;
                case "cg6":
                    _image.sprite = cg6;
                    _image.color = Color.white;
                    break;
                case "cg7":
                    _image.sprite = cg7;
                    _image.color = Color.white;
                    break;
                case "cg8":
                    _image.sprite = cg8;
                    _image.color = Color.white;
                    break;
                case "cg9":
                    _image.sprite = cg9;
                    _image.color = Color.white;
                    break;
                case "cg10":
                    _image.sprite = cg10;
                    _image.color = Color.white;
                    break;
                case "cg11":
                    _image.sprite = cg11;
                    _image.color = Color.white;
                    break;
                case "cg12":
                    _image.sprite = cg12;
                    _image.color = Color.white;
                    break;
                default:
                    _image.sprite = cg0;
                    break;
            }

            if (eventName.ToLower().Contains("black"))
            {
                _unblackColor = _image.color;
                _image.color = Color.black;
                _image.sprite = cg0;
            }
            if(eventName.ToLower().Contains("stopmusic")) AudioManager.instance.StopMusic();

        }
    }
}
