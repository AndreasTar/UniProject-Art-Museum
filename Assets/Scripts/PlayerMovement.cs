using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 turn;

    public float speed = 50f;
    public float sensitivity = 4f;
    public CharacterController charCont;
    [SerializeField] float lockedHeight = 0;
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

        direction = transform.forward * vertical + transform.right * horizontal;

        charCont.Move(direction.normalized * speed * Time.deltaTime);

        turn.x += sensitivity * Input.GetAxis("Mouse X");
        turn.y += sensitivity * Input.GetAxis("Mouse Y");
        turn.y = Mathf.Clamp(turn.y, -65.0f, 65.0f);
        transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);

        transform.SetLocalPositionAndRotation(new Vector3(transform.position.x, lockedHeight, transform.position.z),
            transform.rotation
        );

    }

}
