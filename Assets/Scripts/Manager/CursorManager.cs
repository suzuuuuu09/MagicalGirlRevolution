using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursor;

    /// <summary>
    /// �I�u�W�F�N�g�ɓ�����
    /// </summary>
    /// <param name="eventData">
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �J�[�\����ύX
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// �I�u�W�F�N�g����o��
    /// </summary>
    /// <param name="eventData">
    public void OnPointerExit(PointerEventData eventData)
    {
        // �J�[�\�������Ƃɖ߂��ꍇ
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
