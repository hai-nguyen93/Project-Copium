using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public Vector2 direction = Vector2.up;
    public float speed = 2f;
    public float lifeSpan = 1f;
    public float fadeOutSpeed = 0.7f;

    Color textColor;
    
    // Start is called before the first frame update
    void Start()
    {
        direction = direction.normalized;
        textColor = textMesh.color;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        lifeSpan -= Time.deltaTime;
        textColor.a -= fadeOutSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(string text)
    {
        textMesh.text = text;
    }

    public void Setup(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        textColor = textMesh.color;
    }
}
