using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursor;

    /// <summary>
    /// オブジェクトに入った
    /// </summary>
    /// <param name="eventData">
    public void OnPointerEnter(PointerEventData eventData)
    {
        // カーソルを変更
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// オブジェクトから出た
    /// </summary>
    /// <param name="eventData">
    public void OnPointerExit(PointerEventData eventData)
    {
        // カーソルをもとに戻す場合
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
