using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHPBar : MonoBehaviour
{
    public Slider slider;
    public EnemyStatus enemyStatus;
    public UIHue uiHue;

    private float value = 1;

    void Start()
    {
        slider.value = 1f;
    }

    void Update()
    {
        value = (float)enemyStatus.curHP / (float)enemyStatus.maxHP;
        print(value);
        ChangeBarColor();
        slider.value = value;
        if(enemyStatus.transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        if(enemyStatus.transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
    }


    void ChangeBarColor()
    {
        if(value > 0.75f) 
        {
            uiHue.ChangeHue(240);
        }
        else if(value > 0.625f)
        {
            uiHue.ChangeHue(260);
        }
        else if (value > 0.5f)
        {
            uiHue.ChangeHue(280);
        }
        else if(value > 0.375f)
        {
            uiHue.ChangeHue(300);
        }
        else if (value > 0.25f)
        {
            uiHue.ChangeHue(320);
        }
        else if(value > 0.125f)
        {
            uiHue.ChangeHue(340);
        }
        else if (value > 0)
        {
            uiHue.ChangeHue(360);
        }
    }
}
