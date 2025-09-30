using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkCardUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    // TODO: 퍽 아이콘 저장 
    // public Image iconImage;

    private Button _cardButton;
    private PerkData _assignedPerkData;
    private UIManager _uiManager;

    void Awake()
    {
        _cardButton = GetComponent<Button>();
    }
    public void Setup(PerkData perkData, UIManager uIManager)
    {
        _assignedPerkData = perkData;
        _uiManager = uIManager;

        nameText.text = perkData.perkName;
        descriptionText.text = perkData.description;

        _cardButton.onClick.RemoveAllListeners(); // 전에 선택 중복 방지위해서 추가
        _cardButton.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        _uiManager.OnPerkSelected(_assignedPerkData);
    }
}
