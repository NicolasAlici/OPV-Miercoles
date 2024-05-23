using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float posX = 0f;
    [SerializeField] private float speed = 0f;
    private float dt;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        Movement(dt);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    public void Movement(float delta)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            posX -= speed * delta;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            posX += speed * delta;
        }
    }
}
