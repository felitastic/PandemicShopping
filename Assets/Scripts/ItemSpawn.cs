using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField]
    Item[] itemPrefab;
    [SerializeField]
    Shelf[] allShelves;
    [SerializeField]
    bool random;
    int chosenItem;

    public static event Action<Item> OnItemCreation = delegate { };

    private void Start()
    {
        //if (SetShelfIDs())
            FillShelf();
    }

    //bool SetShelfIDs()
    //{
    //    for (int i = 0; i < allShelves.Length-1; i++)
    //    {
    //        allShelves[i].ShelfID = i;
    //    }
    //    return true;
    //}

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

            //get first spawnpoint values in the shelf
            //float[] _origSpawnPoint = OriginalSpawnPos(shelf);

            //for as many times as this item fits into a shelf
            for (int _itemCount = 0; _itemCount < _item.maxNoInShelf - 1; _itemCount++)
            {
                //Vector3 newPos = SpawnPosition(shelf, _itemOffset, _itemCount);
                Item newItem = Instantiate(_item, shelf.transform);
                newItem.transform.localPosition = SpawnPosition(shelf, _itemOffset, _itemCount);
                newItem.ShelfID = shelf.ShelfID;
                OnItemCreation(newItem);
            }
        }
    }

    Vector3 SpawnPosition(Shelf shelf, float offset, int itemCount)
    {
        //float[] spawnPoint = origSpawnPos;
        Vector3 newPos = Vector3.zero;

        switch (shelf.FaceDir)
        {
            case eShelfFaceDirection.down:
                newPos.x += offset * itemCount;
                break;
            case eShelfFaceDirection.right:
                newPos.z += offset * itemCount;
                break;
            case eShelfFaceDirection.up:
                newPos.x -= offset * itemCount;
                break;
            case eShelfFaceDirection.left:
                newPos.z -= offset * itemCount;
                break;
            default:
                break;
        }

        return newPos;
    }

}
