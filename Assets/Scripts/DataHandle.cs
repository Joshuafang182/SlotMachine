using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DataHandle
{
    public ReturnRoll returnRoll { get; private set; }

    private string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

    private EventHandler requestComplete;
    public event EventHandler RequestComplete
    {
        add { requestComplete += value; }
        remove { requestComplete -= value; }
    }


    public IEnumerator SendRequest(string v1, string v2)
    {
        WWWForm form = new WWWForm();
        form.AddField(v1, v2);

        using UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Response" + jsonResponse);
            returnRoll = JsonUtility.FromJson<ReturnRoll>(jsonResponse);
            requestComplete?.Invoke(this, EventArgs.Empty);
        }

    }
}

[System.Serializable]
public class ReturnRoll
{
    public int STATUS;
    public string MSG;
    public string[] CURRENT_ROLL;
}
