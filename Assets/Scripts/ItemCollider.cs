using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    [SerializeField]
    Item thisItem;
    [SerializeField]
    Movement shoppingCart;

    public static event Action ItemLocationChange;
    void Start()
    {
        thisItem = GetComponent<Item>();
        shoppingCart = FindObjectOfType<Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
            //print(thisItem.name + "entered a trigger");
        if (other.GetComponentInParent<Movement>())
        {
            ItemManager.Instance.ChangeValue(thisItem, eItemLocation.cart);
            ItemLocationChange();
            print(thisItem.name + "is now in the cart");
        }

        if (other.gameObject.layer.Equals(8))
        {
            ItemManager.Instance.ChangeValue(thisItem, eItemLocation.ground);
            ItemLocationChange();
            print(thisItem.name + "is now on the ground");
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    //print(thisItem.name + "exited a trigger");
    //    if (other.GetComponentInParent<Movement>() || other.GetComponentInParent<ShelfTrigger>())
    //    {
    //        ItemManager.Instance.ChangeValue(thisItem, eItemLocation.ground);
    //        print(thisItem.name + "is now on the ground");
    //    }
    //}
}
