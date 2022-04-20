using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Version serializable de SaveGameModel.
/// Correspond à la table game de la base de données.
/// </summary>
[System.Serializable]
public class SaveGameSerializable
{
    public string id;
    public int score;
    public int click_level;
    public int autogather_level;
    //public List<DecorationSerializable> decorations;

    public override string ToString()
    {
        return $"id: {id}, score: {score}, click_level: {click_level}, autogather_level: {autogather_level}";
    }
}
