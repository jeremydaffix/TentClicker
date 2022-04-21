using UnityEngine;

namespace TentClicker
{
    /// <summary>
    /// Modèle représentant les décorations.
    /// </summary>
    [System.Serializable] // serializable pour avoir la liste des champs dans l'Inspector
    public class DecorationModel
    {
        #region Enums
        // types de décoration
        public enum DecorationType
        {
            Pine1,
            Pine2,
            Pine3
        }
        #endregion

        #region Champs
        [SerializeField] DecorationType _type; // type de la décoration
        [SerializeField] int _row; // numéro de ligne
        [SerializeField] int _col; // numéro de colonne
        #endregion

        #region Constructeurs
        public DecorationModel(DecorationType type = DecorationType.Pine1, int row = 0, int col = 0)
        {
            _type = type;
            _row = row;
            _col = col;
        }
        #endregion

        #region Propriétés
        public DecorationType Type { get => _type; set => _type = value; }
        public int Row { get => _row; set => _row = value; }
        public int Col { get => _col; set => _col = value; }
        #endregion
    }
}
