using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject itemPrefab; // The item that will be spawned when the chest is opened
    public Sprite openedSprite; // The sprite to use when the chest is opened
    public DialogueTrigger openChest;
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);

        // Drop the item
        if (itemPrefab)
        {
            GameObject item = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(Random.Range(-1f, 1f), 2f), ForceMode2D.Impulse); // Apply bounce force
            }
        }
    }

    public void SetOpened(bool opened)
    {
        if (IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            DialogueManager.Instance.StartDialogue(openChest.dialogue);
        }
    }
}