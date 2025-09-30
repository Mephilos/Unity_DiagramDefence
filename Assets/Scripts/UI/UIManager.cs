using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI 는 접근의 용이성을 위해서 싱글톤으로
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("UI 패널")]
    public GameObject perkSelectionPanel;
    [Header("퍽 카드")]
    public List<PerkCardUI> perkCards;


    private PlayerTowerController _requestingPlayer;

    void Start()
    {
        perkSelectionPanel.SetActive(false);
    }

    public void ShowPerkSelection(List<PerkData> options, PlayerTowerController player)
    {
        _requestingPlayer = player;
        perkSelectionPanel.SetActive(true);

        for (int i = 0; i < perkCards.Count; i++)
        {
            // 보여줄 퍽이 카드 갯수 보다 적을 수 있음
            if (i < options.Count)
            {
                perkCards[i].gameObject.SetActive(true);
                perkCards[i].Setup(options[i], this);
            }
            else
            {
                perkCards[i].gameObject.SetActive(false);
            }
        }
    }

    // PeckCard 선택시 호출
    public void OnPerkSelected(PerkData selectedPerk)
    {
        perkSelectionPanel.SetActive(false);
        _requestingPlayer.SelectNApplyPerk(selectedPerk);
    }
}
