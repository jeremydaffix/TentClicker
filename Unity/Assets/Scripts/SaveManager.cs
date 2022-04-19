using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager : MonoBehaviour
{
    [SerializeField] string _server = "http://localhost/tentclicker";


    public void SaveGame()
    {
        StartCoroutine(SaveCoroutine($"{_server}/game/"));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadCoroutine($"{_server}/game/")); // + id
    }


    IEnumerator SaveCoroutine(string uri)
    {
        //Debug.Log("SAVE URI: " + uri);

        WWWForm form = new WWWForm();
        form.AddField("score", GameManager.Instance.Resources);
        form.AddField("clickUpgradeLevel", GameManager.Instance.ClickUpgradeLevel);
        form.AddField("autoGatherUpgradeLevel", GameManager.Instance.AutoGathererUpgradeLevel);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest(); // on attend le retour (réponse) du serveur web !

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {

                string result = webRequest.downloadHandler.text;


                if (result.Length != 6) // pas d'identifiant retourné : erreur
                {
                    Debug.Log("ERROR: " + result);

                }

                else
                {
                    Debug.Log("ID = " + result);
                }
            }
        }
    }
 
    IEnumerator LoadCoroutine(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // On envoie la requête et on attend la réponse
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
