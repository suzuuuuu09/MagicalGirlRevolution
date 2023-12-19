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
        float maxHP_remainder = playerStatus.maxHP % 2;
        int heartsToMake = (int)((playerStatus.maxHP / 2) + maxHP_remainder);
        for(int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for(int i = 0;i < hearts.Count; i++) 
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerStatus.curHP - (i * 2), 0, 2);
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
