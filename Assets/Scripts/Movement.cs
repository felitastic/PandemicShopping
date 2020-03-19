using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    float MovementSpeed = 5f;
    [SerializeField]
    float RotationSpeed = 5f;
    [SerializeField]
    float SpeedBoost = 1.5f;

    private float rotation = 0f;
    private float movement = 0f;

    public float SpeedTest;

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
                MovementSpeed *= SpeedBoost;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                print("moving normal again");
                //speed boost off
                MovementSpeed /= SpeedBoost;
            }
        }
   
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.CurGameState == eGameState.paused)
            rigid.velocity = Vector3.zero;
        else
        {
            rigid.velocity = transform.forward * MovementSpeed * movement;
            transform.Rotate(Vector3.up * RotationSpeed * rotation * 10 * Time.fixedDeltaTime);
        }
    }

    public float GetSpeed()
    {
        return SpeedTest = Mathf.Abs(rigid.velocity.z);
    }
}

