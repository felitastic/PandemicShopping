using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }
    [SerializeField]
    private PlayerSettings save;
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

    bool LoadPlayerSettings()
    {
        if (save == null)
            return false;

        RandomizedSpawn = save.RandomizedSpawn;
        FixedAmountInShopList = save.FixedAmountInShopList;
        FullShelves = save.FullShelves;
        return true;
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
