using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutTrigger : MonoBehaviour
{
    public static event System.Action<eCutscene> OnCheckOut = delegate { };
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(11) && GameManager.Instance.CurGameState == eGameState.running)
        {
            print("player hit the checkout");
            OnCheckOut(eCutscene.checkout);
        }
    }
}
