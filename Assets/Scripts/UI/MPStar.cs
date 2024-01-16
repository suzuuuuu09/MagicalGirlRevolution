using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPStar : MonoBehaviour
{
    public Sprite fullStar, quarterStar, halfStar, threeQuartersStar, emptyStar;
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
            case StarStatus.Quarter:
                starImage.sprite = quarterStar;
                break;
            case StarStatus.Half:
                starImage.sprite = halfStar;
                break;
            case StarStatus.ThreeQuarters:
                starImage.sprite = threeQuartersStar;
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
    Quarter = 1,
    Half = 2,
    ThreeQuarters = 3,
    Full = 4
}
