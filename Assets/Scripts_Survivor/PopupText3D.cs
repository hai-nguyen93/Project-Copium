using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText3D : MonoBehaviour
{
    public TextMeshPro textMesh;
    private Vector3 direction = Vector3.up;
    public float speed = 2f;
    private float _speed;
    public float lifeSpan = 1f;
    private float timer;
    public float fadeOutSpeed = 0.7f;
    public bool faceCamera;

    Color textColor;
    
    void Start()
    {
        direction = direction.normalized;
        textColor = textMesh.color;
        timer = lifeSpan;

        if (faceCamera) transform.rotation = Camera.main.transform.rotation;
    }

    private void Update()
    {
        transform.Translate(direction * _speed * Time.deltaTime, Space.World);

        _speed = Mathf.Lerp(_speed, 0f, 1 - timer / lifeSpan);
        timer -= Time.deltaTime;
        textColor.a -= fadeOutSpeed * Time.deltaTime;
        textMesh.color = textColor;
        
        if (timer <= 0)
        {
            if (PopupTextPool.instance != null)
            {
                PopupTextPool.instance.ReleasePopupText3D(this);
            }
            else { Destroy(gameObject); }
        }
    }

    public void SimpleSetup(string text, Vector3 position, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        textColor = color;
        transform.position = position;
        direction = Vector3.up;
        _speed = speed;
        timer = lifeSpan;

        faceCamera = true;
        transform.rotation = Camera.main.transform.rotation;
    }

    public void Setup(string text, Vector3 position, Color color, bool faceCamera = true)
    {
        textMesh.text = text;
        textMesh.color = color;
        textColor = color;
        transform.position = position;
        _speed = speed;
        timer = lifeSpan;

        this.faceCamera = faceCamera;
        if (faceCamera) transform.rotation = Camera.main.transform.rotation;

        direction = GetRandomDirection();
    }

    public Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(0.65f, 1f), Random.Range(-0.5f, 0.5f)).normalized;
    }
}
