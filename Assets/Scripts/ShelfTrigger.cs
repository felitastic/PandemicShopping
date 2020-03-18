using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfTrigger : MonoBehaviour
{
    [SerializeField]
    eShelfSide side;
    [SerializeField]
    bool justHit;

    public event Action<eShelfSide, float> HitByCart;
    private void OnTriggerEnter(Collider other)
    {
        justHit = true;
        Movement player = other.GetComponentInParent<Movement>() ? other.GetComponentInParent<Movement>() : null;
        float speed = player.GetSpeed();

        if (player != null && speed > 0.25f)
        {
            Debug.Log(this.gameObject.name + " was hit by the cart at speed "+speed);
            HitByCart(side, speed);
            justHit = false;
        }
    }
}
