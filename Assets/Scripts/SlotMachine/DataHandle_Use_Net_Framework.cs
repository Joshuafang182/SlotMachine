using Joshua.Model;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using UnityEngine;

public class DataHandle_Use_Net_Framework : DataHandle
{
    public override async Task SendRequest(string url, string v1, string v2)
    {
        HttpContent content = new StringContent($"{v1}{v2}");

        using HttpClient client = new HttpClient();
        var operation = await client.PostAsync(url, content);

        if (!operation.IsSuccessStatusCode)
            Debug.Log(operation.StatusCode);
        else
        {
            string jsonResponse = await operation.Content.ReadAsStringAsync();
            Debug.Log("Response" + jsonResponse);
            Data = JsonUtility.FromJson<ReturnRoll>(jsonResponse);
            requestComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}
