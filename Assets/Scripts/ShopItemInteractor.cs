using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopItemInteractor : MonoBehaviour
{
    [SerializeField]
    private uint m_ItemNumber;

    private string m_Name;
    private string m_ShortDescription;
    private string m_Description;

    private Coroutine m_HoverCoroutine = null;

    private void Start()
    {
        ShopDatabase.ShopItem item=ShopDatabase.TheShop.FindItemById(m_ItemNumber);

        if (item == null)
            return;

        m_Name = item.Name;
        m_ShortDescription = item.ShortDescription;
        m_Description = item.Description;
    }

    public void OnHover()
    {
        m_HoverCoroutine = StartCoroutine(HoverCoroutine());
    }

    public void OnExitHover()
    {
        PopupHandler.Instance.OnClickAnywhere();

        if (m_HoverCoroutine != null)
        {
            StopCoroutine(m_HoverCoroutine);
        }    
    }

    IEnumerator HoverCoroutine()
    {
        Vector2 screenPos = Pointer.current.position.ReadValue();

        PopupHandler.Instance.ShowName(screenPos, "<b>" + m_Name + "</b>");

        yield return new WaitForSeconds(1.0f);
        PopupHandler.Instance.ShowShort(screenPos, "<b>" + m_Name + "</b>\n\n"
            + m_ShortDescription );

        yield return new WaitForSeconds(1.0f);
        PopupHandler.Instance.ShowFull(screenPos, "<b>" + m_Name + "</b>\n\n"
            + m_ShortDescription + "\n\n" 
            + "<i><size=70%>" + m_Description,
            () => 
            {
                ShopDatabase.TheShop.AddToCart(m_ItemNumber);
                PopupHandler.Instance.OnClickAnywhere();
            }
        );

        m_HoverCoroutine = null;
        yield break;
    }
}
