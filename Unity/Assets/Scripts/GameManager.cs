using UnityEngine;

namespace TentClicker
{
    /// <summary>
    /// Composant contenant la logique de l'application.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Champs static
        static GameManager _instance; // instance du singleton
        #endregion

        #region Champs exposés
        [Header("Game parameters")]
        [SerializeField] int _autoGatherTime = 5; // délai pour l'autogather
        [SerializeField] int _upgradePrice = 20; // coût (fixe) pour chaque upgrade ou achat de décoration
        [SerializeField] int _maxUpgradeLevel = 10; // limite du niveau des upgrades

        [Header("Game variables (displayed for debugging purposes)")]
        [SerializeField] SaveGameModel _saveGame = new SaveGameModel();
        #endregion


        #region Variables
        float _lastTimeGathered = 0f; // pour le timer de l'autogather
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

        }

        void Update()
        {
            // temps de l'autogather écoulé
            if (Time.time >= (_lastTimeGathered + AutoGatherTime))
            {
                _lastTimeGathered = Time.time;
                AutoGather();
            }
        }

        #endregion


        #region Gestion des événements liés aux upgrades

        /// <summary>
        /// Evénement appelé lors d'un clic de récolte.
        /// </summary>
        public void ClickGather()
        {
            SaveGame.AddResources(SaveGame.CalculateClickGatherRessources()); // récupération des ressources selon le niveau de l'upgrade
            UIManager.Instance.UpdateResources();
        }

        /// <summary>
        /// Evénement appelé lorsque le temps de récolte automatique est écoulé.
        /// </summary>
        void AutoGather()
        {
            if (SaveGame.AutoGatherUpgradeLevel > 0) // au niveau 0 on ne récolte rien
            {
                SaveGame.AddResources(SaveGame.CalculateAutoGatherRessources()); // récupération des ressources selon le niveau de l'upgrade
                UIManager.Instance.UpdateResources();
            }
        }


        /// <summary>
        /// Evénement appelé lors du clic sur le bouton de l'upgrade clic.
        /// Augmente le niveau si on n'est pas au maximum, et prend les ressources si elles sont disponibles.
        /// </summary>
        public void BuyClickUpgrade()
        {
            if (SaveGame.ClickUpgradeLevel < MaxUpgradeLevel && SaveGame.TakeResources(UpgradePrice))
            {
                ++SaveGame.ClickUpgradeLevel;

                UIManager.Instance.UpdateUpgradesItems(); // maj UI
                UIManager.Instance.UpdateResources();
            }
        }

        /// <summary>
        /// Evénement appelé lors du clic sur le bouton de l'upgrade autogather.
        /// Augmente le niveau si on n'est pas au maximum, et prend les ressources si elles sont disponibles.
        /// </summary>
        public void BuyAutoGatherUpgrade()
        {
            if (SaveGame.AutoGatherUpgradeLevel < MaxUpgradeLevel && SaveGame.TakeResources(UpgradePrice))
            {
                ++SaveGame.AutoGatherUpgradeLevel;

                UIManager.Instance.UpdateUpgradesItems(); // maj UI
                UIManager.Instance.UpdateResources();
            }
        }

        #endregion


        #region Propriétés

        public static GameManager Instance { get => _instance; set => _instance = value; }
        public int AutoGatherTime { get => _autoGatherTime; set => _autoGatherTime = value; }
        public int UpgradePrice { get => _upgradePrice; set => _upgradePrice = value; }
        public int MaxUpgradeLevel { get => _maxUpgradeLevel; set => _maxUpgradeLevel = value; }
        public SaveGameModel SaveGame { get => _saveGame; set => _saveGame = value; }

        #endregion
    }
}
