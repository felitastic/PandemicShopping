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
    int chosenItem;

    public static event Action<Item> OnItemCreation = delegate { };
    public static event Action ItemSpawnFinished = delegate { };

    private void Start()
    {
        FillShelf();
    }

    void FillShelf()
    {
        chosenItem = 0;

        foreach (Shelf shelf in allShelves)
        {
            Item _item;

            if (random)
            {
                _item = itemPrefab[UnityEngine.Random.Range(0, itemPrefab.Length)];
            }
            else
            {
                _item = chosenItem < itemPrefab.Length ? itemPrefab[chosenItem] : itemPrefab[UnityEngine.Random.Range(0, itemPrefab.Length)];
                chosenItem++;
            }

            //offset for the chosen item
            float _itemOffset = _item.SpawnOffset;

            //get the shelfID
            int _shelfID = shelf.ShelfID;

            //for as many times as this item fits into a shelf
            for (int _itemCount = 0; _itemCount < _item.MaxNoInShelf; _itemCount++)
            {
                //Vector3 newPos = SpawnPosition(shelf, _itemOffset, _itemCount);
                Item newItem = Instantiate(_item, shelf.transform);
                newItem.transform.localPosition = SpawnPosition(shelf, _itemOffset, _itemCount);
                newItem.ShelfID = shelf.ShelfID;
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
