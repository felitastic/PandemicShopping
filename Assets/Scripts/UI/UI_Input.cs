using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Input : MonoBehaviour
{
    GameManager GM { get { return GameManager.Instance; } }

    public static event Action<eUI_Input> OnUI_Input = delegate { };
    public static event Action<bool> ShoppingListVisibel = delegate { };
    void Update()
    {
        //inputs than can only be pressed during active gameplay
        if (GM.CurGameState == eGameState.running)
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
        if (GM.CurGameState != eGameState.loading || GM.CurGameState != eGameState.cutscene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("pressed Escape");
                OnUI_Input(eUI_Input.pause);
            }
        }
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);       
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
        
    //public void ChangedToggle(Toggle thisToggle, int whichBool)
    //{
    //    switch (whichBool)
    //    {
    //        case 1:
    //            //value must be reversed cause I am dumb-dumb
    //            ToggleClicked(eModeBool.apocalypse, !thisToggle.isOn);
    //            break;
    //        case 2:
    //            ToggleClicked(eModeBool.randomSpawn, thisToggle.isOn);
    //            break;
    //        case 3:
    //            ToggleClicked(eModeBool.fixedAmount, thisToggle.isOn);
    //            break;            
    //        default:
    //            print("gamemode bool no" + whichBool + " has not been set");
    //            break;
    //    }
    //}
}
