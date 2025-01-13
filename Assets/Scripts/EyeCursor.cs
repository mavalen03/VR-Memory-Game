using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCursor : MonoBehaviour
{
    RectTransform canvas;
    RectTransform cursor;
    Vector3 startingPosition;
    public float speed;

    void Start()
    {
        cursor = gameObject.GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        startingPosition = transform.position;
        speed = -1f;
    }

    void Update()
    {
        //transform.Translate(0f, speed, 0f);
        //if (cursor.position.y < -cursor.rect.height)
            //transform.position = new Vector3(startingPosition.x, canvas.rect.height + cursor.rect.height, startingPosition.z);
    }

    public void SetCursorPosition(Vector3 position)
    {
        //transform.position = position;
    }
}
