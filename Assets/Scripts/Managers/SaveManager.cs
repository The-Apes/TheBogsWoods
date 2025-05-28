
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        public void LoadSave()
        {
            
        }
    }
}
