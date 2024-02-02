using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{

    private DataHandle dataHandle = new DataHandle();
    private DisplayResult displayResult;
    private TextMeshProUGUI bt_SpinText;

    private void Start()
    {
        displayResult = transform.GetComponentInChildren<DisplayResult>();
        bt_SpinText = GameObject.Find("Bt_Spin/Bt_Text").GetComponent<TextMeshProUGUI>();

        GameObject.Find("Bt_Spin").GetComponent<Button>().onClick.AddListener(() => GetRollTask());

        dataHandle.RequestComplete += OnRequestComplete;
        displayResult.SpinWheels[0].StopReceive += OnStoped;
    }

    private void OnDestroy()
    {
        dataHandle.RequestComplete -= OnRequestComplete;
        displayResult.SpinWheels[0].StopReceive -= OnStoped;
    }

    private void OnStoped(object sender, EventArgs e)
    {
        bt_SpinText.text = "Spin!";
    }

    private void OnRequestComplete(object sender, EventArgs e)
    {
        displayResult.SpinAndShow(dataHandle.returnRoll.CURRENT_ROLL);
    }

    private void GetRollTask()
    {
        if (!displayResult.SpinWheels[0].stop)
        {
            displayResult.Stop();
            bt_SpinText.text = "Spin!";
            return;
        }

        StartCoroutine(dataHandle.SendRequest("spin", "test"));
        bt_SpinText.text = "Stop!";
    }
    
}


