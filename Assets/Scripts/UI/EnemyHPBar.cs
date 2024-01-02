using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHPBar : MonoBehaviour
{
    public Slider hpSlider;
    public EnemyStatus enemyStatus;
    public UIHue uiHue;

    private float value = 1;

    void Start()
    {
        hpSlider.value = 1f;
    }

    void Update()
    {
        value = (float)enemyStatus.curHP / (float)enemyStatus.maxHP;
        print(value);
        ChangeBarColor();
        hpSlider.value = value;
        ScaleWithoutInfluence();
    }


    private void ScaleWithoutInfluence()
    {
        if (enemyStatus.transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        if (enemyStatus.transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
    }


    private void ChangeBarColor()
    {
        float hueValue = 240;
        if(hueValue < 360)
        {
            hueValue = -170 * value + 410;
            uiHue.ChangeHue(hueValue);
        }
    }
}
