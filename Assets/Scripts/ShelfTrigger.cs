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
    public static event Action HitAnyShelf = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (justHit)
            return;

        //Debug.Log(this.gameObject.name + " was hit by " + other.gameObject.name);
        float speed = cart.GetSpeed();

        if (other.gameObject.layer.Equals(11) && speed > 1.0f)
        {
            //Debug.Log(this.gameObject.name + " was hit by the cart at speed " + speed);
            justHit = true;
            HitByCart(speed);
            HitAnyShelf();
            StartCoroutine(HitDelay());
        }        
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.2f);
        justHit = false;
    }
}
