using System;
using System.Collections.Generic;
using UnityEngine;

namespace TentClicker
{
    /// <summary>
    /// Composant contenant la logique de la gestion des décorations.
    /// </summary>
    public class DecorationManager : MonoBehaviour
    {
        #region Champs static
        static DecorationManager _instance; // instance du singleton
        #endregion

        #region Structures imbriquées

        /// <summary>
        /// structure pour pouvoir faire les associations type de décoration -> prefab directement dans l'Inspector
        // (Unity ne gérant pas directement l'édition de dictionnaires)
        /// </summary>
        [Serializable]
        public struct DecorationPrefab
        {
            public DecorationModel.DecorationType Type;
            public GameObject Prefab;
        }
        #endregion

        #region Champs exposés
        [SerializeField] Transform _gridRoot; // root de la grille
        [SerializeField] List<DecorationPrefab> _prefabs; // liste (éditable dans l'Inspector) liant types de décoration et prefabs
        #endregion

        #region Variables
        int _nbrRows; // nombre de lignes
        int _nbrCols; // nombre de colonnes
        int _nbrDecorations = 0; // nombre de décorations ajoutées
                                 // dictionnaire liant types de décoration et prefabs (généré à partir de la liste)
        Dictionary<DecorationModel.DecorationType, GameObject> _prefabsDictionary = new Dictionary<DecorationModel.DecorationType, GameObject>();
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
            // calcul du nombre de lignes et colonnes disponibles
            if (_gridRoot != null)
            {
                _nbrRows = _gridRoot.childCount;

                if (_nbrRows > 0)
                {
                    _nbrCols = _gridRoot.GetChild(0).childCount;
                }
            }

            // on remplit un dictionnaire liant un type de décoration et un prefab
            foreach (DecorationPrefab p in _prefabs)
            {
                _prefabsDictionary[p.Type] = p.Prefab;
            }
        }

        #endregion

        #region Gestion des décorations

        /// <summary>
        /// Acheter une décoration et la placer aléatoirement.
        /// </summary>
        /// <param name="typeDecoration">Type de la décoration.</param>
        public void BuyDecoration(int typeDecoration)
        {
            DecorationModel.DecorationType type = (DecorationModel.DecorationType)typeDecoration;

            // si il reste de la place sur la grille et qu'on a suffisamment de ressources
            if (_nbrDecorations < (_nbrRows * _nbrCols) && GameManager.Instance.SaveGame.TakeResources(GameManager.Instance.UpgradePrice))
            {
                //Debug.Log("BUY " + type);

                UIManager.Instance.UpdateResources();

                // recherche d'une place libre aléatoire
                int row, col;
                do
                {
                    row = UnityEngine.Random.Range(0, _nbrRows);
                    col = UnityEngine.Random.Range(0, _nbrCols);

                } while (!IsCellFree(row, col));

                AddDecoration(type, row, col);
            }
        }


        /// <summary>
        /// Ajouter une décoration sur la grille, et dans le modèle.
        /// </summary>
        /// <param name="type">Type de la décoration.</param>
        /// <param name="row">Ligne de la grille.</param>
        /// <param name="col">Colonne de la grille.</param>
        public void AddDecoration(DecorationModel.DecorationType type, int row, int col)
        {
            // on s'assure que tout est bien configuré
            if (_gridRoot != null && _prefabsDictionary.ContainsKey(type))
            {
                Transform parent = _gridRoot.GetChild(row).GetChild(col);
                GameObject prefab = _prefabsDictionary[type];

                GameObject deco = GameObject.Instantiate(prefab);

                if (parent != null && deco != null)
                {
                    // positionnement dans la grille
                    deco.transform.SetParent(parent);
                    deco.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                    deco.transform.localRotation = Quaternion.identity;

                    // maj du modèle
                    GameManager.Instance.SaveGame.Decorations.Add(new DecorationModel(type, row, col));

                    _nbrDecorations++;
                }
            }
        }

        /// <summary>
        /// Vérifier si une cellule de la grille est bien libre.
        /// </summary>
        /// <param name="row">Numéro de ligne de la cellule.</param>
        /// <param name="col">Numéro de colonne de la cellule.</param>
        /// <returns>True si la cellule est libre, False sinon.</returns>
        public bool IsCellFree(int row, int col)
        {
            return _gridRoot.GetChild(row).GetChild(col).childCount == 0;
        }

        /// <summary>
        /// Nettoyer la grille (notamment lors d'un chargement).
        /// </summary>
        public void ClearGrid()
        {
            //GameManager.Instance.SaveGame.Decorations.Clear();

            if (_gridRoot != null)
            {
                foreach (Transform row in _gridRoot)
                {
                    foreach (Transform col in row)
                    {
                        foreach (Transform tree in col)
                        {
                            Destroy(tree.gameObject);
                        }
                    }
                }

                _nbrDecorations = 0;
            }
        }

        #endregion

        #region Propriétés
        public static DecorationManager Instance { get => _instance; set => _instance = value; }
        #endregion

    }
}
