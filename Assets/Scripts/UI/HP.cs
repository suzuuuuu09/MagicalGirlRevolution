using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerStatus playerStatus;
    List<HPHeart> hearts = new();


    private void Update()
    {
        DrawHearts();
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
            print(hearts.Count);
            //int heartStatusRemainder = (int)Mathf.Clamp(playerStatus.curHP - (i * 20), 0, 2);
            int heartStatusRemainder = (int)Mathf.Lerp(0, 1, playerStatus.curHP - (i * 20));
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
