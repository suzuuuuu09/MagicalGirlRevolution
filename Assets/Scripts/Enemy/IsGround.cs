using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGround : MonoBehaviour
{
    bool isGround = false;
    public static bool isGroundEnter, isGroundStay, isGroundExit;

    public bool IsGrounds()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGroundExit = true;
        }
    }
}
