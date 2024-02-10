using DG.Tweening;
using Joshua.Presenter;
using Joshua.Publlic;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joshua.View
{
    public class SlotMachineView : MonoBehaviour
    {
        private SlotMachinePresenter _presenter;
        private ItemData itemData; // 轉輪的資料
        private GameObject prefab; // 轉輪的Prefab
        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public TextMeshProUGUI Bt_SpinText { get; private set; }
        public GameObject Bt_Spin { get; private set; }

        private SpinWheel[] _spinWheels;

        public SpinWheel[] SpinWheels
        {
            get
            {
                if (_spinWheels == null)
                {
                    Transform root = GameObject.Find("Container").transform;
                    _spinWheels = new SpinWheel[itemData.NumberOfSpinWheels];
                    GameObject go;
                    for (int j = 0; j < itemData.NumberOfSpinWheels; j++)
                    {
                        go = Instantiate(prefab, root);
                        Image[] images = go.GetComponentsInChildren<Image>();
                        SpinWheel spinWheel = new(images);
                        _spinWheels[j] = spinWheel;
                    }
                }

                return _spinWheels;
            }
            set { _spinWheels = value; }
        }

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
        private void Awake()
        {
            Bt_Spin = GameObject.Find("Bt_Spin");
            Bt_Spin.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed());
            Bt_SpinText = GameObject.Find("Bt_Spin/Bt_Text").GetComponent<TextMeshProUGUI>();

            itemData = Resources.Load<ItemData>("Data/SpritesData");
            prefab = Resources.Load<GameObject>("Prefabs/SpinWheel");

            _presenter = new SlotMachinePresenter(this);

            for (int i = 0; i < itemData.sprites.Length; i++)
            {
                sprites.Add(itemData.sprites[i].name, itemData.sprites[i]);
            }

            for (int j = 0; j < SpinWheels.Length; j++)
            {
                Display(j, 0, PublicFunction.RandomLetters(1));
                Display(j, 1, PublicFunction.RandomLetters(1));
            }
        }
        private void OnDestroy()
        {
            _presenter.Dispose();
            _spinWheels = null;
        }


        private void OnButtonPressed()
        {
            pressedReceive?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 轉動和顯示結果
        /// </summary>
        /// <param name="strings">盤面結果</param>
        public void SpinAndShow(string[] strings)
        {
            for (int i = 0; i < SpinWheels.Length; i++)
            {
                if (!SpinWheels[i].Stop) continue;
                Display(i, 1, PublicFunction.RandomLetters(1));
                Queue<Sprite> sprites = CreateRandomSpriteQueue(20);
                string s = strings[i];
                StartCoroutine(Spinning(i, 20, sprites, this.sprites[s]));
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
            SpinWheels[wheelIndex].image[i].sprite = sprites[spriteKey];
        }
        /// <summary>
        /// 停止盤面轉動
        /// </summary>
        public void Stop()
        {
            for (int k = 0; k < SpinWheels.Length; k++)
            {
                SpinWheels[k].Stop = true;
            }
        }
        /// <summary>
        /// 停止按鈕UI動畫
        /// </summary>
        public void StopStrongButtonAnim()
        {
            Bt_Spin.transform.DOKill();
            Bt_Spin.transform.localScale = Vector2.one;
        }
        /// <summary>
        /// 開始呼吸動畫
        /// </summary>
        public void StartStrongButtonAnim()
        {
            Bt_Spin.transform.DOScale(0.1f, 1f)
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
        private IEnumerator Spinning(int index, int times, Queue<Sprite> queue, Sprite finalSprite)
        {
            SpinWheels[index].Stop = false;
            while (times > 0)
            {
                SpinWheels[index].image[0].sprite = SpinWheels[index].image[1].sprite;
                SpinWheels[index].rectTransform_Back.anchoredPosition = Vector2.zero;
                SpinWheels[index].rectTransform_Front.anchoredPosition = new Vector2(0, 120);
                SpinWheels[index].image[1].sprite = queue.Dequeue();
                SpinWheels[index].rectTransform_Front.DOAnchorPosY(0, .1f, false);
                SpinWheels[index].rectTransform_Back.DOAnchorPosY(-120, .1f, false);
                yield return new WaitForSeconds(.1f);
                if (SpinWheels[index].Stop) break;
                times--;
            }

            SpinWheels[index].image[0].sprite = SpinWheels[index].image[1].sprite;
            SpinWheels[index].rectTransform_Back.anchoredPosition = Vector2.zero;
            SpinWheels[index].rectTransform_Front.anchoredPosition = new Vector2(0, 120);
            SpinWheels[index].image[1].sprite = finalSprite;
            SpinWheels[index].rectTransform_Front.DOAnchorPosY(0, .5f, false).SetEase(Ease.OutCubic);
            SpinWheels[index].rectTransform_Back.DOAnchorPosY(-120, .5f, false).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(.5f);
            SpinWheels[index].Stop = true;

            if (index != 0) yield break;

            stopReceive?.Invoke(this, EventArgs.Empty);
        }
    }

}
