namespace TentClicker
{
    /// <summary>
    /// Version serializable de DecorationSerializable  (pour récupérer sous forme d'objets le json retourné par le serveur).
    /// Correspond à la table decoration de la base de données.
    /// </summary>
    [System.Serializable]
    public class DecorationSerializable
    {
        public string id_game; // identifiant de la sauvegarde associée à la décoration
        public int type; // type de la décoration
        public int row; // ligne de la décoration
        public int col; // colonne de la décoration

        public override string ToString()
        {
            return $"id_game: {id_game}, type: {type}, row: {row}, col: {col}";
        }
    }
}
