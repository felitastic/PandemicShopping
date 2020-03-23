using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup fadeOutScreen;
    [SerializeField]
    Image fadeOutColor;
    [SerializeField]
    TextMeshProUGUI ShoppingListText;
    [SerializeField]
    Animator shoppingListAnim;
    [SerializeField]
    GameObject[] shopListStrikethrough;
    [SerializeField]
    GameObject[] MenuUI;

    ShoppingCart shoppingCart;
    GameManager GM;

    private void Start()
    {
        GM = GameManager.Instance;
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
                StartCoroutine(ExitGame());
                break;
            case eUI_Input.toTitle:
                StartCoroutine(GoToMainScreen());
                break;
        }
    }

    IEnumerator ExitGame()
    {
        GM.ChangeGameState(eGameState.loading);
        StartCoroutine(FadeOut(0.8f));
        yield return new WaitForSeconds(1.3f);
        print("Game quit");
        Application.Quit();
    }

    IEnumerator FadeOut(float timeInSeconds)
    {
        float wait = timeInSeconds / 100;
        while (fadeOutScreen.alpha < 1.0f)
        {
            fadeOutScreen.alpha += 0.01f;
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator FadeOut(float timeInSeconds, Color newColor)
    {
        float wait = timeInSeconds / 100;
        fadeOutColor.color = newColor;
        fadeOutScreen.blocksRaycasts = true;

        while (fadeOutScreen.alpha < 1.0f)
        {
            fadeOutScreen.alpha += 0.01f;
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator FadeIn(float timeInSeconds)
    {
        float wait = timeInSeconds / 60;
        while (fadeOutScreen.alpha >= 0.0f)
        {
            fadeOutScreen.alpha -= 0.01f;
            yield return new WaitForSeconds(wait);
        }
        fadeOutScreen.blocksRaycasts = false;
    }

    IEnumerator GoToMainScreen()
    {
        //TODO deactivate shop in background
        GM.ChangeGameState(eGameState.loading);
        StartCoroutine(FadeOut(0.8f));
        yield return new WaitForSeconds(1.1f);
        ChangeWindowStatus((int)eUIMenu.main, true);
        StartCoroutine(FadeIn(1.0f));
        yield return new WaitForSeconds(0.8f);
        GM.ChangeGameState(eGameState.running);
    }

    void PauseGame()
    {
        if (GameManager.Instance.CurGameState != eGameState.running && GameManager.Instance.CurGameState != eGameState.paused)
            return;

        ToggleWindowStatus((int)eUI_Input.pause);
        eGameState newGameState = GameManager.Instance.CurGameState == eGameState.running ? eGameState.paused : eGameState.running;
        GameManager.Instance.ChangeGameState(newGameState);
    }

    void ToggleWindowStatus(int menu)
    {
        bool active = !MenuUI[menu].activeSelf;
        MenuUI[menu].SetActive(active);
    }
    void ChangeWindowStatus(int menu, bool open)
    {
        MenuUI[menu].SetActive(open);
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
