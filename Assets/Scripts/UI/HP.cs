using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerStatus playerStatus;
    public float amp;
    public float freq;
    List<HPHeart> hearts = new();

    private Vector3 initPos;
    private Vector3 midPos;

    private void Start()
    {
        initPos = transform.position;
    }

    private void Update()
    {
        midPos = transform.position;
        DrawHearts();
        transform.position = new Vector3(midPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, midPos.z);
    }

    public void DrawHearts()
    {
        ClearHearts();
        float maxHP_remainder = playerStatus.maxHP % 20;
        int _maxHP_remainder = (int)Mathf.Clamp01(maxHP_remainder);
        int heartsToMake = (int)((playerStatus.maxHP / 20) + _maxHP_remainder);
        for(int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++) 
        {   
            int heartStatusRemainder = (int)Mathf.Lerp(0, 4, ((float)playerStatus.curHP - (i * 20)) / 20);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
        newHeart.transform.localScale = new Vector3(1, 1, 1);

        HPHeart HPComp = newHeart.GetComponent<HPHeart>();
        HPComp.SetHeartImage(HeartStatus.Empty);
        hearts.Add(HPComp);
    }


    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HPHeart> ();
    }
}
