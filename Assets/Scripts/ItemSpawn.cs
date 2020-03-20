using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the items on the shelfs
/// </summary>
public class ItemSpawn : MonoBehaviour
{
    [SerializeField]
    Item[] itemPrefab;
    [SerializeField]
    Shelf[] allShelves;
    [SerializeField]
    bool random { get { return GameManager.Instance.RandomizedSpawn; } }
    bool fullShelves { get { return GameManager.Instance.FullShelves; } }
    int chosenItem;

    public static event Action<Item> OnItemCreation = delegate { };
    public static event Action ItemSpawnFinished = delegate { };

    private void Start()
    {
        FillShelf();
    }

    public int PrefabCount()
    {
        return itemPrefab.Length;
    }

    void FillShelf()
    {
        chosenItem = 0;

        foreach (Shelf shelf in allShelves)
        {
            Item itemToSpawn;
            int itemsInShelf;
            int shelfID = shelf.ShelfID;

            if (random)
            {
                itemToSpawn = itemPrefab[UnityEngine.Random.Range(0, itemPrefab.Length)];
            }
            else
            {
                itemToSpawn = itemPrefab[chosenItem];
                chosenItem = chosenItem >= itemPrefab.Length - 1 ? 0 : chosenItem + 1;
            }

            //offset for the chosen item
            float itemSpawnOffset = itemToSpawn.SpawnOffset;

            //set random no of items in shelf
            if (!fullShelves)
                itemsInShelf = UnityEngine.Random.Range(0, itemToSpawn.MaxNoInShelf);
            else
                itemsInShelf = itemToSpawn.MaxNoInShelf;

            //for as many items as there should be in this shelf
            for (int _itemCount = 0; _itemCount < itemsInShelf; _itemCount++)
            {
                Item newItem = Instantiate(itemToSpawn, shelf.transform);
                newItem.transform.localPosition = SpawnPosition(shelf, itemSpawnOffset, _itemCount);
                newItem.ShelfID = shelf.ShelfID;
                newItem.gameObject.name = newItem.Name + "_" + shelfID+"_No"+ _itemCount;
                OnItemCreation(newItem);
            }
        }
        ItemSpawnFinished();
    }

    Vector3 SpawnPosition(Shelf shelf, float offset, int itemCount)
    {
        Vector3 newPos = Vector3.zero;
        newPos.x += offset * itemCount;

        //switch (shelf.FaceDir)
        //{
        //    case eShelfFaceDirection.down:
        //        newPos.x += offset * itemCount;
        //        break;
        //    case eShelfFaceDirection.right:
        //        newPos.z += offset * itemCount;
        //        break;
        //    case eShelfFaceDirection.up:
        //        newPos.x -= offset * itemCount;
        //        break;
        //    case eShelfFaceDirection.left:
        //        newPos.z -= offset * itemCount;
        //        break;
        //    default:
        //        break;
        //}

        return newPos;
    }

}
