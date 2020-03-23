using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{

    public static event System.Action OnScoreCheck = delegate { };
    void Start()
    {
        CheckOutTrigger.OnCheckOut += CallCutscene;
    }    

    void CallCutscene(int cutsceneNo)
    {
        GameManager.Instance.ChangeGameState(eGameState.cutscene);

        switch (cutsceneNo)
        {
            case 0:
                print("playing intro");

                
                break;

            case 1:
                print("checkout triggered");
                OnScoreCheck();
                //Stop timer

                //OnScoreCheck
                //check how many items from the list they got -> shopping cart
                //check how many items in general -> shopping cart

                //calculate score 

                //show receipt -> ui controller string geben

                //show score as price at the bottom

                break;
            default:
                break;
        }
    }

     

}
