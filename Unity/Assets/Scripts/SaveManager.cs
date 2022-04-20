using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;

    [SerializeField] string _server = "http://localhost/tentclicker";


    void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }


    public void SaveGame()
    {
        StartCoroutine(SaveCoroutine($"{_server}/game"));
    }

    public void LoadGame(string id)
    {
        StartCoroutine(LoadCoroutine($"{_server}/game/" + id));
    }


    IEnumerator SaveCoroutine(string uri)
    {
        //Debug.Log("SAVE URI: " + uri);

        UIManager.Instance.ShowPopup("Save game", "Saving in progress...");

        WWWForm form = new WWWForm();
        form.AddField("score", GameManager.Instance.SaveGame.Resources);
        form.AddField("clickUpgradeLevel", GameManager.Instance.SaveGame.ClickUpgradeLevel);
        form.AddField("autoGatherUpgradeLevel", GameManager.Instance.SaveGame.AutoGatherUpgradeLevel);
        form.AddField("decorations", JsonFromDecorations());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest(); // on attend le retour (réponse) du serveur web !

            if (webRequest.isNetworkError)
            {
                UIManager.Instance.SetPopupMessage("Error while trying to reach the server");
            }

            else if (webRequest.isHttpError)
            {
                UIManager.Instance.SetPopupMessage("Error: " + webRequest.error);
            }

            else
            {
                string result = webRequest.downloadHandler.text;

                if (result.Length != 6) // pas d'identifiant retourné : erreur
                {
                    Debug.Log("ERROR: " + result);
                    UIManager.Instance.SetPopupMessage("Error: no ID returned");
                }

                else
                {
                    //Debug.Log("ID = " + result);
                    UIManager.Instance.SetPopupMessage("Success!");
                    UIManager.Instance.SetPopupInput(result);
                }
            }
        }
    }
 
    IEnumerator LoadCoroutine(string uri)
    {
        Debug.Log("LOAD URI: " + uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // On envoie la requête et on attend la réponse
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                UIManager.Instance.SetPopupMessage("Error while trying to reach the server");
            }

            else if (webRequest.isHttpError)
            {
                UIManager.Instance.SetPopupMessage("Error: " + webRequest.error);
            }

            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                SaveGameSerializable game = JsonUtility.FromJson<SaveGameSerializable>(webRequest.downloadHandler.text);

                if(game != null)
                {
                    UIManager.Instance.SetPopupMessage("Success!");

                    Debug.Log(game.ToString());

                    GameManager.Instance.SaveGame.ClickUpgradeLevel = game.click_level;
                    GameManager.Instance.SaveGame.AutoGatherUpgradeLevel = game.autogather_level;
                    GameManager.Instance.SaveGame.Resources = game.score;

                    UIManager.Instance.UpdateUpgradesItems();
                    UIManager.Instance.UpdateResources();   
                }

                else
                {
                    UIManager.Instance.SetPopupMessage("Corrupted data received");
                }
            }
        }
    }


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

    public static SaveManager Instance { get => _instance; set => _instance = value; }

}
