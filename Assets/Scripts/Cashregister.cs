using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cashregister : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CheckOutTrigger.OnCheckOut += GameResult;
    }
    void GameResult()
    {
        print("player is paying for dem stuff");
        GameManager.Instance.ChangeGameState(eGameState.cutscene);



        //check what items are in cart
        //check if you fulfilled the quest
        //check the timer
        //calculate score
        //again (choose gamemode) or quit option
    }
}
