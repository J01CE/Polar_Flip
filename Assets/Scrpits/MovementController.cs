using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Vector2 move;
    public int speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                
        transform.Translate(move * speed * Time.deltaTime);
        flip();
    }


    void flip()
    {
        if (move.x < -0.01f)
        {
            transform.localScale = new Vector3(-7, 7, 1);
        }
        else if (move.x > 0.01f)
        {
            transform.localScale = new Vector3(7, 7, 1);
        }
    }
}
    