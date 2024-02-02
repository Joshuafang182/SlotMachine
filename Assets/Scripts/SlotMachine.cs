using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{

    private DataHandle dataHandle = new DataHandle();
    private DisplayResult displayResult;

    private void Start()
    {
        displayResult = transform.GetComponentInChildren<DisplayResult>();

        GameObject.Find("Bt_Spin")
            .GetComponent<Button>().onClick
            .AddListener(() => GetRollTask());

        dataHandle.RequestComplete += OnRequestComplete;
    }

    private void OnRequestComplete(object sender, EventArgs e)
    {
        displayResult.Show(dataHandle.returnRoll.CURRENT_ROLL);
    }

    private void GetRollTask()
    {
        StartCoroutine(dataHandle.SendRequest("spin", "test"));
    }
    
}


