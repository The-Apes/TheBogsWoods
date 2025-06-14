using System.Collections;
using DialogueFramework;
using Player;
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
            CutsceneManager.instance.PlayCutscene(startingCutscene);
            RuriMovement.instance.controlling = false;
        }
        

        private void OnCutsceneStopped(PlayableDirector director)
        {
            switch (director.playableAsset.name)
            {
                case "OttoRun":
                    DialogueManager.instance.StartDialogue(startingDialogue);
                    SaveManager.instance.ChangeFlag("LevelZeroStartingCutscene", false);
                    break;
                case "Monkey Encounter":
                    DialogueManager.instance.StartDialogue(encounterDialogue);
                    break;
            }
        }
    }
}
