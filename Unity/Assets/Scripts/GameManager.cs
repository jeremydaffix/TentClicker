using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] int _resources = 0; // ressources / score
    [SerializeField] int _clickUpgradeLevel = 0; // niveau actuel de l'upgrade clic
    [SerializeField] int _autoGatherUpgradeLevel = 0; // niveau actuel de l'upgrade autogather
    #endregion


    #region Variables
    float _lastTimeGathered = 0f; // pour le timer de l'autogather
    #endregion

    #region Evénements MonoBehaviour

    void OnEnable()
    {
        // une seule instance en même temps
        if(_instance != null && _instance != this)
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
        if(Time.time >= (_lastTimeGathered + AutoGatherTime))
        {
            _lastTimeGathered = Time.time;
            AutoGather();
        }
    }

    void FixedUpdate()
    {
        
    }

    #endregion


    #region Gestion des upgrades

    /// <summary>
    /// Evénement appelé lors d'un clic de récolte.
    /// </summary>
    public void ClickGather()
    {
        AddResources(CalculateClickGather()); // récupération des ressources selon le niveau de l'upgrade
    }

    /// <summary>
    /// Evénement appelé lorsque le temps de récolte automatique est écoulé.
    /// </summary>
    void AutoGather()
    {
        if (AutoGathererUpgradeLevel > 0) // au niveau 0 on ne récolte rien
        {
            AddResources(CalculateAutoGather()); // récupération des ressources selon le niveau de l'upgrade
        }
    }


    /// <summary>
    /// Calcul du nombre de ressources à récupérer lors du clic, selon le niveau de l'upgrade actuel.
    /// </summary>
    /// <returns>Nombre de ressources à récupérer.</returns>
    public int CalculateClickGather()
    {
        return (int)Mathf.Pow(2, ClickUpgradeLevel);
    }

    /// <summary>
    /// Calcul du nombre de ressources à récupérer lors de l'autogather, selon le niveau de l'upgrade actuel.
    /// </summary>
    /// <returns>Nombre de ressources à récupérer.</returns>
    public int CalculateAutoGather()
    {
        int rsrc = 0;

        if (AutoGathererUpgradeLevel > 0) // niveau 0 = 0 ressources
        {
           rsrc = (int)Mathf.Pow(2, AutoGathererUpgradeLevel - 1);
        }

        return rsrc;
    }


    /// <summary>
    /// Evénement appelé lors du clic sur le bouton de l'upgrade clic.
    /// Augmente le niveau si on n'est pas au maximum, et prend les ressources si elles sont disponibles.
    /// </summary>
    public void BuyClickUpgrade()
    {
        if(ClickUpgradeLevel < MaxUpgradeLevel && TakeResources(UpgradePrice))
        {
            ++ClickUpgradeLevel;

            UIManager.Instance.UpdateUpgradesItems(); // maj UI
        }
    }

    /// <summary>
    /// Evénement appelé lors du clic sur le bouton de l'upgrade autogather.
    /// Augmente le niveau si on n'est pas au maximum, et prend les ressources si elles sont disponibles.
    /// </summary>
    public void BuyAutoGatherUpgrade()
    {
        if (AutoGathererUpgradeLevel < MaxUpgradeLevel && TakeResources(UpgradePrice))
        {
            ++AutoGathererUpgradeLevel;

            UIManager.Instance.UpdateUpgradesItems(); // maj UI
        }
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
            UIManager.Instance.UpdateResources(); // maj UI
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
        UIManager.Instance.UpdateResources(); // maj UI
    }

    #endregion


    // propriétés

    #region Propriétés

    public static GameManager Instance { get => _instance; set => _instance = value; }

    public int Resources { get => _resources; set => _resources = value; }
    public int ClickUpgradeLevel { get => _clickUpgradeLevel; set => _clickUpgradeLevel = value; }
    public int AutoGathererUpgradeLevel { get => _autoGatherUpgradeLevel; set => _autoGatherUpgradeLevel = value; }
    public int AutoGatherTime { get => _autoGatherTime; set => _autoGatherTime = value; }
    public int UpgradePrice { get => _upgradePrice; set => _upgradePrice = value; }
    public int MaxUpgradeLevel { get => _maxUpgradeLevel; set => _maxUpgradeLevel = value; }

    #endregion
}
