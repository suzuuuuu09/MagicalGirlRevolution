using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP : MonoBehaviour
{

    public GameObject starPrefab;
    public PlayerStatus playerStatus;
    List<MPStar> stars = new();


    private void Update()
    {
        DrawStars();
    }


    public void DrawStars()
    {
        ClearStars();
        float max_mp_remainder = playerStatus.maxMP % 20;
        int _max_mp_remainder = (int)Mathf.Clamp01(max_mp_remainder);
        int starsToMake = (int)((playerStatus.maxMP / 20) + _max_mp_remainder);
        for (int i = 0; i < starsToMake; i++)
        {
            CreateEmptyStar();
        }

        for (int i = 0; i < stars.Count; i++)
        {
            int starStatusRemainder = (int)Mathf.Lerp(0, 4, ((float)playerStatus.curMP - (i * 20)) / 20);
            stars[i].SetStarImage((StarStatus)starStatusRemainder);
        }
    }


    public void CreateEmptyStar()
    {
        GameObject newStar = Instantiate(starPrefab);
        newStar.transform.SetParent(transform);
        newStar.transform.localScale = new Vector3(1, 1, 1);

        MPStar MPComp = newStar.GetComponent<MPStar>();
        MPComp.SetStarImage(StarStatus.Empty);
        stars.Add(MPComp);
    }


    public void ClearStars()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        stars = new List<MPStar>();
    }
}
