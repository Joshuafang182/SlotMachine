using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

namespace Joshua.Model
{
    /// <summary>
    /// 處理網路和資料
    /// </summary>
    public class DataHandle : IDisposable
    {
        public ReturnRoll ReturnRoll { get; private set; }

        private bool disposed = false;

        private EventHandler requestComplete;
        public event EventHandler RequestComplete
        {
            add { requestComplete += value; }
            remove { requestComplete -= value; }
        }

        /// <summary>
        /// 發送POST請求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="v1">參數1</param>
        /// <param name="v2">參數2</param>
        /// <returns></returns>
        public async Task SendRequest(string url, string v1, string v2)
        {
            WWWForm form = new WWWForm();
            form.AddField(v1, v2);

            using UnityWebRequest www = UnityWebRequest.Post(url, form);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Response" + jsonResponse);
                ReturnRoll = JsonUtility.FromJson<ReturnRoll>(jsonResponse);
                requestComplete?.Invoke(this, EventArgs.Empty);
            }

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
            ReturnRoll = null;
        }

        ~DataHandle()
        {
            Dispose(false);
        }
    }

    [System.Serializable]
    public class ReturnRoll
    {
        public int STATUS;
        public string MSG;
        public string[] CURRENT_ROLL;
    }

}
