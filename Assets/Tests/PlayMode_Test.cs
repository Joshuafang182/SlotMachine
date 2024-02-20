using System.Collections;
using Joshua.Model;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SlotMachine_EditModeTest
{
    [UnityTest]
    public IEnumerator _01_Test_DataHandle_POSTRequestAsync()
    {
        DataHandle dataHandle = new DataHandle();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);
        yield return new WaitForSeconds(2);
        Assert.AreEqual(dataHandle.Data.STATUS, 1);
    }
    [UnityTest]
    public IEnumerator _02_Test_DataHandle_CompleteEvent()
    {
        DataHandle dataHandle = new DataHandle();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";
        bool wasCalled = false;

        dataHandle.RequestComplete += (o, e) => wasCalled = true;

        dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);

        yield return new WaitForSeconds(2);

        Assert.IsTrue(wasCalled);
    }
    [UnityTest]
    public IEnumerator _03_Test_DataHandle_GetString()
    {
        DataHandle dataHandle = new DataHandle();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);

        yield return new WaitForSeconds(2);

        Assert.IsNotNull(dataHandle.GetStrings());
    }
    [UnityTest]
    public IEnumerator _04_Test_DataHandle_Use_Net_Framework_PostRequestAsync()
    {
        DataHandle_Use_Net_Framework dataHandle = new DataHandle_Use_Net_Framework();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);
        yield return new WaitForSeconds(2);
        Assert.AreEqual(dataHandle.Data.STATUS, 1);
    }
    [UnityTest]
    public IEnumerator _05_Test_DataHandle_Use_Net_Framework_CompleteEvent()
    {
        DataHandle_Use_Net_Framework dataHandle = new DataHandle_Use_Net_Framework();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";
        bool wasCalled = false;

        dataHandle.RequestComplete += (o, e) => wasCalled = true;

        dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);

        yield return new WaitForSeconds(2);

        Assert.IsTrue(wasCalled);
    }
    [UnityTest]
    public IEnumerator _06_Test_DataHandle_Use_Net_Framework_GetString()
    {
        DataHandle_Use_Net_Framework dataHandle = new DataHandle_Use_Net_Framework();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);

        yield return new WaitForSeconds(2);

        Assert.IsNotNull(dataHandle.GetStrings());
    }
}
