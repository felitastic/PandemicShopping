using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup ShopCloseMessage;
    [SerializeField]
    CanvasGroup fadeOutScreen;
    [SerializeField]
    Image fadeOutImage;
    [SerializeField]
    Animator shoppingListAnim;
    [SerializeField]
    GameObject[] shopListStrikethrough;
    [SerializeField]
    GameObject[] MenuUI;
    [SerializeField]
    TextMeshProUGUI[] ReceiptText = new TextMeshProUGUI[2];
    [SerializeField]
    TextMeshProUGUI ShoppingListText;
    [SerializeField]
    TextMeshProUGUI ShoppingTimer;

    ShoppingCart shoppingCart;
    GameManager GM;
    
    private void Start()
    {
        GM = GameManager.Instance;
        shoppingCart = GetComponent<ShoppingCart>();
        shoppingCart.CreateShoppingList += WriteShoppingListUI;
        shoppingCart.UpdateShoppingList += StrikeItems;
        shoppingCart.OnReceiptPrint += SetReceipt;
        UI_Input.OnUI_Input += GetUIButtonInput;
        UI_Input.ShoppingListVisibel += ShowShoppingList;
        Timer.UpdateTimer += SetTimer;
        CutsceneController.CloseShop += CloseShopMessage;
        CutsceneController.CallScreenFade += CallFade;
    }

    IEnumerator CloseShopMessage(float waitInSec)
    {
       float wait = waitInSec / 60;
       while (ShopCloseMessage.alpha >= 0.0f)
        {
            ShopCloseMessage.alpha -= 0.01f;
            yield return new WaitForSeconds(wait);
        }
    }

    void SetTimer(string newTime)
    {
        ShoppingTimer.text = newTime;
        //TODO change timer when it comes to the last seconds
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
            case eUI_Input.gameMode:

                break;
        }
    }
       
    void CallFade(float waitTime, bool fadeOut)
    {
        if (fadeOut)
            StartCoroutine(FadeOut(waitTime));
        else
            StartCoroutine(FadeIn(waitTime));
    }

    IEnumerator ChooseGamemode()
    {
        GM.ChangeGameState(eGameState.loading);
        StartCoroutine(FadeOut(0.8f));
        yield return new WaitForSeconds(1.0f);
        ChangeWindowStatus((int)eUIMenu.gamemode, true);
        StartCoroutine(FadeIn(0.8f));
        GM.ChangeGameState(eGameState.paused);
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
        float wait = timeInSeconds / 60;
        while (fadeOutScreen.alpha < 1.0f)
        {
            fadeOutScreen.alpha += 0.01f;
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator FadeOut(float timeInSeconds, Color newColor)
    {
        float wait = timeInSeconds / 60;
        fadeOutImage.color = newColor;
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

    void SetReceipt(int score, List<Item> purchasedItems, string percentage)
    {
        Queue<string> names = new Queue<string>();
        Queue<string> values = new Queue<string>();
        Queue<string> bottom = new Queue<string>();

        if (purchasedItems.Count == 0)
        {            
            bottom.Enqueue("\n\n" + "You were served by ");
            bottom.Enqueue("\nfelitastic & DasBilligeAlien");
            StartCoroutine(PrintReceipt(names, values, bottom));
            return;
        }

        foreach(Item item in purchasedItems)
        {
            names.Enqueue(item.Name + "\n");
            values.Enqueue(item.Value +".00\n");
        }
        names.Enqueue("\nTax:");
        values.Enqueue("\n+"+ percentage + "% ");
        names.Enqueue("\nTotal: ");

        values.Enqueue("\n"+score+".00");

        bottom.Enqueue("\n\n" + "You were served by ");
        bottom.Enqueue("\nfelitastic & DasBilligeAlien");

        StartCoroutine(PrintReceipt(names, values, bottom));
    }

    IEnumerator PrintReceipt(Queue<string> names, Queue<string> values, Queue<string> bottom)
    {    
        print("checking out");
        ReceiptText[0].text = "";
        ReceiptText[1].text = "";
        ChangeWindowStatus((int)eUIMenu.receipt, true);
        yield return new WaitForSeconds(0.7f);
        if (names.Count == 0 || values.Count == 0)
        {
            ReceiptText[0].text = "Pleasy buy something next time! :)";
            yield return new WaitForSeconds(0.4f);
        }
        else
        {
            int temp = names.Count;
            for (int i = 0; i < temp; i++)
            {
                ReceiptText[0].text += names.Dequeue();
                ReceiptText[1].text += values.Dequeue();
                yield return new WaitForSeconds(0.4f);
            }
        }

        print("bottoms up!");
        yield return new WaitForSeconds(0.4f);
        ReceiptText[0].text += bottom.Dequeue();
        yield return new WaitForSeconds(0.4f);
        ReceiptText[0].text += bottom.Dequeue();

        yield return new WaitForSeconds(0.5f);
        print("all items paid");
        //TODO: reveal pay button
    }
}
