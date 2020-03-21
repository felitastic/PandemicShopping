using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    ShoppingCart shoppingCart;
    [SerializeField]
    TextMeshProUGUI ShoppingListText;
    [SerializeField]
    GameObject[] MenuUI;
    [SerializeField]
    Animator shoppingListAnim;
    [SerializeField]
    GameObject[] shopListStrikethrough;

    private void Start()
    {
        shoppingCart = GetComponent<ShoppingCart>();
        shoppingCart.CreateShoppingList += WriteShoppingListUI;
        shoppingCart.UpdateShoppingList += StrikeItems;
        UI_Input.OnUI_Input += GetUIButtonInput;
        UI_Input.ShoppingListVisibel += ShowShoppingList;
    }

    void GetUIButtonInput(eUI_Input ui_Input)
    {
        switch (ui_Input)
        {
            case eUI_Input.pause:
                PauseGame();
                break;
            case eUI_Input.exit:
                break;
        }
    }

    void PauseGame()
    {
        if (GameManager.Instance.CurGameState != eGameState.running && GameManager.Instance.CurGameState != eGameState.paused)
            return;

        ToggleMenu((int)eUI_Input.pause);
        eGameState newGameState = GameManager.Instance.CurGameState == eGameState.running ? eGameState.paused : eGameState.running;
        GameManager.Instance.ChangeGameState(newGameState);
    }

    void ToggleMenu(int menu)
    {
        bool active = !MenuUI[menu].activeSelf;
        MenuUI[menu].SetActive(active);
    }

    void EnableShoppingList()
    {
        shoppingListAnim.SetTrigger("enable");

    }

    void ShowShoppingList(bool show)
    {
        if (show)
            shoppingListAnim.SetBool("visible", true);        
        else
            shoppingListAnim.SetBool("visible", false);
    }

    void WriteShoppingListUI(string[] _newList)
    {
        string list = "";
        foreach (string s in _newList)
            list += s + "\n";

        ShoppingListText.text = list;
    }

    void StrikeItems(int whichRow, bool striked)
    {
        StartCoroutine(UpdateShoppingListUI(whichRow, striked));
    }

    IEnumerator UpdateShoppingListUI(int whichRow, bool striked)
    {
        ShowShoppingList(true);

        yield return new WaitForSeconds(0.75f);
        shopListStrikethrough[whichRow].SetActive(striked);
        yield return new WaitForSeconds(1.0f);

        ShowShoppingList(false);
    }
}
