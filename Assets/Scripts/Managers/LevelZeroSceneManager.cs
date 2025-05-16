using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Managers
{
    public class LevelZeroSceneManager : MonoBehaviour
    {
        public DialogueAsset startingDialogue;
        public DialogueAsset encounterDialogue;
        public PlayableAsset startingCutscene;
    
        void Start()
        {
            RuriMovement.instance.controlling = false;
            CutsceneManager.instance.director.stopped += OnCutsceneStopped;
            CutsceneManager.instance.PlayCutscene(startingCutscene);
        }
        

        private void OnCutsceneStopped(PlayableDirector director)
        {
            switch (director.playableAsset.name)
            {
                case "OttoRun":
                    DialogueManager.instance.StartDialogue(startingDialogue.dialogue);
                    break;
                case "Monkey Encounter":
                    DialogueManager.instance.StartDialogue(encounterDialogue.dialogue);
                    break;
            }
        }
        
 
    }
}
