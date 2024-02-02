using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResult : MonoBehaviour
{
    [SerializeField, Header("圖案")]
    private ItemData itemData;

    [Header("Setting")]
    public float spineDuration = 3.0f;
    public float spineSpeed = 1.0f;

    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private SpinWheel[] _spinWheels;
    private SpinWheel[] SpinWheels
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

        for(int i = 0; i < SpinWheels.Length; i++)
        {
            SpinWheels[i].Set(1, sprites[PublicFunction.RandomLetters(1)]);
        }
    }

    private void Display(int i, string c)
    {
        SpinWheels[i].Set(1, sprites[c]);
    }

    public void Show(string[] strings)
    {
        for (int i = 0;i < strings.Length;i++)
        {
            Display(i, strings[i]);
        }
    }

}
