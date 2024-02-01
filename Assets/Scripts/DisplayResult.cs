using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResult : MonoBehaviour
{
    [SerializeField, Header("圖案")]
    private ItemData itemData;

    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private Image[] _images;
    private Image[] images
    {
        get
        {
            if (_images == null)
                _images = GetComponentsInChildren<Image>();
            return _images;
        }
        set { _images = value; }
    }

    private void Start()
    {
        itemData = Resources.Load<ItemData>("Data/SpritesData");

        foreach (var item in itemData.sprites)
        {
            sprites.Add(item.name, item);
        }

        for(int i = 0; i < images.Length; i++)
        {
            char randomLetter = (char)('a' + Random.Range(0, 17));
            images[i].sprite = sprites[randomLetter.ToString()];
        }
    }

    public void Display(int i, string c)
    {
        images[i].sprite = sprites[c];
    }

}
