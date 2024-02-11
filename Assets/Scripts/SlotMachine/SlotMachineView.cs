using DG.Tweening;
using Joshua.Presenter;
using Joshua.Publlic;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joshua.View
{
    public class SlotMachineView : ISlotMachineView
    {
        private TextMeshProUGUI bt_SpinText;
        private SpinWheel[] spinWheels;


        private Dictionary<string, Sprite> sprites;
        private GameObject bt_Spin;


        private EventHandler stopReceive;
        public event EventHandler StopReceive
        {
            add { stopReceive += value; }
            remove { stopReceive -= value; }
        }

        private EventHandler pressedReceive;
        public event EventHandler PressedReceive
        {
            add { pressedReceive += value; }
            remove { pressedReceive -= value; }
        }
        
        public SlotMachineView(TextMeshProUGUI bt_SpinText, SpinWheel[] spinWheels, Dictionary<string, Sprite> sprites, GameObject bt_Spin)
        {
            this.bt_SpinText = bt_SpinText;
            this.spinWheels = spinWheels;
            this.sprites = sprites;
            this.bt_Spin = bt_Spin;
            bt_Spin.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed());
        }

        public void OnButtonPressed()
        {
            pressedReceive?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 回傳盤面是否停止
        /// </summary>
        /// <returns>回傳布林</returns>
        public bool IsStop()
        {
            return spinWheels[0].Stop;
        }
        /// <summary>
        /// 設定按鈕顯示的字串
        /// </summary>
        /// <param name="text">顯示的字串</param>
        public void SetButtonText(string text)
        {
            bt_SpinText.text = text;
        }
        /// <summary>
        /// 轉動和顯示結果
        /// </summary>
        /// <param name="strings">盤面結果</param>
        public void SpinAndShow(string[] strings)
        {
            for (int i = 0; i < spinWheels.Length; i++)
            {
                if (!spinWheels[i].Stop) continue;
                Display(i, 1, PublicFunction.RandomLetters(1));
                Queue<Sprite> sprites = CreateRandomSpriteQueue(20);
                string s = strings[i];
                _ = Spinning(i, 20, sprites, this.sprites[s]);
            }
        }
        /// <summary>
        /// 產生隨機的圖案佇列
        /// </summary>
        /// <param name="numberOfSprite">佇列長度</param>
        /// <returns>Sprite佇列</returns>
        private Queue<Sprite> CreateRandomSpriteQueue(int numberOfSprite)
        {
            Queue<Sprite> queue = new Queue<Sprite>(numberOfSprite);
            for (int i = 0; i < numberOfSprite; i++)
            {
                string randomLetter = PublicFunction.RandomLetters(1);
                queue.Enqueue(sprites[randomLetter]);
            }
            return queue;
        }
        /// <summary>
        /// 設定個別轉輪顯示的圖案
        /// </summary>
        /// <param name="wheelIndex">轉輪指標</param>
        /// <param name="imageIndex">選擇前後顯示元件</param>
        /// <param name="spriteKey">顯示的圖案</param>
        private void Display(int wheelIndex, int imageIndex, string spriteKey)
        {
            int i = Math.Clamp(imageIndex, 0, 1);
            spinWheels[wheelIndex].image[i].sprite = sprites[spriteKey];
        }
        /// <summary>
        /// 停止盤面轉動
        /// </summary>
        public void Stop()
        {
            for (int k = 0; k < spinWheels.Length; k++)
            {
                spinWheels[k].Stop = true;
            }
        }
        /// <summary>
        /// 停止按鈕UI動畫
        /// </summary>
        public void StopStrongButtonAnim()
        {
            bt_Spin.transform.DOKill();
            bt_Spin.transform.localScale = Vector2.one;
        }
        /// <summary>
        /// 開始呼吸動畫
        /// </summary>
        public void StartStrongButtonAnim()
        {
            bt_Spin.transform.DOScale(0.1f, 1f)
            .SetRelative(true)
            .SetEase(Ease.OutQuart)
            .SetLoops(-1, LoopType.Restart);
        }
        /// <summary>
        /// 播放旋轉動畫並顯示結果
        /// </summary>
        /// <param name="times">旋轉次數</param>
        /// <param name="queue">隨機圖案佇列</param>
        /// <param name="finalSprite">最終結果</param>
        private async Task Spinning(int index, int times, Queue<Sprite> queue, Sprite finalSprite)
        {
            spinWheels[index].Stop = false;
            while (times > 0)
            {
                spinWheels[index].image[0].sprite = spinWheels[index].image[1].sprite;
                spinWheels[index].rectTransform_Back.anchoredPosition = Vector2.zero;
                spinWheels[index].rectTransform_Front.anchoredPosition = new Vector2(0, 120);
                spinWheels[index].image[1].sprite = queue.Dequeue();
                spinWheels[index].rectTransform_Front.DOAnchorPosY(0, .1f, false);
                spinWheels[index].rectTransform_Back.DOAnchorPosY(-120, .1f, false);
                await Task.Delay(100);
                if (spinWheels[index].Stop) break;
                times--;
            }

            spinWheels[index].image[0].sprite = spinWheels[index].image[1].sprite;
            spinWheels[index].rectTransform_Back.anchoredPosition = Vector2.zero;
            spinWheels[index].rectTransform_Front.anchoredPosition = new Vector2(0, 120);
            spinWheels[index].image[1].sprite = finalSprite;
            spinWheels[index].rectTransform_Front.DOAnchorPosY(0, .5f, false).SetEase(Ease.OutCubic);
            spinWheels[index].rectTransform_Back.DOAnchorPosY(-120, .5f, false).SetEase(Ease.OutCubic);
            await Task.Delay(500);
            spinWheels[index].Stop = true;

            if (index == 0) stopReceive?.Invoke(this, EventArgs.Empty);
        }
    }

}
