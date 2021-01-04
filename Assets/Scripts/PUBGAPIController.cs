using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

// public class Tournament{
//     public string id;
//     public string createdAt;
// }

public class PUBGAPIController : MonoBehaviour
{
    public GameObject PrefabTournament;
    public Transform Parent;
    public Scrollbar scroll;
    // private TextMeshProUGUI IdTorneo, FechaTorneo;

    //URL for API
    private readonly string BaseURL = "https://api.pubg.com/tournaments";
    //Authorization header
    private readonly string TokenAuth = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJlMjFlZWZlMC0zMDNjLTAxMzktNTMzZi0zNTUwMzY5NjEyZTUiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNjA5NzExMTcyLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6ImNoYWxsZW5nZSJ9.4j4dxLICyxIib3yP7oIPc16K2QLkc8fFBpi1V3_mubI";

    public void LoadTournaments(){
         StartCoroutine(GetTournaments());
         //scroll.Value = 1;
    }
    public void ClearTournaments(){
        try
        {
            foreach (Transform child in Parent) {
                GameObject.Destroy(child.gameObject);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }


   IEnumerator GetTournaments(){
        UnityWebRequest tournamentsInfoReq = UnityWebRequest.Get(BaseURL);
        tournamentsInfoReq.SetRequestHeader("Authorization", TokenAuth);
        tournamentsInfoReq.SetRequestHeader("Accept", "application/vnd.api+json");
        yield  return tournamentsInfoReq.SendWebRequest();

        if(tournamentsInfoReq.isNetworkError || tournamentsInfoReq.isHttpError){
            Debug.LogError(tournamentsInfoReq.error);
            yield break;
        }

        JSONNode tournamentInfoJson = JSON.Parse(tournamentsInfoReq.downloadHandler.text); 

        var tournaments = tournamentInfoJson["data"];
        for (int i=0;i<tournaments.Count; i++){
             GameObject _tournament = Instantiate(PrefabTournament, Parent);
            _tournament.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tournaments[i]["id"];
            _tournament.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tournaments[i]["attributes"]["createdAt"];
        }
   }
}
