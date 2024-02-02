using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpinWheel : MonoBehaviour
{
    public Image[] image;
    public RectTransform rectTransform_Back;
    public RectTransform rectTransform_Front;

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

    public IEnumerator Spinning(int times)
    {
        while (times > 0)
        {
            image[0].sprite = image[1].sprite;
            rectTransform_Front.anchoredPosition = new Vector2(0, 120);
            rectTransform_Front.DOAnchorPosY(0, .1f, false);
            yield return new WaitForSeconds(.1f);
            times--;
        }
    }

}
