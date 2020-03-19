using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CartInventory;
    [SerializeField]
    GameObject[] MenuToOpen;
    [SerializeField]
    Animator shoppingListAnim;
    [SerializeField]
    bool shoppingListVisible = false;

    private void Start()
    {
        UI_Input.PlayerInput += GetPlayerInput;
        ItemCollider.ItemLocationChange += UpdateCartInventory;
    }

    void GetPlayerInput(eKeys pressedKey)
    {
        switch (pressedKey)
        {
            case eKeys.none:
                break;
            case eKeys.pause:
                PauseGame();
                break;
            case eKeys.exit:
                break;
            case eKeys.shoppinglist:
                ToggleShoppingList();
                break;
        }
    }

    void PauseGame()
    {
        if (GameManager.Instance.CurGameState != eGameState.running && GameManager.Instance.CurGameState != eGameState.paused)
            return;

        eGameState newGameState = GameManager.Instance.CurGameState == eGameState.running ? eGameState.paused : eGameState.running;
        GameManager.Instance.ChangeGameState(newGameState);
    }

    void ToggleShoppingList()
    {
        string trigger = shoppingListVisible ? "disable" : "enable";
        shoppingListVisible = !shoppingListVisible;
         shoppingListAnim.SetTrigger(trigger);
    }

    void UpdateCartInventory()
    {
        //CartInventory.text = "Items in cart:\n"+newInventory;
    }
}
