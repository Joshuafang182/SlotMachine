using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    private string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";
    private ReturnRoll returnRoll;
    private DisplayResult displayResult;

    private void Start()
    {
        displayResult = transform.GetComponentInChildren<DisplayResult>();
        GameObject.Find("Bt_Spin")
            .GetComponent<Button>().onClick
            .AddListener(() => StartCoroutine(SendRequestAndDisplayResult("spin", "test")));
    }

    private IEnumerator SendRequestAndDisplayResult(string v1, string v2)
    {
        WWWForm form = new WWWForm();
        form.AddField(v1, v2);

        using UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            print(www.error);
        else
        {
            string jsonResponse = www.downloadHandler.text;
            print("Response" + jsonResponse);
            returnRoll = JsonUtility.FromJson<ReturnRoll>(jsonResponse);
        }

        for (int i = 0; i < returnRoll.CURRENT_ROLL.Length; i++)
            displayResult.Display(i, returnRoll.CURRENT_ROLL[i]);
    }
}

[System.Serializable]
public class ReturnRoll
{
    public int STATUS;
    public string MSG;
    public string[] CURRENT_ROLL;
}
