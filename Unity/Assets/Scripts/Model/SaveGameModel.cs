using System.Collections.Generic;
using UnityEngine;

namespace TentClicker
{
    /// <summary>
    /// Model de la classe de sauvegarde du jeu principale.
    /// </summary>
    [System.Serializable] // serializable pour avoir la liste des champs dans l'Inspector
    public class SaveGameModel
    {
        #region Champs
        [SerializeField] int _resources; // ressources / score
        [SerializeField] int _clickUpgradeLevel; // niveau actuel de l'upgrade clic
        [SerializeField] int _autoGatherUpgradeLevel; // niveau actuel de l'upgrade autogather

        [SerializeField] List<DecorationModel> _decorations = new List<DecorationModel>(); // liste des décorations
        #endregion

        #region Constructeurs
        public SaveGameModel(int resources = 0, int clickUpgradeLevel = 0, int autoGatherUpgradeLevel = 0)
        {
            _resources = resources;
            _clickUpgradeLevel = clickUpgradeLevel;
            _autoGatherUpgradeLevel = autoGatherUpgradeLevel;
        }
        #endregion

        #region Gestion des ressources

        /// <summary>
        /// Méthode pour tenter de prendre une quantité de ressources.
        /// Si elles sont disponibles, la quantité est déduite.
        /// </summary>
        /// <param name="price">Quantité de ressources à prendre.</param>
        /// <returns>True si suffisamment de ressources, False sinon.</returns>
        public bool TakeResources(int price)
        {
            bool enoughResources = (Resources >= price);

            if (enoughResources)
            {
                _resources -= price;
            }

            return enoughResources;
        }

        /// <summary>
        /// Ajout d'une quantité de ressources.
        /// </summary>
        /// <param name="nbr">Quantité de ressources à ajouter.</param>
        public void AddResources(int nbr)
        {
            _resources += nbr;
        }

        #endregion


        #region Gestion des upgrades

        /// <summary>
        /// Calcul du nombre de ressources à récupérer lors du clic, selon le niveau de l'upgrade actuel.
        /// </summary>
        /// <returns>Nombre de ressources à récupérer.</returns>
        public int CalculateClickGatherRessources()
        {
            return (int)Mathf.Pow(2, ClickUpgradeLevel);
        }

        /// <summary>
        /// Calcul du nombre de ressources à récupérer lors de l'autogather, selon le niveau de l'upgrade actuel.
        /// </summary>
        /// <returns>Nombre de ressources à récupérer.</returns>
        public int CalculateAutoGatherRessources()
        {
            int rsrc = 0;

            if (AutoGatherUpgradeLevel > 0) // niveau 0 = 0 ressources
            {
                rsrc = (int)Mathf.Pow(2, AutoGatherUpgradeLevel - 1);
            }

            return rsrc;
        }

        #endregion


        #region Propriétés
        public int Resources { get => _resources; set => _resources = value; }
        public int ClickUpgradeLevel { get => _clickUpgradeLevel; set => _clickUpgradeLevel = value; }
        public int AutoGatherUpgradeLevel { get => _autoGatherUpgradeLevel; set => _autoGatherUpgradeLevel = value; }
        public List<DecorationModel> Decorations { get => _decorations; set => _decorations = value; }
        #endregion
    }
}
