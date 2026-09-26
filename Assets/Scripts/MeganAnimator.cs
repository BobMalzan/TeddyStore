using UnityEngine;
using UnityEngine.InputSystem;

public class MeganAnimator : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;
    
    private Animator m_Animator;
    private int m_GreetingTriggerId, m_GreetingStateId, m_IdleStateId;
    private const string AID_GREET_STATE = "greeting";
    private const string AID_IDLE_STATE = "idle";
    private const string AID_GREET_TRIGGER = "greet";

    void Awake()
    {
        m_Animator = GetComponent<Animator>();

        m_GreetingTriggerId = Animator.StringToHash(AID_GREET_TRIGGER);
        m_GreetingStateId = Animator.StringToHash(AID_GREET_STATE);
        m_IdleStateId = Animator.StringToHash(AID_IDLE_STATE);
    }

    // Update is called once per frame
    void Update()
    {
        if (Pointer.current == null)
            return;

        Vector2 pos = Pointer.current.position.ReadValue();

        Ray ray = m_Camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Player"))
            {
                AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (info.shortNameHash == m_IdleStateId && !m_Animator.IsInTransition(0))
                {
                    m_Animator.SetTrigger(m_GreetingTriggerId);
                }
            }
        }
    }
}