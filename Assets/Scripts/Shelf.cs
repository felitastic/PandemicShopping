using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    //[SerializeField]
    //Text itemList;
    [SerializeField]
    float minPushMulti = 0.5f, maxPushMulti = 1.0f;
    [SerializeField]
    int minPushNo = 4, maxPushNo = 10;
    [SerializeField]
    private float minPushForce = 1.5f;
    [SerializeField]
    ShelfTrigger[] shelfTrigger;
    [SerializeField]
    ItemList shelfList;
    //[SerializeField]
    //List<Item> shelvedItems = new List<Item>();
    
    private object mutex = new object();

    private void Start()
    {
        shelfList.WriteList(UpdateShelfList());
        Debug.Log("Items in shelf: " + shelfList.Count());
        
        foreach(ShelfTrigger trigger in shelfTrigger)
            trigger.HitByCart += PushItems;

        foreach (Item item in shelfList.Items)
            item.Unshelved += shelfList.RemoveFromList;
    }

    List<Item> UpdateShelfList()
    {        
        List<Item> _tempList = new List<Item>();
        _tempList.AddRange(GetComponentsInChildren<Item>());
        return _tempList;
    }

    List<Item> UpdateShelfList(Item itemToRemove)
    {
        lock(mutex)
        {
            List<Item> _tempList = new List<Item>();
            _tempList.AddRange(shelfList.Get());
            _tempList.Remove(itemToRemove);
            return _tempList;
        }
    }

    Item GetRandomItemOnShelf()
    {
        int _randItem = Random.Range(0, shelfList.Count() - 1);
        return shelfList.Items[_randItem];
    }

    string ShelfItemList()
    {
        string _string = "";
        foreach (Item item in shelfList.Items)
            _string += item.gameObject.name + "\n";

        return _string;
    }

    //check which side and tell item where "front" is
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetComponentInParent<Movement>())
    //    {
    //        if (shelfList.Count() > 0)
    //            PushItems(10);
    //        else
    //            Debug.Log("No items on shelf");
    //    }
    //}

    private void PushItems(eShelfSide side, float impact)
    {
        int pushedItemNo = ItemNoToPush();

        if (pushedItemNo > shelfList.Count())
            pushedItemNo = shelfList.Count();

        for (int i = 0; i < pushedItemNo; i++)
        {
            GetRandomItemOnShelf().ThrowItem(pushVector(side, impact));
        }
    }

    int ItemNoToPush()
    {
        int _pushNo = Random.Range(minPushNo, maxPushNo);
        int curItemCount = shelfList.Count();
        return _pushNo >= curItemCount ? curItemCount - 1 : _pushNo;
    }

    Vector3 pushVector(eShelfSide side, float force)
    {
        Vector3 _pushVector = Vector3.zero;
        float _pushForce = force * Random.Range(maxPushMulti, minPushMulti);

        if (_pushForce < minPushForce)
            _pushForce = Random.Range(minPushForce, minPushForce+0.5f);

        //float _pushForce = Random.Range(pushStart, pushEnd);
        switch (side)
        {
            case eShelfSide.front:
                _pushVector = new Vector3(0, _pushForce, -_pushForce);

                break;
            case eShelfSide.back:
                _pushVector = new Vector3(0, _pushForce, _pushForce);

                break;
            case eShelfSide.side:
                int randomizer = Random.Range(0, 1) > 0 ? 1 : -1;
                _pushVector = new Vector3(0, _pushForce, _pushForce * randomizer);

                break;
            default:
                Debug.Log("This shelf side is not known to me!");
                break;
        }
        return _pushVector;
    }

    //private void RemoveFromShelf(Item itemToRemove)
    //{
    //    Debug.Log("To remove " + itemToRemove.gameObject.name + " from shelf list");

    //    if (!shelvedItems.Contains(itemToRemove))
    //        return;

    //    shelvedItems = UpdateShelfList(itemToRemove);
    //    Debug.Log("Removed " + itemToRemove.gameObject.name + " from shelf list");
    //    //itemList.text = ShelfItemList();        
    //}


}
