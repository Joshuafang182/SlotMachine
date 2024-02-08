using Joshua.Model;
using Joshua.View;
using System;
using UnityEngine;

namespace Joshua.Presenter
{
    [RequireComponent(typeof(SlotMachineView))]
    public class SlotMachinePresenter : MonoBehaviour
    {
        [SerializeField, Header("冷卻時間")]
        private float cooldownTime = 1.0f;

        private float lastPressTime = 0f;

        private readonly string getroll = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        private DataHandle dataHandle = new DataHandle();
        private SlotMachineView view;

        private void Start()
        {
            view = transform.GetComponent<SlotMachineView>();

            view.PressedReceive += GetRollTask;
            dataHandle.RequestComplete += OnRequestComplete;
        }

        private void OnDestroy()
        {
            view.PressedReceive -= GetRollTask;
            dataHandle.RequestComplete -= OnRequestComplete;
        }



        private void OnRequestComplete(object sender, EventArgs e)
        {
            view.SpinAndShow(dataHandle.ReturnRoll.CURRENT_ROLL);
        }

        private void GetRollTask(object sender, EventArgs e)
        {
            if (!view.SpinWheels[0].Stop)
            {
                view.Stop();
                view.Bt_SpinText.text = "Spin!";
                return;
            }

            if (Time.time - lastPressTime < cooldownTime) return;

            StartCoroutine(dataHandle.SendRequest(getroll, "spin", "test"));
            view.Bt_SpinText.text = "Stop!";
            lastPressTime = Time.time;
        }

    }

}


