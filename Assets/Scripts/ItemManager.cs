using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ItemManager : Singleton<ItemManager>
{
    protected ItemManager() { }

    public Dictionary<Item, eItemLocation> AllItems { get; private set; }
    public Dictionary<eItemType, int> AllItemTypes { get; private set; }
    public Dictionary<eItemType, int> RequiredItems { get; private set; }

    private void Awake()
    {
        AllItems = new Dictionary<Item, eItemLocation>();
        AllItemTypes = new Dictionary<eItemType, int>();
        RequiredItems = new Dictionary<eItemType, int>();
    }
    private void Start()
    {
        ItemSpawn.OnItemCreation += AddToDictionary; 
        ItemSpawn.OnItemCreation += SortAllItemsByType;
        ItemCollider.ItemLocationChange += ChangeLocation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (KeyValuePair<eItemType, int> pair in AllItemTypes)
            {
                print(pair.Value + "x " + pair.Key);
            }
            if (AllItemTypes.Count == 0)
                print("No entries in AllItemTypes dic");

            //List<Item> items = ItemsOnShelfList(0);
            //if (items.Count == 0)
            //{
            //    print("no items here");
            //}
            //else
            //{
            //    //foreach (Item i in items)
            //    print(items.Count + " in the shelf");
            //}
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            List<Item> cart = AllItemsInCartList();
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
        AllItems.Add(newItem, eItemLocation.shelf);
    }

    public void ChangeLocation(Item item, eItemLocation newLocation)
    {
        AllItems[item] = newLocation;  
    }

    public void SortAllItemsByType(Item item)
    {
        if (AllItemTypes.ContainsKey(item.Type))
        {
            AllItemTypes[item.Type] += 1;
            return;
        }

        AllItemTypes.Add(item.Type, +1);
    }

    public void GetRandomShoppingList()
    {

    }

    public List<Item> AllItemsInCartList()
    {
        List<Item> cartItems = (from i in AllItems
                                where i.Value == eItemLocation.cart
                                select i.Key).ToList();
        return cartItems;
    }

    public List<Item> ItemsOnShelfList(int shelfID)
    {
        List<Item> shelfItems = (from i in AllItems
                                 where i.Value == eItemLocation.shelf && i.Key.ShelfID == shelfID
                                 select i.Key).ToList();        
        return shelfItems;
    }
}
