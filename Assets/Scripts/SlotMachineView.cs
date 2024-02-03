using DG.Tweening;
using Joshua.Publlic;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joshua.View
{
    public class SlotMachineView : MonoBehaviour
    {
        [SerializeField, Header("圖案")]
        private ItemData itemData;

        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public TextMeshProUGUI Bt_SpinText { get; private set; }
        public GameObject Bt_Spin {  get; private set; }

        private SpinWheel[] _spinWheels; 

        public SpinWheel[] SpinWheels
        {
            get
            {
                if (_spinWheels == null)
                    _spinWheels = GetComponentsInChildren<SpinWheel>();
                return _spinWheels;
            }
            set { _spinWheels = value; }
        }

        private EventHandler pressedReceive;
        public event EventHandler PressedReceive
        {
            add { pressedReceive += value; }
            remove { pressedReceive -= value; }
        }

        private void Start()
        {
            Bt_Spin = GameObject.Find("Bt_Spin");
            Bt_Spin.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed());

            itemData = Resources.Load<ItemData>("Data/SpritesData");
            Bt_SpinText = GameObject.Find("Bt_Spin/Bt_Text").GetComponent<TextMeshProUGUI>();

            SpinWheels[0].StopReceive += OnStoped;

            StartStrongButtonAnim();

            foreach (var item in itemData.sprites)
            {
                sprites.Add(item.name, item);
            }

            for (int j = 0; j < SpinWheels.Length; j++)
            {
                Display(j, 0, "a");
                Display(j, 1, "a");
            }
        }
        private void OnDestroy()
        {
            SpinWheels[0].StopReceive -= OnStoped;
        }


        private void OnButtonPressed()
        {
            pressedReceive?.Invoke(this, EventArgs.Empty);
            Bt_Spin.transform.DOKill();
            Bt_Spin.transform.localScale = Vector2.one;
        }

        private void OnStoped(object sender, EventArgs e)
        {
            Bt_SpinText.text = "Spin!";
            StartStrongButtonAnim();
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
                SpinWheels[i].Set(1, this.sprites[PublicFunction.RandomLetters(1)]);
                Queue<Sprite> sprites = CreateRandomSpriteQueue(20);
                string s = strings[i];
                StartCoroutine(SpinWheels[i].Spinning(20, sprites, this.sprites[s]));
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
            SpinWheels[wheelIndex].Set(imageIndex, sprites[spriteKey]);
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
        /// 開始呼吸動畫
        /// </summary>
        private void StartStrongButtonAnim()
        {
            Bt_Spin.transform.DOScale(0.1f, 1f)
            .SetRelative(true)
            .SetEase(Ease.OutQuart)
            .SetLoops(-1, LoopType.Restart);
        }

    }

}
