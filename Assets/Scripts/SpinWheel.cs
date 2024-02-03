using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace Joshua.View
{
    public class SpinWheel : MonoBehaviour
    {
        public Image[] image;
        public RectTransform rectTransform_Back;
        public RectTransform rectTransform_Front;
        public bool Stop { get; set; }

        private EventHandler stopReceive;
        public event EventHandler StopReceive
        {
            add { stopReceive += value; }
            remove { stopReceive -= value; }
        }

        private void Start()
        {
            image = GetComponentsInChildren<Image>();
            rectTransform_Back = image[0].transform.GetComponent<RectTransform>();
            rectTransform_Front = image[1].transform.GetComponent<RectTransform>();
            Stop = true;
        }

        private void OnDestroy()
        {
            stopReceive = null;
        }
        /// <summary>
        /// 設定圖案
        /// </summary>
        /// <param name="i">選擇前後(0 or 1)</param>
        /// <param name="sprite">圖案</param>
        public void Set(int i, Sprite sprite)
        {
            int index = Math.Clamp(i, 0, 1);
            image[index].sprite = sprite;
        }
        /// <summary>
        /// 播放旋轉動畫並顯示結果
        /// </summary>
        /// <param name="times">旋轉次數</param>
        /// <param name="queue">隨機圖案佇列</param>
        /// <param name="finalSprite">最終結果</param>
        public IEnumerator Spinning(int times, Queue<Sprite> queue, Sprite finalSprite)
        {
            Stop = false;
            while (times > 0)
            {
                image[0].sprite = image[1].sprite;
                rectTransform_Back.anchoredPosition = Vector2.zero;
                rectTransform_Front.anchoredPosition = new Vector2(0, 120);
                image[1].sprite = queue.Dequeue();
                rectTransform_Front.DOAnchorPosY(0, .1f, false);
                rectTransform_Back.DOAnchorPosY(-120, .1f, false);
                yield return new WaitForSeconds(.1f);
                if (Stop) { break; }
                times--;
            }

            image[0].sprite = image[1].sprite;
            rectTransform_Back.anchoredPosition = Vector2.zero;
            rectTransform_Front.anchoredPosition = new Vector2(0, 120);
            image[1].sprite = finalSprite;
            rectTransform_Front.DOAnchorPosY(0, .5f, false).SetEase(Ease.OutCubic);
            rectTransform_Back.DOAnchorPosY(-120, .5f, false).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(.5f);
            Stop = true;
            stopReceive?.Invoke(this, EventArgs.Empty);
        }
    }

}
