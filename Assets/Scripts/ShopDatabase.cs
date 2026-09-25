using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopDatabase : MonoBehaviour
{
    [SerializeField]
    private List<ShopItem> m_Data = new List<ShopItem>();

    [Serializable]
    public class ShopItem
    {
        public uint ID;
        public string Name;
        public string ShortDescription;
        public string Description;
        public float Price;
    }

    public static ShopDatabase TheShop;
    public static List<ShopItem> ShopData;

    private void Awake()
    {
        TheShop = this;
        ShopData = m_Data;
    }

    void Start()
    {
        
    }

    public ShopItem FindItemById(uint id)
    {
        foreach (var item in m_Data)
            if (item.ID == id)
                return item;

        return null;
    }

    public void AddToCart(uint itemNumber)
    {
        ShopItem item = FindItemById(itemNumber);
        Debug.Log($"add {item.Name} (ID={item.ID}) to cart");
    }
}
