using System.Collections.Generic;
using UnityEngine;

public class ShopDatabase : MonoBehaviour
{
    public class ShopItem
    {
        public uint ID;
        public string Name;
        public string ShortDescription;
        public string Description;
    }
        
    private List<ShopItem> m_Data;

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
}
