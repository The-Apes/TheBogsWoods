using UnityEngine;

namespace Levels.Level0
{
    public class Hedge : MonoBehaviour
    {
        [SerializeField] SpriteRenderer main;
        [SerializeField] SpriteRenderer sec;

        private void Awake()
        {
            sec.sprite = main.sprite;
        }
    }
}
