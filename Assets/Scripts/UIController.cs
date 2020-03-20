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
    bool shoppingListVisible;
    [SerializeField]
    GameObject[] shopListStrikethrough;
    
    private void Start()
    {
        shoppingCart = GetComponent<ShoppingCart>();
        shoppingCart.CreateShoppingList += UpdateShoppingList;
        shoppingCart.StrikeItems += StrikeItems;
        UI_Input.PlayerInput += GetPlayerInput;
        shoppingListVisible = false;
    }

    void GetPlayerInput(eKeys pressedKey)
    {
        switch (pressedKey)
        {
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

        ToggleMenu((int)eKeys.pause);
        eGameState newGameState = GameManager.Instance.CurGameState == eGameState.running ? eGameState.paused : eGameState.running;
        GameManager.Instance.ChangeGameState(newGameState);
    }

    void ToggleMenu(int menu)
    {
        bool active = !MenuUI[menu].activeSelf;
        MenuUI[menu].SetActive(active);
    }

    void ToggleShoppingList()
    {
        string trigger = shoppingListVisible ? "disable" : "enable";
        shoppingListVisible = !shoppingListVisible;
         shoppingListAnim.SetTrigger(trigger);
    }

    void UpdateShoppingList(string[] _newList)
    {
        string list = "";
        foreach (string s in _newList)
            list += s + "\n";

        ShoppingListText.text = list;
    }

    void StrikeItems(Dictionary<int, bool> _itemsToStrike)
    {
        if (!shoppingListVisible)
            ToggleShoppingList();

        StartCoroutine(ShowStriking(_itemsToStrike));        
    }

    IEnumerator ShowStriking(Dictionary<int, bool> _itemsToStrike)
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var pair in _itemsToStrike)
        {
            shopListStrikethrough[pair.Key].SetActive(pair.Value);
            yield return new WaitForSeconds(0.75f);
            //print(pair.Key + ". Item on list set to " + shopListStrikethrough[pair.Key].activeSelf);           
        }

        yield return new WaitForSeconds(1.5f);

        if(shoppingListVisible)
            ToggleShoppingList();
    }
}
