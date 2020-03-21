using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Input : MonoBehaviour
{
    [SerializeField]
    //bool buttonPressed;

    public static event Action<eUI_Input> OnUI_Input = delegate { };
    public static event Action<bool> ShoppingListVisibel = delegate { };
    void Update()
    {
        //inputs than can only be pressed during active gameplay
        if (GameManager.Instance.CurGameState == eGameState.running)
        {
            if (Input.GetButtonDown("ShoppingList"))
            {
                ShoppingListVisibel(true);
            }

            if (Input.GetButtonUp("ShoppingList"))
            {
                ShoppingListVisibel(false);
            }
        }

        //inputs that also work in pause mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("pressed Escape");
            OnUI_Input(eUI_Input.pause);
        }

    }

    //IEnumerator ButtonDelay()
    //{
    //    buttonPressed = true;
    //    yield return new WaitForSeconds(0.15f);
    //    buttonPressed = false;
    //}
}
