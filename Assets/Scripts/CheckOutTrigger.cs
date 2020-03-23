using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutTrigger : MonoBehaviour
{
    public static event System.Action OnCheckOut = delegate { };
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer.Equals(11))
        {
            OnCheckOut();
        }
    }
}
