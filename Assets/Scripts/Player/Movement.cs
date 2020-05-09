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

    GameManager GM { get { return GameManager.Instance; } }

    void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
        GameManager.OnGameStateChange += ResetVelocity;
    }

    void Update()
    {
        if (GM.CurGameState == eGameState.running)
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
        if (GM.CurGameState == eGameState.running)
        {
            rigid.velocity = transform.forward * movementSpeed * movement;
            transform.Rotate(Vector3.up * rotationSpeed * rotation * 10 * Time.fixedDeltaTime);
        }
    }

    public float GetSpeed()
    {
        return movementSpeed;
    }

    void ResetVelocity()
    {
        if (rigid.velocity == Vector3.zero || GM.CurGameState == eGameState.running)
            return;

        if (GM.CurGameState == eGameState.paused || GM.CurGameState == eGameState.cutscene)
        {
            print("no player input for movement");
            rigid.velocity = Vector3.zero;
        }            
    }
}

