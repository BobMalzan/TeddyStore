using UnityEngine;

public class CounterTopManager : MonoBehaviour
{
    public void AddItem(GameObject originalItemGO)
    {
        ShopDatabase.ShopItem item = originalItemGO.GetComponent<ShopDatabase.ShopItem>();
        originalItemGO.SetActive(false);


    }
}
