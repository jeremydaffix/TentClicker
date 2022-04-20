using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model de la classe de sauvegarde du jeu principale.
/// </summary>
[System.Serializable] // serializable pour avoir la liste des champs dans l'Inspector
public class SaveGameModel
{
    [SerializeField] int _resources = 0; // ressources / score
    [SerializeField] int _clickUpgradeLevel = 0; // niveau actuel de l'upgrade clic
    [SerializeField] int _autoGatherUpgradeLevel = 0; // niveau actuel de l'upgrade autogather

    [SerializeField] List<DecorationModel> _decorations = new List<DecorationModel>(); // liste des décorations

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
            //UIManager.Instance.UpdateResources(); // maj UI
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
        //UIManager.Instance.UpdateResources(); // maj UI
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


    public int Resources { get => _resources; set => _resources = value; }
    public int ClickUpgradeLevel { get => _clickUpgradeLevel; set => _clickUpgradeLevel = value; }
    public int AutoGatherUpgradeLevel { get => _autoGatherUpgradeLevel; set => _autoGatherUpgradeLevel = value; }
    public List<DecorationModel> Decorations { get => _decorations; set => _decorations = value; }
}
