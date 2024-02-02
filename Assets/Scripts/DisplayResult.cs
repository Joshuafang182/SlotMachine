using System.Collections.Generic;
using UnityEngine;

public class DisplayResult : MonoBehaviour
{
    [SerializeField, Header("圖案")]
    private ItemData itemData;

    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
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

    private void Start()
    {
        itemData = Resources.Load<ItemData>("Data/SpritesData");

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

    public void SpinAndShow(string[] strings)
    {
        for (int i = 0; i < SpinWheels.Length; i++)
        {
            if (!SpinWheels[i].stop) continue;
            SpinWheels[i].Set(1, this.sprites[PublicFunction.RandomLetters(1)]);
            Queue<Sprite> sprites = CreateRandomSpriteQueue(20);
            string s = strings[i];
            StartCoroutine(SpinWheels[i].Spinning(20, sprites, this.sprites[s]));
        }
    }

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

    private void Display(int wheelIndex, int imageIndex, string spriteKey)
    {
        SpinWheels[wheelIndex].Set(imageIndex, sprites[spriteKey]);
    }

    public void Stop()
    {
        for (int k = 0; k < SpinWheels.Length; k++)
        {
            SpinWheels[k].stop = true;
        }
    }

}
