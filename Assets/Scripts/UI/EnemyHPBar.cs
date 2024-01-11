using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHPBar : MonoBehaviour
{
    public EnemyStatus enemyStatus;
    public UIHue uiHue;
    public float fillSpeed;
    public Image hpBarFill;

    private float value = 1;

    void Update()
    {
        UpdateHPBar();
        ChangeBarColor();
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
            uiHue.hue = hueValue;
        }
    }

    private void UpdateHPBar()
    {
        value = (float)enemyStatus.curHP / (float)enemyStatus.maxHP;
        enemyStatus.curHP = Mathf.Clamp(enemyStatus.curHP, 0, enemyStatus.maxHP);
        hpBarFill.DOFillAmount(value, fillSpeed);
    }
}
