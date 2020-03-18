using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField]
    ItemList cartList; 
    //List<Item> cartData.ItemList = new List<Item>();

    public event Action<string> UpdateCartList = delegate { };

    private void Awake()
    {
        //Item.UpdateCart += UpdateCart;
        cartList.ClearList();
    }

    //void UpdateCart(Item newItem, eCartUpdate updateType)
    //{
    //        switch (updateType)
    //        {
    //            case eCartUpdate.add:
    //                if (cartList.Contains(newItem))
    //                    return;

    //                cartList.AddToList(newItem);
    //                UpdateCartList(NewInventoryList());

    //                break;
    //            case eCartUpdate.remove:
    //                if (!cartList.Contains(newItem))
    //                    return;

    //                cartList.RemoveFromList(newItem);
    //                UpdateCartList(NewInventoryList());

    //                break;
    //            case eCartUpdate.checkout:

    //                break;
    //            default:
    //                break;
    //        }
        
    //}

    string NewInventoryList()
    {
        string _inventory = "";
        foreach (Item item in cartList.Items)
            _inventory += item.name + "\n";

        return _inventory;
    }
    //List<Item> AddItem(Item itemToAdd)
    //{
    //    List<Item> _tempList = new List<Item>();
    //    _tempList.AddRange(cartData.ItemList);
    //    _tempList.Add(itemToAdd);
    //    return _tempList;
    //}

    //List<Item> RemoveItem(Item itemToRemove)
    //{
    //    List<Item> _tempList = new List<Item>();
    //    _tempList.AddRange(cartData.ItemList);
    //    _tempList.Remove(itemToRemove);
    //    return _tempList;
    //}

}
