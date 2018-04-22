using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour {

	public float speed = 0.001f;
    private Vector3 startPosition;
    private Vector3 maxLeftPosition;
    private Vector3 maxRightPosition;
    public float distance = 0.5f;
    public bool moveLeft = false;    // Controls left or right
    private bool isCollisionInProgress = false;

    public void Awake()
    {
        startPosition = transform.position;
        maxLeftPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
        maxRightPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            MoveIdle();
        }
    }

   
    private void OnTriggerExit2D(Collider2D collision)
    {
        isCollisionInProgress = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player")
        {
            // Simple switch
            if (isCollisionInProgress)
            {
                return;
            }

            isCollisionInProgress = true;
            if (!moveLeft)
            {
                moveLeft = true;
            }
            else
            {
                moveLeft = false;
            }
        }
    }

    void MoveIdle()
    {
        if (moveLeft)
        {
            // WE move right
            if (maxLeftPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
                transform.position = new Vector3((float)transform.position.x - speed, (float)transform.position.y, (float)transform.position.z);
            }
            else
            {
                moveLeft = false;
            }
        }
        else
        {
            // WE move right
            if (maxRightPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(-4, transform.localScale.y, transform.localScale.z);
                transform.position = new Vector3((float)transform.position.x + speed, (float)transform.position.y, (float)transform.position.z);
            }
            else
            {
                moveLeft = true;
            }
        }
    }
    
}
