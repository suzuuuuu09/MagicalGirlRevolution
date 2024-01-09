using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCheck : MonoBehaviour
{
    /// <summary>
    /// �m������
    /// </summary>
    /// <param name="fPercent">�m�� (0�`100)</param>
    /// <returns>���I���� [true]���I</returns>
    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
