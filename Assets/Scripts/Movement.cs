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

    private float rotation = 0f;
    private float movement = 0f;

    public float SpeedTest;

    void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        GetSpeed();
        rotation = Input.GetAxis("Horizontal");
        movement = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rigid.velocity = transform.forward * MovementSpeed * movement;
        transform.Rotate(Vector3.up * RotationSpeed * rotation * 10 * Time.fixedDeltaTime);
    }

    public float GetSpeed()
    {
        return SpeedTest = Mathf.Abs(rigid.velocity.z);
    }
}

