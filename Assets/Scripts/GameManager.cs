using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }
    [SerializeField]
    private PlayerSettings save;

    public int ShoppingTimer { get; private set; }
    public eGameState CurGameState { get; private set; } = eGameState.loading;
    //true: items from prefab list spawn randomized on the shelfs
    public bool RandomizedSpawn;
    //true: Shopping list only asks for items without specific number
    public bool FixedAmountInShopList { get; private set; }
    //true: all shelves are fully filled
    public bool FullShelves { get; private set; }
    public int AvailablePrefabCount { get; private set; }

    public static event Action OnGameStateChange = delegate { };
    private void Awake()
    {
        if (!LoadPlayerSettings())
            print("Could not load player settings, using default");

        AvailablePrefabCount = FindObjectOfType<ItemSpawn>().PrefabCount();
        print("Game Settings\n" +
            "Spawn items random: " + RandomizedSpawn + "\n" +
            "Fixed Amount of items in Shopping list: " + FixedAmountInShopList + "\n" +
            "Shelves fully stocked: " + FullShelves);
    }
    private void Start()
    {
        StartCoroutine(WaitForLoad());
    }


    public void SaveGameMode(eModeBool changeBool, bool isOn)
    {
           
    }

    bool LoadPlayerSettings()
    {
        if (save == null)
            return false;

        RandomizedSpawn = save.RandomizedSpawn;
        FixedAmountInShopList = save.FixedAmountInShopList;
        FullShelves = save.FullShelves;
        ShoppingTimer = save.ShoppingTimer;
        return true;
    }

    public IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(1.0f);
        ChangeGameState(eGameState.running);
    }

    public void ChangeGameState(eGameState newGameState)
    {
        CurGameState = newGameState;
        //print("cur GameState: " + CurGameState);
        OnGameStateChange();
    }
}
