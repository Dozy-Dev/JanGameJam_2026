using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float movespeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        if( Input.GetKey(KeyCode.W) )
        {
            movement.z = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement.z = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }

        GetComponent<CharacterController>().Move(movement * Time.deltaTime * movespeed);
    }
}
