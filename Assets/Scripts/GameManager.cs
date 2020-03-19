using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }
    public eGameState CurGameState { get; private set; } = eGameState.loading;

    public static event Action OnGameStateChange = delegate { };

    private void Start()
    {
        StartCoroutine(WaitForLoad());
    }

    public IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(1.0f);
        ChangeGameState(eGameState.running);
    }

    public void ChangeGameState(eGameState newGameState)
    {
        CurGameState = newGameState;
        OnGameStateChange();
    }

}
