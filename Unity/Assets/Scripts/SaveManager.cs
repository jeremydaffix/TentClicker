using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Composant contenant la logique de sauvegarde.
/// </summary>
public class SaveManager : MonoBehaviour
{
    #region Champs static
    static SaveManager _instance; // instance du singleton
    #endregion

    #region Champs exposés
    // url du serveur
    // _server/game/ + POST -> création d'une sauvegarde
    // _server/game/:id (GET) -> récupération d'une sauvegarde
    [SerializeField] string _server = "http://localhost/tentclicker";
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
    #endregion


    #region Création d'une sauvegarde

    /// <summary>
    /// Lancement de la procédure de sauvegarde.
    /// </summary>
    public void SaveGame()
    {
        StartCoroutine(SaveCoroutine($"{_server}/game"));
    }

    /// <summary>
    /// Coroutine effectuant la sauvegarde.
    /// </summary>
    /// <param name="uri">Adresse de l'API pour la sauvegarde.</param>
    /// <returns></returns>
    IEnumerator SaveCoroutine(string uri)
    {
        //Debug.Log("SAVE URI: " + uri);

        UIManager.Instance.ShowPopup("Save game", "Saving in progress...");

        // ajout des données POST de la requête HTTP
        WWWForm form = new WWWForm();
        form.AddField("score", GameManager.Instance.SaveGame.Resources);
        form.AddField("clickUpgradeLevel", GameManager.Instance.SaveGame.ClickUpgradeLevel);
        form.AddField("autoGatherUpgradeLevel", GameManager.Instance.SaveGame.AutoGatherUpgradeLevel);
        form.AddField("decorations", JsonFromDecorations());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest(); // on attend la réponse du serveur web

            if (webRequest.isNetworkError) // erreur de connectivité
            {
                UIManager.Instance.SetPopupMessage("Error while trying to reach the server");
            }

            else if (webRequest.isHttpError) // erreur HTTP
            {
                UIManager.Instance.SetPopupMessage("Error: " + webRequest.error);
            }

            else // pas d'erreur trouvée
            {
                string result = webRequest.downloadHandler.text;

                if (result.Length != 6) // pas d'identifiant retourné : erreur
                {
                    //Debug.Log("ERROR: " + result);
                    UIManager.Instance.SetPopupMessage("Error: no ID returned");
                }

                else // identifiant bien retourné
                {
                    //Debug.Log("ID = " + result);
                    UIManager.Instance.SetPopupMessage("Success!");
                    UIManager.Instance.SetPopupInput(result);
                }
            }
        }
    }

    #endregion

    #region Récupération d'une sauvegarde

    /// <summary>
    /// Lancement de la procédure de chargement.
    /// </summary>
    /// <param name="id">Identifiant de la sauvegarde précédemment enregistrée.</param>
    public void LoadGame(string id)
    {
        StartCoroutine(LoadCoroutine($"{_server}/game/" + id));
    }

    /// <summary>
    /// Coroutine effectuant le chargement.
    /// </summary>
    /// <param name="uri">Adresse de l'API pour la récupération.</param>
    /// <returns></returns>
    IEnumerator LoadCoroutine(string uri)
    {
        //Debug.Log("LOAD URI: " + uri);

        UIManager.Instance.ShowPopup("Load game", "Loading in progress...");

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest(); // on attend le serveur

            if (webRequest.isNetworkError) // erreur de connectivité
            {
                UIManager.Instance.SetPopupMessage("Error while trying to reach the server");
            }

            else if (webRequest.isHttpError) // erreur http (par exemple mauvais identifiant donné)
            {
                UIManager.Instance.SetPopupMessage("Error: " + webRequest.error);
            }

            else // pas d'erreur retournée
            {
                //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                // on désérialise le JSON reçu
                SaveGameSerializable game = JsonUtility.FromJson<SaveGameSerializable>(webRequest.downloadHandler.text);

                if(game != null)
                {
                    UIManager.Instance.SetPopupMessage("Success!");

                    DecorationManager.Instance.ClearGrid(); // nettoyage de la grille des décorations

                    // maj du modèle de la sauvegarde
                    GameManager.Instance.SaveGame = new SaveGameModel(game.score, game.click_level, game.autogather_level);

                    // pour toutes les décorations on ajoute sur la grille (et on ajoute dans le modèle par la même occasion)
                    foreach(DecorationSerializable deco in game.decorations)
                    {
                        DecorationManager.Instance.AddDecoration((DecorationModel.DecorationType)deco.type, deco.row, deco.col);
                    }

                    // maj UI
                    UIManager.Instance.UpdateUpgradesItems();
                    UIManager.Instance.UpdateResources();   
                }

                else // problème lors de la désérialisation
                {
                    UIManager.Instance.SetPopupMessage("Corrupted data received");
                }
            }
        }
    }

    #endregion


    #region Fonctions diverses

    /// <summary>
    /// Retourne une chaîne JSON à partir des décorations du modèle.
    /// </summary>
    /// <returns>Tableau d'objets JSON correspondant aux décorations ajoutées au jeu.</returns>
    string JsonFromDecorations()
    {
        string array = "";

        foreach(DecorationModel decoration in GameManager.Instance.SaveGame.Decorations)
        {
            if (array != "")
                array += ", ";

            int type = (int)decoration.Type;
            string obj = $"{{\"type\": \"{type}\", \"row\": \"{decoration.Row}\", \"col\": \"{decoration.Col}\"}}";

            array += obj;
        }

        array = $"[{array}]";

        //Debug.Log("JsonFromDecorations = " + array);

        return array;
    }

    #endregion

    #region Propriétés
    public static SaveManager Instance { get => _instance; set => _instance = value; }
    #endregion

}
