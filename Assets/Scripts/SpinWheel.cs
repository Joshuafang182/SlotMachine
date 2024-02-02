using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class SpinWheel : MonoBehaviour
{
    public Image[] image;
    public RectTransform rectTransform_Back;
    public RectTransform rectTransform_Front;
    public bool stop { get; set; }

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
        stop = true;
    }

    private void OnDestroy()
    {
        stopReceive = null;
    }

    public void Set(int i, Sprite sprite)
    {
        int index = Math.Clamp(i, 0, 1);
        image[index].sprite = sprite;
    }

    public IEnumerator Spinning(int times, Queue<Sprite> queue, Sprite finalSprite)
    {
        stop = false;
        while (times > 0)
        {
            image[0].sprite = image[1].sprite;
            rectTransform_Back.anchoredPosition = Vector2.zero;
            rectTransform_Front.anchoredPosition = new Vector2(0, 120);
            image[1].sprite = queue.Dequeue();
            rectTransform_Front.DOAnchorPosY(0, .1f, false);
            rectTransform_Back.DOAnchorPosY(-120, .1f, false);
            yield return new WaitForSeconds(.1f);
            if (stop) { break; }
            times--;
        }

        image[0].sprite = image[1].sprite;
        rectTransform_Back.anchoredPosition = Vector2.zero;
        rectTransform_Front.anchoredPosition = new Vector2(0, 120);
        image[1].sprite = finalSprite;
        rectTransform_Front.DOAnchorPosY(0, .5f, false).SetEase(Ease.OutCubic);
        rectTransform_Back.DOAnchorPosY(-120, .5f, false).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(.5f);
        stop = true;
        stopReceive?.Invoke(this, EventArgs.Empty);
    }
}
