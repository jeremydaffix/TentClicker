using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] GameObject _popup;

    [SerializeField] TMP_Text _popupTitle, _popupMessage;
    [SerializeField] TMP_InputField _popupInput;
    [SerializeField] Button _popupOkButton;


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
        HidePopup();
    }

    void Update()
    {
        
    }


    public void UpdateResources()
    {
        if(_resourcesText != null)
        {
            _resourcesText.SetText(GameManager.Instance.SaveGame.Resources.ToString());
        }
    }


    public void UpdateUpgradesItems()
    {
        SetUpgradesText(_clickUpgradeDescriptionText, $"{GameManager.Instance.SaveGame.CalculateClickGatherRessources()} per click",
                        _clickUpgradeLevelText, GameManager.Instance.SaveGame.ClickUpgradeLevel,
                        _clickUpgradePriceText);

        SetUpgradesText(_autoGatherUpgradeDescriptionText, $"{GameManager.Instance.SaveGame.CalculateAutoGatherRessources()} every {GameManager.Instance.AutoGatherTime}s",
                        _autoGatherUpgradeLevelText, GameManager.Instance.SaveGame.AutoGatherUpgradeLevel,
                        _autoGatherUpgradePriceText);

        if(_clickUpgradeButton != null && GameManager.Instance.SaveGame.ClickUpgradeLevel >= GameManager.Instance.MaxUpgradeLevel)
        {
            _clickUpgradeButton.enabled = false;
        }

        if (_autoGatherUpgradeButton != null && GameManager.Instance.SaveGame.AutoGatherUpgradeLevel >= GameManager.Instance.MaxUpgradeLevel)
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
        HidePopup();

        if (_upgradeMenu != null && !_upgradeMenu.activeInHierarchy)
        {
            UpdateUpgradesItems();
            _upgradeMenu.SetActive(true);
        }
    }

    public void HideUpgradeMenu()
    {
        if (_upgradeMenu != null && _upgradeMenu.activeInHierarchy)
        {
            _upgradeMenu.SetActive(false);
        }
    }


    public void ShowPopup()
    {
        HideUpgradeMenu();

        if (_popup != null && !_popup.activeInHierarchy)
        {
            _popup.SetActive(true);
        }
    }

    public void ShowPopup(string title, string message, string input = "", bool inputReadOnly = true, bool okButtonVisible = false, UnityAction okButtonAction = null)
    {
        SetPopupTitle(title);
        SetPopupMessage(message);
        SetPopupInput(input, inputReadOnly);
        SetPopupOkButton(okButtonVisible, okButtonAction);

        ShowPopup();
    }

    public void HidePopup()
    {
        if (_popup != null && _popup.activeInHierarchy)
        {
            _popup.SetActive(false);
        }
    }

    public void SetPopupTitle(string title)
    {
        if(_popupTitle != null)
        {
            _popupTitle.SetText(title);
        }
    }

    public void SetPopupMessage(string msg)
    {
        if (_popupMessage != null)
        {
            _popupMessage.SetText(msg);
        }
    }

    public void SetPopupInput(string text, bool readOnly = true)
    {
        if (_popupInput != null)
        {
            _popupInput.text = text;
            _popupInput.readOnly = readOnly;
            _popupInput.characterLimit = 6;

        }
    }

    public string GetPopupInput()
    {
        return _popupInput == null ? "" : _popupInput.text;
    }

    public void SetPopupOkButton(bool visible = false, UnityAction action = null)
    {
        if(_popupOkButton != null)
        {
            _popupOkButton.gameObject.SetActive(visible);

            _popupOkButton.onClick.RemoveAllListeners();

            if(visible)
            {
                _popupOkButton.onClick.AddListener(action);
            }
        }
    }



    public void ShowLoadPopup()
    {
        ShowPopup("Load game", "Please enter your save ID", "", false, true, () => { SaveManager.Instance.LoadGame(GetPopupInput()); });
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
