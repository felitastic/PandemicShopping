using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Input : MonoBehaviour
{
    public static event Action<eKeys> PlayerInput = delegate { };

    void Update()
    {
        //keys than can only be pressed during active gameplay
        if (GameManager.Instance.CurGameState == eGameState.running)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                print("pressed R");
                PlayerInput(eKeys.shoppinglist);
            }
        }

        //keys that also work in pause mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("pressed Escape");
            PlayerInput(eKeys.pause);
        }

    }
}
