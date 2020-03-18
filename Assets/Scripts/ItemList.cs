using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Itemlist", menuName = "ScriptableObjects")]
public class ItemList : ScriptableObject
{
    public List<Item> Items { get; private set; }

    private void OnDisable()
    {
        ClearList();        
    }

    public void WriteList(List<Item> newList)
    {
        ClearList();
        Items = newList;
    }
    public void ClearList()
    {
        Items.Clear();
    }

    public void AddToList(Item itemToAdd)
    {
        Items.Add(itemToAdd);
    }

    public void RemoveFromList(Item itemToRemove)
    {
        Items.Remove(itemToRemove);
    }

    public bool Contains(Item itemToSearchFor)
    {
        return Items.Contains(itemToSearchFor);
    }

    public int Count()
    {
        return Items.Count;
    }

    public List<Item> Get()
    {
        return Items;
    }
}
