using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Slider slider;
    public EnemyStatus enemyStatus;
    public UIHue uiHue;

    private float value = 1;
    private float currentValue = 1;

    void Start()
    {
        slider.value = 1f;
    }

    void Update()
    {
        value = (float)enemyStatus.curHP / (float)enemyStatus.maxHP;
        slider.value = value;
        if(enemyStatus.transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        if(enemyStatus.transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
        if(currentValue > value)
        {
            float uihueValue = -100 * value;
            uiHue.ChangeHue(uihueValue);
            currentValue = value;
        }
    }
}
