using System;

namespace Joshua.Presenter
{
    public interface ISlotMachineView
    {
        public event EventHandler StopReceive;
        public event EventHandler PressedReceive;
        public void StartStrongButtonAnim();
        public void StopStrongButtonAnim();
        public void SpinAndShow(string[] strings);
        public void Stop();
        public bool IsStop();
        public void SetButtonText(string text);
    }
}