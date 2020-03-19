using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }
    public eGameState CurGameState { get; private set; } = eGameState.loading;
    //true: items from prefab list spawn randomized on the shelfs
    public bool RandomizedSpawn;
    //true: Shopping list only asks for items without specific number
    public bool NoCountInShoppingList { get; private set; }

    public static event Action OnGameStateChange = delegate { };
    private void Awake()
    {
        NoCountInShoppingList = true;        
    }

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
