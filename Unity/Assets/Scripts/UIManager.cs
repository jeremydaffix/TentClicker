using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace TentClicker
{
    /// <summary>
    /// Composant permettant de gérer l'UI (menu des achats et popup utilisée pour la sauvegarde / chargement).
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Champs static
        static UIManager _instance; // instance du singleton
        #endregion

        #region Champs exposés

        [Header("UI Elements in scene")]
        [SerializeField] GameObject _upgradeMenu; // root du menu
        [SerializeField] TMP_Text _resourcesText; // texte du compteur de ressources
        [SerializeField] Button _loadButton; // bouton load
        [SerializeField] Button _saveButton; // bouton save
        [SerializeField] Button _openMenuButton; // bouton ouverture menu upgrades
        [SerializeField] GameObject _tent; // objet de la tente
        [Header("UI Elements in menu")]
        // textes click upgrade
        [SerializeField] TMP_Text _clickUpgradeDescriptionText;
        [SerializeField] TMP_Text _clickUpgradeLevelText;
        [SerializeField] TMP_Text _clickUpgradePriceText;
        // textes autogather upgrade
        [SerializeField] TMP_Text _autoGatherUpgradeDescriptionText;
        [SerializeField] TMP_Text _autoGatherUpgradeLevelText;
        [SerializeField] TMP_Text _autoGatherUpgradePriceText;
        // boutons
        [SerializeField] Button _clickUpgradeButton;
        [SerializeField] Button _autoGatherUpgradeButton;
        [Header("Save/Load popup parameters")]
        [SerializeField] GameObject _popup; // root de la popup
        [SerializeField] TMP_Text _popupTitle, _popupMessage; // textes de la popup
        [SerializeField] TMP_InputField _popupInput; // input de la popup
        [SerializeField] Button _popupOkButton; // bouton de la popup

        #endregion

        #region Variables
        Color _scoreColor;
        #endregion

        #region Evénements MonoBehaviour

        void OnEnable()
        {
            // une seule instance en même temps
            if (_instance != null && _instance != this)
            {
                Destroy(_instance);
            }

            _instance = this;
        }

        void Start()
        {
            if(_resourcesText != null)
            {
                _scoreColor = _resourcesText.color;
            }

            // on cache tout au démarrage
            HideUpgradeMenu();
            HidePopup();
        }

        #endregion

        #region Méthodes de mise à jour de l'UI

        // plutôt que de faire les maj dans une boucle d'update,
        // on n'appelle ces fonctions que lorsque c'est nécessaire

        /// <summary>
        /// Mise à jour de l'indicateur de ressources.
        /// </summary>
        public void UpdateResources()
        {
            if (_resourcesText != null)
            {
                string newText = GameManager.Instance.SaveGame.Resources.ToString("000000");

                if (newText != _resourcesText.text)
                {
                    //_resourcesText.SetText(newText);
                    _resourcesText.DOComplete();
                    _resourcesText.DOText(newText, 0.3f, true, ScrambleMode.Numerals);
                    _resourcesText.DOColor(Color.white, 0.15f).OnComplete(() => { _resourcesText.DOColor(_scoreColor, 0.15f); });
                }
            }
        }

        /// <summary>
        /// Mise à jour des items dans le menu d'achat.
        /// </summary>
        public void UpdateUpgradesItems()
        {
            SetUpgradesText(_clickUpgradeDescriptionText, $"{GameManager.Instance.SaveGame.CalculateClickGatherRessources()} per click",
                            _clickUpgradeLevelText, GameManager.Instance.SaveGame.ClickUpgradeLevel,
                            _clickUpgradePriceText);

            SetUpgradesText(_autoGatherUpgradeDescriptionText, $"{GameManager.Instance.SaveGame.CalculateAutoGatherRessources()} every {GameManager.Instance.AutoGatherTime}s",
                            _autoGatherUpgradeLevelText, GameManager.Instance.SaveGame.AutoGatherUpgradeLevel,
                            _autoGatherUpgradePriceText);

            if (_clickUpgradeButton != null && (GameManager.Instance.SaveGame.ClickUpgradeLevel >= GameManager.Instance.MaxUpgradeLevel /*||
            GameManager.Instance.SaveGame.Resources >= GameManager.Instance.UpgradePrice*/))
            {
                _clickUpgradeButton.interactable = false;
            }

            if (_autoGatherUpgradeButton != null && (GameManager.Instance.SaveGame.AutoGatherUpgradeLevel >= GameManager.Instance.MaxUpgradeLevel /*||
            GameManager.Instance.SaveGame.Resources >= GameManager.Instance.UpgradePrice*/))
            {
                _autoGatherUpgradeButton.interactable = false;
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

        /// <summary>
        /// Affichage du menu.
        /// </summary>
        public void ShowUpgradeMenu()
        {
            HidePopup();

            if (_upgradeMenu != null && !_upgradeMenu.activeInHierarchy)
            {
                UpdateUpgradesItems();
                _upgradeMenu.SetActive(true);

                if(_openMenuButton != null)
                {
                    _openMenuButton.interactable = false;
                }

                _upgradeMenu.transform.GetChild(0).DOComplete();
                _upgradeMenu.transform.GetChild(0).DOShakeScale(0.75f, 0.05f, 10);
            }
        }

        /// <summary>
        /// Cacher le menu.
        /// </summary>
        public void HideUpgradeMenu()
        {
            if (_upgradeMenu != null && _upgradeMenu.activeInHierarchy)
            {
                _upgradeMenu.SetActive(false);

                if (_openMenuButton != null)
                {
                    _openMenuButton.interactable = true;
                }
            }
        }

        #endregion

        #region Gestion de la popup

        /// <summary>
        /// Afficher la popup.
        /// </summary>
        public void ShowPopup()
        {
            HideUpgradeMenu();

            if (_popup != null && !_popup.activeInHierarchy)
            {
                _popup.SetActive(true);

                if (_popupInput != null)
                {
                    _popupInput.Select(); // focus sur le input
                }

                if (_saveButton != null && _loadButton != null)
                {
                    _saveButton.interactable = false;
                    _loadButton.interactable = false;
                }

                _popup.transform.GetChild(0).DOComplete();
                _popup.transform.GetChild(0).DOShakeScale(0.75f, 0.05f, 10);
            }
        }

        /// <summary>
        /// Afficher la popup en lui donnant les données à afficher.
        /// </summary>
        /// <param name="title">Titre de la popup.</param>
        /// <param name="message">Message affiché dans la popup.</param>
        /// <param name="input">Texte dans l'inputfield (utilisé pour afficher ou entrer le code).</param>
        /// <param name="inputReadOnly">Inputfield en lecture seule ?</param>
        /// <param name="okButtonVisible">Affichage du bouton OK.</param>
        /// <param name="okButtonAction">Action à effectuer lors du clic sur le bouton OK.</param>
        public void ShowPopup(string title, string message, string input = "", bool inputReadOnly = true, bool okButtonVisible = false, UnityAction okButtonAction = null)
        {
            SetPopupTitle(title);
            SetPopupMessage(message);
            SetPopupInput(input, inputReadOnly);
            SetPopupOkButton(okButtonVisible, okButtonAction);

            ShowPopup();
        }

        /// <summary>
        /// Cacher la popup.
        /// </summary>
        public void HidePopup()
        {
            if (_popup != null && _popup.activeInHierarchy)
            {
                _popup.SetActive(false);

                if (_saveButton != null && _loadButton != null)
                {
                    _saveButton.interactable = true;
                    _loadButton.interactable = true;
                }
            }
        }

        public void SetPopupTitle(string title)
        {
            if (_popupTitle != null)
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
            if (_popupOkButton != null)
            {
                _popupOkButton.gameObject.SetActive(visible);

                _popupOkButton.onClick.RemoveAllListeners();

                if (visible)
                {
                    _popupOkButton.onClick.AddListener(action);
                }
            }
        }


        /// <summary>
        /// Evénement lors du clic sur le bouton Load.
        /// </summary>
        public void ShowLoadPopup()
        {
            ShowPopup("Load game", "Please enter your save ID", "", false, true, () => { SaveManager.Instance.LoadGame(GetPopupInput()); });
        }

        #endregion


        #region Fonctions diverses
        /// <summary>
        /// Permet de savoir si le pointeur est sur l'UI.
        /// </summary>
        /// <returns>True si le pointeur de la souris est au dessus de l'UI.</returns>
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

        /// <summary>
        /// Feedback graphique sur la tente.
        /// </summary>
        public void FeedbackTent()
        {
            if(_tent != null)
            {
                _tent.transform.DOComplete();
                _tent.transform.DOPunchScale(new Vector3(1.01f, 1.01f, 1.01f), 0.25f);
            }
        }

        #endregion

        #region Propriétés
        public static UIManager Instance { get => _instance; set => _instance = value; }
        #endregion

    }
}
