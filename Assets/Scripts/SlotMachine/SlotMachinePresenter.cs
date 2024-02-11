using Joshua.Model;
using System;
using UnityEngine;

namespace Joshua.Presenter
{
    public class SlotMachinePresenter : IDisposable
    {
        private float cooldownTime = 2.0f;

        private float lastPressTime = 0f;

        private readonly string getroll = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        private readonly IDataHandle dataHandle;
        private readonly ISlotMachineView view;

        private bool disposed = false;

        public SlotMachinePresenter(ISlotMachineView view, IDataHandle dataHandle)
        {
            this.view = view;
            this.dataHandle = dataHandle;
            view.PressedReceive += GetRollTask;
            view.StopReceive += OnStoped;
            dataHandle.RequestComplete += OnRequestComplete;
            view.StartStrongButtonAnim();
        }

        private void OnRequestComplete(object sender, EventArgs e)
        {
            view.SpinAndShow(dataHandle.GetStrings());
        }

        private void GetRollTask(object sender, EventArgs e)
        {
            if (!view.IsStop())
            {
                view.Stop();
                view.SetButtonText("Spin!");
                return;
            }

            if (Time.time - lastPressTime < cooldownTime) return;

            view.StopStrongButtonAnim();
            _ = dataHandle.SendRequest(getroll, "spin", "test");

            view.SetButtonText("Stop");
            lastPressTime = Time.time;
        }
        private void OnStoped(object sender, EventArgs e)
        {
            view.SetButtonText("Spin!");
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
        }

        ~SlotMachinePresenter()
        {
            Dispose(false);
        }
    }

}


