using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public delegate IEnumerator ClosingShopUI(float wait);
    public static event ClosingShopUI CloseShop;
    //public delegate IEnumerator FadeScreen(float wait, bool fadeOut);
    public static event System.Action<float, bool> CallScreenFade;

    public static event System.Action<bool> OnScoreCheck = delegate { };
    //public static event System.Action ClosingShopUI = delegate { };
    void Start()
    {
        CheckOutTrigger.OnCheckOut += CallCutscene;
        Timer.OnTimerEnd += CallCutscene;
    }    

    void CallCutscene(eCutscene cutScene)
    {
        GameManager.Instance.ChangeGameState(eGameState.cutscene);

        switch (cutScene)
        {
            case eCutscene.intro:

                break;
            case eCutscene.checkout:
                OnScoreCheck(true);

                break;
            case eCutscene.timeout:
                StartCoroutine(TimeRanOut());

                break;
            default:
                break;
        }
    }

    IEnumerator TimeRanOut()
    {
        //play sound
        CloseShop(0.75f);
        yield return new WaitForSeconds(0.5f);
        CallScreenFade(0.8f, true);
        yield return new WaitForSeconds(1.0f);
        //teleport player
        CallScreenFade(0.8f, false);
        yield return new WaitForSeconds(0.5f);
        OnScoreCheck(false);
    }
}
