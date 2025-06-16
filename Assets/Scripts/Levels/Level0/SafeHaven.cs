using System.Collections;
using DialogueFramework;
using HeightObjects;
using Managers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels.Level0
{
    public class SafeHaven : MonoBehaviour
    {
        [SerializeField] private Dialogue[] casualDialogues;
        [SerializeField] private Dialogue[] hurtDialogues;
        
        [SerializeField] private GameObject fairy;
        [SerializeField] private GameObject healthPotionPrefab;
        
        private Animator _animator;

        private bool _talkCooldown = false;
        private bool isMissingHealth = false;
        //private bool appeared = false;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            fairy.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            StartCoroutine(FairyAppear());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            StartCoroutine(FairyDisappear());
        }

        private IEnumerator FairyAppear()
        {
            RuriMovement.instance.RemoveStar();
            yield return new WaitForSeconds(1.5f);
            fairy.SetActive(true);
            _animator.SetTrigger("Appear");
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
            SaveManager.instance.SaveGame();
            print(PotionsInRange());
            StartCoroutine(SpawnPotions());
            Talk();
            
            yield return new WaitForSeconds(1f);

            
        }
        private IEnumerator FairyDisappear()
        {
            _animator.SetTrigger("Disappear");
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
            fairy.SetActive(false);
            RuriMovement.instance.AddStar();
            
        }

        private IEnumerator SpawnPotions()
        {
            var playerHealth = RuriMovement.instance.GetComponent<PlayerHealth>();
            int missingHealth = playerHealth.maxHealth - playerHealth.currentHealth - PotionsInRange();
            isMissingHealth = (missingHealth > 0);
            if (!isMissingHealth) yield break; 
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < missingHealth; i++)
            {
                var pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y+1);
                var potion = Instantiate(healthPotionPrefab);
                potion.transform.position = pos;
                potion.GetComponent<FakeHeightObject>().Initialize(new Vector2(Random.Range(-3f,3f),Random.Range(-3f,3f)), Random.Range(2,10));
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void Talk()
        {
            if (isMissingHealth)
            {
                DialogueManager.instance.StartDialogue(hurtDialogues[Random.Range(0, hurtDialogues.Length)]);
            }else if (!_talkCooldown)
            {
                DialogueManager.instance.StartDialogue(casualDialogues[Random.Range(0, casualDialogues.Length)]);
                StartCoroutine(CoolDown());
            }
        }
        private IEnumerator CoolDown()
        {
            _talkCooldown = true;
            yield return new WaitForSeconds(120f);
            _talkCooldown = false;
        }
        public int PotionsInRange()
        {
            var circle = GetComponent<CircleCollider2D>();

            Vector2 center = (Vector2)transform.position + circle.offset;
            float radius = circle.radius * Mathf.Abs(transform.lossyScale.x); // Handles scaling

            var hits = Physics2D.OverlapCircleAll(center, radius);
            int potionCount = 0;
            foreach (var hit in hits)
            {
                if (hit.CompareTag("HealthPotion"))
                {
                    potionCount++;
                }
            }
            return potionCount/3;
        }
    }
}
