using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHPBar : MonoBehaviour
{
    public UIHue uiHue;
    public float fillSpeed;
    public Image hpBarFill;


    private EnemyStatus enemyStatus = null;
    private float value = 1;
    private bool isPlusScale;


    private void Start()
    {
        enemyStatus = GetComponentInParent<EnemyStatus>();
        if (transform.parent.parent.localScale.x > 0)
        {
            isPlusScale = true;
        }
        else
        {
            isPlusScale= false;
        }
    }


    void Update()
    {
        UpdateHPBar();
        ChangeBarColor();
        ScaleWithoutInfluence();
    }


    private void ScaleWithoutInfluence()
    {
        float xScale = 1;
        float zAngles = -enemyStatus.transform.localEulerAngles.z;
        if(isPlusScale)
        {
            if (transform.parent.parent.localScale.x > 0)
            {
                xScale = 1;
            }
            else if (transform.parent.parent.localScale.x < 0)
            {
                 xScale = -1;
            }
        }
        else
        {
            if (transform.parent.parent.localScale.x > 0)
            {
                xScale = -1;
            }
            else if (transform.parent.parent.localScale.x < 0)
            {
                xScale = 1;
            }
        }
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        transform.localEulerAngles = new Vector3(0f, 0f, zAngles);
    }


    private void ChangeBarColor()
    {
        float hueValue = 240;
        if(hueValue < 360)
        {
            hueValue = -170 * value + 410;
            hueValue = Mathf.Clamp(hueValue, 240, 360);
            uiHue.hue = hueValue;
        }
    }


    private void UpdateHPBar()
    {
        value = (float)enemyStatus.curHP / (float)enemyStatus.maxHP;
        hpBarFill.DOFillAmount(value, fillSpeed);
    }


    public void OnDestroy()
    {
        hpBarFill.DOKill();
    }
}
