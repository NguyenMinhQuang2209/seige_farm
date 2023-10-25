using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactible
{
    [SerializeField] private int price = 0;
    private List<ChestCollectItem> collections = new();
    bool open = false;
    private Animator animator;
    private ChestCollectItem collectItem;
    [SerializeField] private float offsetY = 1f;
    private void Start()
    {
        collections = ChestController.instance.GetCollections(price);
        animator = GetComponent<Animator>();
    }
    protected override void Interact()
    {
        if (open)
        {
            return;
        }
        if (collections.Count == 0)
        {
            collections = ChestController.instance.GetCollections(price);
        }
        collectItem = GetCollectItem();
        if (animator != null)
        {
            animator.SetTrigger("Open");
            Destroy(gameObject, 5f);
        }
        open = true;
    }
    private ChestCollectItem GetCollectItem()
    {
        int randomPos = Random.Range(0, collections.Count);
        return collections[randomPos];
    }
    public void SpawnOjbect()
    {
        int randomQuantity = Random.Range(1, collectItem.quantity + 1);
        GameObject bag = Instantiate(PreferenceController.instance.present, transform.position + Vector3.up * offsetY, Quaternion.identity);
        List<CollectItem> newListCollect = new()
        {
            new(collectItem.itemType, collectItem.ItemName, randomQuantity)
        };
        if (bag.TryGetComponent<BagCollect>(out BagCollect bagCollect))
        {
            bagCollect.SpawnObject(newListCollect, 0f);
        }
    }
}
