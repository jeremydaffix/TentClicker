using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Version serializable de SaveGameModel (pour récupérer sous forme d'objets le json retourné par le serveur).
/// Correspond à la table game de la base de données.
/// </summary>
[System.Serializable]
public class SaveGameSerializable
{
    public string id; // identifiant de la sauvegarde (6 caractères alphanumériques)
    public int score; // score / nombre de ressources obtenues
    public int click_level; // niveau de l'upgrade click
    public int autogather_level; // niveau de l'upgrade autogather
    public List<DecorationSerializable> decorations; // liste des décorations achetées

    public override string ToString()
    {
        return $"id: {id}, score: {score}, click_level: {click_level}, autogather_level: {autogather_level}";
    }
}
