using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;

    [SerializeField] GameObject _upgradeMenu;
    [SerializeField] TMP_Text _resourcesText;

    [SerializeField] TMP_Text _clickUpgradeDescriptionText, _clickUpgradeLevelText, _clickUpgradePriceText;
    [SerializeField] TMP_Text _autoGatherUpgradeDescriptionText, _autoGatherUpgradeLevelText, _autoGatherUpgradePriceText;
    [SerializeField] Button _clickUpgradeButton, _autoGatherUpgradeButton;


    void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }



    void Start()
    {
        HideUpgradeMenu();
    }

    void Update()
    {
        
    }


    public void UpdateResources()
    {
        if(_resourcesText != null)
        {
            _resourcesText.SetText(GameManager.Instance.Resources.ToString());
        }
    }


    public void UpdateUpgradesItems()
    {
        SetUpgradesText(_clickUpgradeDescriptionText, $"{GameManager.Instance.CalculateClickGather()} per click",
                        _clickUpgradeLevelText, GameManager.Instance.ClickUpgradeLevel,
                        _clickUpgradePriceText);

        SetUpgradesText(_autoGatherUpgradeDescriptionText, $"{GameManager.Instance.CalculateAutoGather()} every {GameManager.Instance.AutoGatherTime}s",
                        _autoGatherUpgradeLevelText, GameManager.Instance.AutoGathererUpgradeLevel,
                        _autoGatherUpgradePriceText);

        if(_clickUpgradeButton != null && GameManager.Instance.ClickUpgradeLevel >= GameManager.Instance.MaxUpgradeLevel)
        {
            _clickUpgradeButton.enabled = false;
        }

        if (_autoGatherUpgradeButton != null && GameManager.Instance.AutoGathererUpgradeLevel >= GameManager.Instance.MaxUpgradeLevel)
        {
            _autoGatherUpgradeButton.enabled = false;
        }
    }

    void SetUpgradesText(TMP_Text descriptionText, string description, TMP_Text levelText, int level, TMP_Text priceText)
    {
        if (descriptionText != null)
        {
            descriptionText.SetText(description);
        }

        if (levelText != null)
        {
            levelText.SetText("Level " + level);
        }

        if (priceText != null)
        {
            priceText.SetText("Upgrade for " + GameManager.Instance.UpgradePrice);
        }
    }


    public void ShowUpgradeMenu()
    {
        //Debug.Log("SHOW MENU");

        if (_upgradeMenu != null && !_upgradeMenu.activeInHierarchy)
        {
            UpdateUpgradesItems();
            _upgradeMenu.SetActive(true);
        }
    }

    public void HideUpgradeMenu()
    {
        //Debug.Log("HIDE MENU");

        if (_upgradeMenu != null && _upgradeMenu.activeInHierarchy)
        {
            _upgradeMenu.SetActive(false);
        }
    }


    public bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        foreach (RaycastResult raysastResult in raysastResults)
        {
            if (raysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }
        return false;
    }


    public static UIManager Instance { get => _instance; set => _instance = value; }

}
