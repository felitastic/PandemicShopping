using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Input : MonoBehaviour
{
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

        //inputs that always except in loading (nothing should work then) and in cutscenes
        if (GameManager.Instance.CurGameState != eGameState.loading || GameManager.Instance.CurGameState != eGameState.cutscene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("pressed Escape");
                OnUI_Input(eUI_Input.pause);
            }
        }
    }

    public void ExitGameButton()
    {
        OnUI_Input(eUI_Input.exit);
    }

    public void MainMenuButton()
    {
        OnUI_Input(eUI_Input.toTitle);
    }

    public void ChooseGameMode()
    {
        OnUI_Input(eUI_Input.gameMode);
    }
}
