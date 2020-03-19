using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    int maxTypeEntries = 6;
    int maxCartCapacity = 8;

    public event Action<string> ShoppingListUpdated = delegate { };

    public string NewShoppingList()
    {
        string[] lines = new string[6];
        //fill the lines
        //add them to one string
        return "- Toiletpaper\n- Soup";
    }
}
