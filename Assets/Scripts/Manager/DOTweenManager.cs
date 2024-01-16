using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenManager : MonoBehaviour
{
    void Start()
    {
        DOTween.SetTweensCapacity(200, 500);
    }
}
