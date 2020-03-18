﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    Rigidbody RB;
    [SerializeField]
    eItemLocation location = eItemLocation.shelf;

    public delegate void FellOffShelf(Item item);
    public event FellOffShelf Unshelved;

    public static event Action<Item, eCartUpdate> UpdateCart = delegate { };

    private void Awake()
    {
        RB = GetComponentInChildren<Rigidbody>();      
    }

    public virtual void ThrowItem(Vector3 vel)
    {
        RB.isKinematic = false;
        RB.velocity = (vel);
    }

    public eItemLocation currentLocation()
    {
        return location;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (location != eItemLocation.ground)
        {
            if (HitTheFloor(other))
            {
                Debug.Log(this.gameObject.name + " fell off the cart");
                location = eItemLocation.ground;
                UpdateCart(this, eCartUpdate.remove);
                return;
            }
        }

        if (HitTheCart(other) && location != eItemLocation.cart)
        {
            Debug.Log(this.gameObject.name + " fell into the cart");
            if (location == eItemLocation.shelf)
                UnshelfItem();

            location = eItemLocation.cart;
            UpdateCart(this, eCartUpdate.add);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LeftTheCart(other))
        {
            Debug.Log(this.gameObject.name + " fell off the cart");
            location = eItemLocation.ground;
            UpdateCart(this, eCartUpdate.remove);
            return;
        }

        if (LeftTheShelf(other))
        {
            Debug.Log(this.gameObject.name + " fell off the shelf");
            UnshelfItem();
            location = eItemLocation.ground;
        }
    }

    bool LeftTheShelf(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Shelf>())
            return true;

        return false;
    }

    bool LeftTheCart(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Cart>())
            return true;

        return false;
    }

    bool HitTheCart(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Cart>())
            return true;

        return false;
    }

    bool HitTheFloor(Collider other)
    {
        if (!other.gameObject.layer.Equals(8))
            return true;

        return false;
    }

    void UnshelfItem()
    {
        if (location == eItemLocation.shelf)
            Unshelved(this);
    }
}
