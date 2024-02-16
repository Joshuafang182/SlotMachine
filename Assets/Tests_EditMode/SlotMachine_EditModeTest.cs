using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Joshua.Model;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SlotMachine_EditModeTest
{
    [Test]
    public async void _01_Test_DataHandle_POSTRequest()
    {
        DataHandle dataHandle = new DataHandle();
        string url = "https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

        await dataHandle.SendRequest(url, "spin", "test").ConfigureAwait(false);

        Assert.AreEqual(dataHandle.Data.STATUS, 1);
    }
}
