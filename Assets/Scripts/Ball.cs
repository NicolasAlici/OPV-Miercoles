using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Ball : MonoBehaviour
{

    [SerializeField] private float posX = 0f;
    [SerializeField] private float posY = 0f;
    [SerializeField] private float velX = 0f;
    [SerializeField] private float velY = 0f;
    [SerializeField] private float aceX = 0f;
    [SerializeField] private float aceY = 0f;
    [SerializeField] private float mass = 0f;
    [SerializeField] private int gravity = 400;

    private float dt;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        ApplyGravity();
        Movement(dt);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }

    public void Movement(float delta)
    {
        velX += aceX * delta;
        velY += aceY * delta;

        posX += velX * delta;
        posY += velY * delta;

        aceX = 0;
        aceY = 0;       
    }

    public void ApplyGravity()
    {
        aceY += gravity / mass;
    }
}
