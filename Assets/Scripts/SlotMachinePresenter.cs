using Joshua.Model;
using Joshua.View;
using System;
using UnityEngine;

namespace Joshua.Presenter
{
    public class SlotMachinePresenter : IDisposable
    {
        private float cooldownTime = 2.0f;

        private float lastPressTime = 0f;

        private readonly string getroll = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        private DataHandle dataHandle;
        private SlotMachineView view;

        private bool disposed = false;

        public SlotMachinePresenter(SlotMachineView view)
        {
            this.view = view;
            dataHandle = new DataHandle();
            view.PressedReceive += GetRollTask;
            view.StopReceive += OnStoped;
            dataHandle.RequestComplete += OnRequestComplete;
            view.StartStrongButtonAnim();
        }

        private void OnRequestComplete(object sender, EventArgs e)
        {
            view.SpinAndShow(dataHandle.ReturnRoll.CURRENT_ROLL);
        }

        private async void GetRollTask(object sender, EventArgs e)
        {
            if (!view.SpinWheels[0].Stop)
            {
                view.Stop();
                view.Bt_SpinText.text = "Spin!";
                return;
            }

            if (Time.time - lastPressTime < cooldownTime) return;

            view.StopStrongButtonAnim();
            await dataHandle.SendRequest(getroll, "spin", "test");

            view.Bt_SpinText.text = "Stop!";
            lastPressTime = Time.time;
        }
        private void OnStoped(object sender, EventArgs e)
        {
            view.Bt_SpinText.text = "Spin!";
            view.StartStrongButtonAnim();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) Cleanup();

                disposed = true;
            }
        }

        private void Cleanup()
        {
            if (view != null)
            {
                view.StopReceive -= OnStoped;
                view.PressedReceive -= GetRollTask;
            }

            if (dataHandle != null) dataHandle.RequestComplete -= OnRequestComplete;
            dataHandle.Dispose();
        }

        ~SlotMachinePresenter()
        {
            Dispose(false);
        }
    }

}


