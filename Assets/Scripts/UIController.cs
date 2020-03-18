using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Cart shoppingCart;
    [SerializeField]
    UI_Buttons buttonControl;
    [SerializeField]
    Text CartInventory;
    [SerializeField]
    GameObject[] MenuToOpen;


    private void Start()
    {
        shoppingCart.UpdateCartList += UpdateCartInventory;
        buttonControl.ButtonInput += ButtonClicked;

    }

    void ButtonClicked(int whichButton)
    {
        switch(whichButton)
        {
            case 0:
                //MenuToOpen[whichButton].SetActive(true);

                break;
            case 1:
                Transform player = FindObjectOfType<Cart>().transform;
                player.gameObject.SetActive(false);
                player.transform.position = new Vector3(0f, 1.2f, -15f);
                player.gameObject.SetActive(true);

                break;
            default:
                break;
        }
    }

    void UpdateCartInventory(string newInventory)
    {
        CartInventory.text = "Items in cart:\n"+newInventory;
    }
}
