using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObjects")]
public class ItemInfo : ScriptableObject
{
    [Tooltip("To sort by specific type")]
    public eItemType Type;
    [Tooltip("Name to print out")]
    public string ItemName;
    [Tooltip("Offset for spawning items next to each other")]
    public float SpawnOffset;
    [Tooltip("How many items fit in one shelf to fill it")]
    public int MaxNoInShelf;
}
