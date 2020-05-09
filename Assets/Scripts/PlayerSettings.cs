using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/SaveData")]
public class PlayerSettings : ScriptableObject
{
    [Tooltip("Items spawn random on the shelves")]
    public bool RandomizedSpawn;
    [Tooltip("Shopping list has fixed amount of items")]
    public bool FixedAmountInShopList;
    [Tooltip("Shelves are fully stocked")]
    public bool FullShelves;
    [Tooltip("Total time for the running session")]
    [Range(1, 7)]
    public int ShoppingTimer;
}
