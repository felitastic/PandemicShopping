using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    float movementSpeed = 5f;
    [SerializeField]
    float rotationSpeed = 5f;
    [SerializeField]
    float speedBoost = 1.5f;

    private float rotation = 0f;
    private float movement = 0f;

    void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        if (GameManager.Instance.CurGameState == eGameState.running)
        {
            rotation = Input.GetAxis("Horizontal");
            movement = Input.GetAxis("Vertical");

            if ((Input.GetKey(KeyCode.LeftShift)))
            {
                //TODO timer for stamina            
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                print("moving faster");
                //speed boost on
                movementSpeed *= speedBoost;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                print("moving normal again");
                //speed boost off
                movementSpeed /= speedBoost;
            }
        }
   
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.CurGameState == eGameState.paused)
            rigid.velocity = Vector3.zero;
        else
        {
            rigid.velocity = transform.forward * movementSpeed * movement;
            transform.Rotate(Vector3.up * rotationSpeed * rotation * 10 * Time.fixedDeltaTime);
        }
    }

    public float GetSpeed()
    {
        return movementSpeed;
    }
}

