using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    public Image[] image;
    private RectTransform rectTransform_Back;
    private RectTransform rectTransform_Front;

    private void Start()
    {
        image = GetComponentsInChildren<Image>();
        rectTransform_Back = image[0].transform.GetComponent<RectTransform>();
        rectTransform_Front = image[1].transform.GetComponent<RectTransform>();
    }

    public void Set(int i, Sprite sprite)
    {
        int index = Math.Clamp(i, 0, 1);
        image[index].sprite = sprite;
    }

    //private IEnumerator Spinning(int times)
    //{
    //    image[0].sprite = image[1].sprite;
    //    rectTransform_Front.anchoredPosition = new Vector2(0, 120);
    //}

}
