using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Camera))]
public class HoverDetector : MonoBehaviour
{
    private Camera m_Camera;
    private GameObject m_LastHovered;
    private ShopItemInteractor m_LastInteractor;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    void Update()
    {
        // Read the current pointer (mouse/touch) position and cast a ray into the world
        if (Pointer.current == null)
            return;

        Vector2 pos = Pointer.current.position.ReadValue();

        Ray ray = m_Camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("ShopItem"))
            {
                // If we've moved to a different hovered object, notify the previous interactor
                if (hitObject != m_LastHovered)
                {
                    if (m_LastInteractor != null)
                    {
                        m_LastInteractor.OnExitHover();
                    }

                    // Update tracking and notify the new interactor
                    m_LastHovered = hitObject;
                    m_LastInteractor = hitObject.GetComponent<ShopItemInteractor>();
                    if (m_LastInteractor != null)
                    {
                        m_LastInteractor.OnHover();
                    }
                }
                // otherwise still hovering same object - do nothing
                return;
            }
        }

        // No shop item hit: if we previously hovered something, send exit
        if (m_LastInteractor != null)
        {
            m_LastInteractor.OnExitHover();
            m_LastInteractor = null;
            m_LastHovered = null;
        }
    }
}
