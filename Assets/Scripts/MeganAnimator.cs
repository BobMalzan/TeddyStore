using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MeganAnimator : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;
    
    private Animator m_Animator;

    private int m_GreetingTriggerId, m_GreetingStateId, m_IdleStateId, m_TwerkTriggerId;
    private int[] m_IdleTriggers = new int[3];

    private const string AID_GREET_STATE = "greeting";
    private const string AID_IDLE_STATE = "idle";
    private const string AID_GREET_TRIGGER = "greet";
    private const string AID_IDLE1_TRIGGER = "happy";
    private const string AID_IDLE2_TRIGGER = "idle2";
    private const string AID_IDLE3_TRIGGER = "idle3";
    private const string AID_TWERK_TRIGGER = "twerk";


    void Awake()
    {
        m_Animator = GetComponent<Animator>();

        m_GreetingTriggerId = Animator.StringToHash(AID_GREET_TRIGGER);
        m_TwerkTriggerId = Animator.StringToHash(AID_TWERK_TRIGGER);
        m_GreetingStateId = Animator.StringToHash(AID_GREET_STATE);
        m_IdleStateId = Animator.StringToHash(AID_IDLE_STATE);

        m_IdleTriggers[0] = Animator.StringToHash(AID_IDLE1_TRIGGER);
        m_IdleTriggers[1] = Animator.StringToHash(AID_IDLE2_TRIGGER);
        m_IdleTriggers[2] = Animator.StringToHash(AID_IDLE3_TRIGGER);
    }

    void Update()
    {
        if (Pointer.current == null)
            return;

        Vector2 pos = Pointer.current.position.ReadValue();

        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);
        SuperviseIdleAnimation(info);

        Ray ray = m_Camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Player"))
            {
                if (info.shortNameHash != m_GreetingStateId && !m_Animator.IsInTransition(0))
                {
                    m_Animator.SetTrigger(m_GreetingTriggerId);
                }
            }
        }

        TriggerDemoFromKeyboard();
    }

    private void TriggerDemoFromKeyboard()
    {
        // Use the new Input System (Keyboard) for active input handling
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.f1Key.wasPressedThisFrame) m_Animator.SetTrigger(m_TwerkTriggerId);
        if (Keyboard.current.f2Key.wasPressedThisFrame) m_Animator.SetTrigger(m_IdleTriggers[0]);
        if (Keyboard.current.f3Key.wasPressedThisFrame) m_Animator.SetTrigger(m_IdleTriggers[1]);
        if (Keyboard.current.f4Key.wasPressedThisFrame) m_Animator.SetTrigger(m_IdleTriggers[2]);
    }

    public void Twerk()
    {
        m_Animator.SetTrigger(m_TwerkTriggerId);
    }

    private void SuperviseIdleAnimation(AnimatorStateInfo info)
    {
        if (info.shortNameHash != m_IdleStateId || m_Animator.IsInTransition(0))
            return;

        float progress = info.normalizedTime % 1f;

        if (progress >= 0.98f)
        {
            int rndValue = UnityEngine.Random.Range(0, 300);
            if (rndValue > 2)
                return;

            m_Animator.SetTrigger(m_IdleTriggers[rndValue]);
        }
    }
}