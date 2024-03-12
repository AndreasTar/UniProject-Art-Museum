using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 turn;

    public float speed = 5f;
    public CharacterController charCont;
    Vector3 direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor inside the window
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float horizontal = Input.GetAxisRaw("KB Horizontal");
        float vertical = Input.GetAxisRaw("KB Vertical");

        //direction = new Vector3(horizontal, 0, vertical).normalized;
        direction = transform.forward * vertical + transform.right * horizontal;

        charCont.Move(direction.normalized * speed * Time.deltaTime);

        //transform.Translate(direction * speed * Time.deltaTime);
        transform.SetLocalPositionAndRotation(new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);



/*        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.left * (-5) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.forward * (-5) * Time.deltaTime);
        }*/


        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }

}
