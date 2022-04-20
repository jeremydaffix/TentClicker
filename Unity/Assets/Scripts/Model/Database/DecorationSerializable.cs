using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Version serializable de DecorationSerializable.
/// Correspond à la table decoration de la base de données.
/// </summary>
[System.Serializable]
public class DecorationSerializable
{
    public string id_game;
    public int type;
    public int row;
    public int col;

    public override string ToString()
    {
        return $"id_game: {id_game}, type: {type}, row: {row}, col: {col}";
    }
}
