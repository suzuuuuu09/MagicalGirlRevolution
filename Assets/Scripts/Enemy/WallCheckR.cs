using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckR : MonoBehaviour
{
    public bool isOn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Enemy")
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Enemy")
        {
            isOn = false;
        }
    }
}
