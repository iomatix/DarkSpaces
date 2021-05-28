using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public bool isInGravity = true;
    public float speedLeft = 0.27f;
    public float directionChangeDelay = 5.5f;
    private bool isDirectionChanged = false;
    private float nextDirectionChange;

    public float minVelocityX = -0.17f, maxVelocitiyX = 0.17f;
    public int velocitySteps = 32;
    private float velocityX = 0f;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float speed = speedLeft;
        // Forward-backward mov
        changeDirection();
        if (isDirectionChanged) speed = -speed;
        if (isInGravity) velocityX = ExtensionMethods.VelocityHandler(speed, velocityX, minVelocityX, maxVelocitiyX, velocitySteps);
        rigid.MovePosition(transform.position + Vector3.left * (speed + velocityX));


    }

    public void changeDirection()
    {
        if (Time.time > nextDirectionChange)
        {
            nextDirectionChange = Time.time + directionChangeDelay;
            isDirectionChanged = !isDirectionChanged;
        }
    }

}
