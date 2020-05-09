using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{    
    public float TimeInMinutes { get { return GameManager.Instance.ShoppingTimer; } }
    private float shoppingTimeInSec;
    private bool runTimer;

    public static event System.Action<string> UpdateTimer = delegate { };
    public static event System.Action<eCutscene> OnTimerEnd = delegate { };

    private void Start()
    {
        shoppingTimeInSec = TimeInMinutes * 60.0f;
    }

    void Update()
    {
        if (GameManager.Instance.CurGameState == eGameState.running)
        {
            ShopTimer();
        }
    }

    private void ShopTimer()
    {
        shoppingTimeInSec -= Time.deltaTime;
        UpdateTimer(ConvertTimeToText(shoppingTimeInSec));

        if (shoppingTimeInSec <= 0.0f)
        {
            GameManager.Instance.ChangeGameState(eGameState.cutscene);
            OnTimerEnd(eCutscene.timeout);
        }
    }

    private string ConvertTimeToText(float curTime)
    {
        int curMin = Mathf.FloorToInt(curTime / 60F);
        int curSec = Mathf.FloorToInt(curTime - curMin * 60);

        if (curSec >= 10)
        {
            return curMin >= 10 ? (curMin + ":" + curSec) : ("0" + curMin + ":" + curSec);
        }
        else
        {
            return curMin >= 10 ? (curMin + ":0" + curSec) : ("0" + curMin + ":0" + curSec);          
        }
    }
}
