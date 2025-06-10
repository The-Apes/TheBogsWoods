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
            
            CutsceneManager.instance.director.stopped += OnCutsceneStopped;

            if (!SaveManager.instance.ShouldExist("LevelZeroStartingCutscene")) return;
            SaveManager.instance.ChangeFlag("LevelZeroStartingCutscene", false);
            RuriMovement.instance.controlling = false;
            CutsceneManager.instance.PlayCutscene(startingCutscene);

        }
        

        private void OnCutsceneStopped(PlayableDirector director)
        {
            switch (director.playableAsset.name)
            {
                case "OttoRun":
                    DialogueManager.instance.StartDialogue(startingDialogue);
                    break;
                case "Monkey Encounter":
                    DialogueManager.instance.StartDialogue(encounterDialogue);
                    break;
            }
        }
    }
}
