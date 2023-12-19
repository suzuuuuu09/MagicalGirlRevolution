using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPStar : MonoBehaviour
{
    public Sprite fullStar, halfStar, emptyStar;
    Image starImage;

    private void Awake()
    {
        starImage = GetComponent<Image>();
    }

    public void SetStarImage(StarStatus status)
    {
        switch (status)
        {
            case StarStatus.Empty:
                starImage.sprite = emptyStar;
                break;
            case StarStatus.Half:
                starImage.sprite = halfStar;
                break;
            case StarStatus.Full:
                starImage.sprite = fullStar;
                break;
        }
    }
}

public enum StarStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}
