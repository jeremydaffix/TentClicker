using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;

    [SerializeField] GameObject _upgradeMenu;
    [SerializeField] TMP_Text _resourcesText;

    //[SerializeField] TMP_Text _clickUpgradeDescriptionText, _clickUpgradeLevelText, _clickUpgradePriceText;
    //[SerializeField] TMP_Text _autoGatherUpgradeDescriptionText, _autoGatherUpgradeLevelText, _autoGatherUpgradePriceText;


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


    /*public void UpdateUpgrades()
    {
        if(_clickUpgradeLevelText != null)
        {
            _clickUpgradeLevelText.SetText("Level " + GameManager.Instance.ClickUpgradeLevel);
        }

        if (_autoGatherUpgradeLevelText != null)
        {
            _autoGatherUpgradeLevelText.SetText("Level " + GameManager.Instance.AutoGathererUpgradeLevel);
        }

        if (_clickUpgradePriceText != null)
        {
            _clickUpgradePriceText.SetText("Upgrade for " + GameManager.Instance.UpgradePrice);
        }

        if (_autoGatherUpgradePriceText != null)
        {
            _autoGatherUpgradePriceText.SetText("Upgrade for " + GameManager.Instance.UpgradePrice);
        }
    }*/


    public void ShowUpgradeMenu()
    {
        Debug.Log("SHOW MENU");

        if (_upgradeMenu != null && !_upgradeMenu.activeInHierarchy)
        {
            _upgradeMenu.SetActive(true);
        }
    }

    public void HideUpgradeMenu()
    {
        Debug.Log("HIDE MENU");

        if (_upgradeMenu != null && _upgradeMenu.activeInHierarchy)
        {
            _upgradeMenu.SetActive(false);
        }
    }


    public static UIManager Instance { get => _instance; set => _instance = value; }

}
