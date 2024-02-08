using UnityEngine;
using UnityEngine.UI;

namespace Joshua.View
{
    public class SpinWheel
    {
        public Image[] image;
        public RectTransform rectTransform_Back;
        public RectTransform rectTransform_Front;
        public bool Stop { get; set; }

        

        public SpinWheel(Image[] images)
        {
            image = images;
            rectTransform_Back = image[0].transform.GetComponent<RectTransform>();
            rectTransform_Front = image[1].transform.GetComponent<RectTransform>();
            Stop = true;
        }
    }

}
