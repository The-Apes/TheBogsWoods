using Managers;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool _isOpened;
    private string _chestID;
    public GameObject itemPrefab; // The item that will be spawned when the chest is opened
    public Sprite openedSprite; // The sprite to use when the chest is opened
    public DialogueAsset openChest;

    private void Start()
    {
        _chestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !_isOpened;
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
        if (!itemPrefab) return;
        GameObject item = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), 2f), ForceMode2D.Impulse); // Apply bounce force
        }
    }

    private void SetOpened(bool opened)
    {
        if (_isOpened != opened) return;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        DialogueManager.instance.StartDialogue(openChest.dialogue);
    }
}