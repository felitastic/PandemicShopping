using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ItemManager : Singleton<ItemManager>
{
    protected ItemManager() { }

    public Dictionary<Item, eItemLocation> allItems = new Dictionary<Item, eItemLocation>();

    private void Start()
    {
        ItemSpawn.OnItemCreation += AddToDictionary;                
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            List<Item> items = ItemsOnShelfList(0);
            if (items.Count == 0)
            {
                print("no items here");
            }
            else
            {
                //foreach (Item i in items)
                print(items.Count + " in the shelf");
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            List<Item> cart = ItemsInCartList();
            if (cart.Count == 0)
            {
                print("no items in the cart");
            }
            else
            {
                //foreach (Item i in items)
                print(cart.Count + " in the cart");
            }
        }
    }

    private void AddToDictionary(Item newItem)
    {
        allItems.Add(newItem, eItemLocation.shelf);
    }

    public void ChangeValue(Item item, eItemLocation newLocation)
    {
        allItems[item] = newLocation;
    }

    public List<Item> ItemsInCartList()
    {
        List<Item> cartItems = (from i in allItems
                                where i.Value == eItemLocation.cart
                                select i.Key).ToList();
        return cartItems;
    }

    public List<Item> ItemsOnShelfList(int shelfID)
    {
        List<Item> shelfItems = (from i in allItems
                                 where i.Value == eItemLocation.shelf && i.Key.ShelfID == shelfID
                                 select i.Key).ToList();        
        return shelfItems;
    }
}
