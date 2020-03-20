using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    [SerializeField]
    Item thisItem;

    public static event Action<Item, eItemLocation> ItemLocationChange;
    void Awake()
    {
        thisItem = GetComponentInParent<Item>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(9) && ItemManager.Instance.curLocation(thisItem) != eItemLocation.cart)
        {
            ItemLocationChange(thisItem, eItemLocation.cart);
            //print(thisItem.name + " entered cart trigger");
        }
        else if (other.gameObject.layer.Equals(12) && ItemManager.Instance.curLocation(thisItem) != eItemLocation.ground)
        {
            ItemLocationChange(thisItem, eItemLocation.ground);
            //print(thisItem.name + " entered ground trigger");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer.Equals(10) && ItemManager.Instance.curLocation(thisItem) != eItemLocation.shelf)
        {
            ItemLocationChange(thisItem, eItemLocation.shelf);
            //print(thisItem.name + "is now on the shelf");
        }
        else if (other.gameObject.layer.Equals(8) && ItemManager.Instance.curLocation(thisItem) != eItemLocation.ground)
        {
            ItemLocationChange(thisItem, eItemLocation.ground);
            //print(thisItem.name + "is now on the ground");
        }
    }
}
