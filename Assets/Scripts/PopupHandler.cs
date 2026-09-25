//using TMPro;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject m_JustNamePanel;
    
    [SerializeField]
    public TMP_Text m_JustNameText;

    [SerializeField]
    private GameObject m_ShortDescriptionPanel;

    [SerializeField]
    private TMP_Text m_ShortDescriptionText;

    [SerializeField]
    private GameObject m_FullDescriptionPanel;

    [SerializeField]
    private TMP_Text m_FullDescriptionText;

    [SerializeField]
    private Button m_FullOkButton;

    public static PopupHandler Instance;

    public bool IsLocked { get; set; } = false;

    private Action m_OkAction = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_JustNamePanel.SetActive(false);
    }

    public void OnClickAnywhere()
    {
        if (IsLocked)
            return;

        m_JustNamePanel.SetActive(false);
        m_ShortDescriptionPanel.SetActive(false);
        m_FullDescriptionPanel.SetActive(false);

        IsLocked = false;
    }

    public void ShowName(Vector2 screenPos, string name)
    {
        if (IsLocked)
            return;
            
        OnClickAnywhere();

        m_JustNamePanel.SetActive(true);

        var worldPos = new Vector3(screenPos.x, screenPos.y, 0f);
        m_JustNameText.text = name;
        m_JustNameText.transform.parent.position = worldPos;
    }

    public void ShowShort(Vector2 screenPos, string shortText)
    {
        if (IsLocked)
            return;

        OnClickAnywhere();

        m_ShortDescriptionPanel.SetActive(true);

        var worldPos = new Vector3(screenPos.x, screenPos.y, 0f);
        m_ShortDescriptionText.text = shortText;
        m_ShortDescriptionText.transform.parent.position = worldPos;
    }

    public void ShowFull(Vector2 screenPos, string fullText, Action okAction)
    {
        if (IsLocked)
            return;

        OnClickAnywhere();

        m_FullDescriptionPanel.SetActive(true);

        var worldPos = new Vector3(screenPos.x, screenPos.y, 0f);
        m_FullDescriptionText.text = fullText;
        m_FullDescriptionText.transform.parent.position = worldPos;

        m_OkAction = okAction;

        IsLocked = true;
    }

    public void OnOkButtonClick()
    {
        m_FullDescriptionPanel.SetActive(false);
        IsLocked = false;
        m_OkAction?.Invoke();
    }

    public void OnCancelButtonClick()
    {
        m_FullDescriptionPanel.SetActive(false);
        IsLocked = false;
    }
}
