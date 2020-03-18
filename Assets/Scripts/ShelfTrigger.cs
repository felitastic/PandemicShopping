using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfTrigger : MonoBehaviour
{
    [SerializeField]
    Movement cart;
    [SerializeField]
    bool justHit;

    private void Start()
    {
        cart = FindObjectOfType<Movement>();
    }

    public event Action<float> HitByCart = delegate { };
    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(this.gameObject.name + " was hit by " + other.gameObject.name);
        float speed = cart.GetSpeed();

        if (other.gameObject.GetComponentInParent<Movement>() && speed > 0.25f)
        {
            justHit = true;
            //Debug.Log(this.gameObject.name + " was hit by the cart at speed " + speed);
            HitByCart(speed);
        }
        justHit = false;
    }
}
