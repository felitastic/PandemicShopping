using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/SaveData")]
public class PlayerSettings : ScriptableObject
{
    public bool RandomizedSpawn;
    public bool FixedAmountInShopList;
    public bool FullShelves;
}
