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
        
        public AudioClip forestMusic;
    
        void Start()
        {
            CutsceneManager.instance.director.stopped += OnCutsceneStopped;
            
            if(!SaveManager.instance.ShouldExist("TitleCard")) AudioManager.instance.PlayMusic(forestMusic, 0.5f);

            if (!SaveManager.instance.ShouldExist("LevelZeroStartingCutscene")) return;
            DialogueManager.instance.StartDialogue(startingDialogue);
            SaveManager.instance.ChangeFlag("LevelZeroStartingCutscene", false);

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
