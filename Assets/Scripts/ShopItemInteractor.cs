using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopItemInteractor : MonoBehaviour
{
    [SerializeField]
    private uint m_ItemNumber;

    [SerializeField]
    private float m_LookAtSpeed = 0.25f;
    
    [SerializeField]
    private GameObject m_MainCamera;

    private string m_Name;
    private string m_ShortDescription;
    private string m_Description;

    private Coroutine m_HoverCoroutine = null;

    private Quaternion m_InitialLookatRotation;
    private bool m_WantToRotateBack = false;

    private IEnumerator Start()
    {
        ShopDatabase.ShopItem item=ShopDatabase.TheShop.FindItemById(m_ItemNumber);

        if (item == null)
            yield break; ;

        m_Name = item.Name;
        m_ShortDescription = item.ShortDescription;
        m_Description = item.Description;

        m_InitialLookatRotation = transform.localRotation;
    }

    private void Update()
    {
        if (m_HoverCoroutine != null)
            RotateYTowardCamera();
        else if (m_WantToRotateBack)
            RotateBackFromCamera();
    }

    public void OnHover()
    {
        if (PopupHandler.Instance.IsLocked)
            return;

        m_HoverCoroutine = StartCoroutine(HoverCoroutine());
    }

    public void OnExitHover()
    {
        PopupHandler.Instance.OnClickAnywhere();

        if (m_HoverCoroutine != null)
        {
            StopCoroutine(m_HoverCoroutine);
            m_HoverCoroutine = null;
        }

        m_WantToRotateBack = true;
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

    private void RotateYTowardCamera()
    {
        if (m_MainCamera == null)
            return;

        m_WantToRotateBack = false;

        RotateYTowardGO(m_MainCamera);
    }

    private void RotateBackFromCamera()
    {
        Quaternion rot = Quaternion.Slerp(transform.localRotation, m_InitialLookatRotation, 0.1f);
        transform.localRotation = rot;
        if (Quaternion.Angle(transform.localRotation, m_InitialLookatRotation) < 1f)
            m_WantToRotateBack = false;
    }

    private void RotateYTowardGO(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        float targetY = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;

        Vector3 currentEuler = transform.eulerAngles;
        float currentY = currentEuler.y;

        float angleDiff = Mathf.DeltaAngle(currentY, targetY);
        if (Mathf.Abs(angleDiff) < 0.01f)
        {
            return;
        }

        float fraction = Time.deltaTime / m_LookAtSpeed;
        float step = Mathf.Abs(angleDiff) * fraction;

        float newY = Mathf.MoveTowardsAngle(currentY, targetY, step);

        transform.eulerAngles = new Vector3(currentEuler.x, newY, currentEuler.z);
    }
}
