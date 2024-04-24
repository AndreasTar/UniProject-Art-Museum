using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    Vector2 turn;

    public float speed = 50f;
    public float sensitivity = 4f;
    public CharacterController charCont;
    [SerializeField] float lockedHeight = 0;
    Vector3 direction = Vector3.zero;

    int correctQuestions = 0;
    bool firstInteraction = true;
    int lastInteractedExhibitIndex = -1;

    Question[] questions;
    Question currentQuestion;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor inside the window
        Cursor.lockState = CursorLockMode.Locked;
        makeAllQuestions();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit rayInfo;
            Debug.Log("sending ray");

            if (Physics.Raycast(transform.position, transform.forward, out rayInfo, 2.0f))
            {
                if (rayInfo.transform.CompareTag("Exhibit"))
                {
                    Debug.Log("found exhibit");
                    // store the painting info for the question
                    handleExhibitInteraction();
                } 
                else if (rayInfo.transform.CompareTag("Door"))
                {
                    // if first time, show message
                    Debug.Log("found door");
                    // if not first time, compare stored painting, show question etc
                    handleDoorInteraction();
                } else {
                    Debug.Log("ray found nothing");
                }
            }
            
        }

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

    struct Question
    {
        public string question { get; }
        public int[] correctExhibitIndexes { get; }

        public Question(string q, int[] i ) : this()
        {
            question = q;
            correctExhibitIndexes = i;
        }
    }

    void makeAllQuestions()
    {
        questions[0] = new Question("find a painting with a kokori", new int[] { 9 });
    }

    void handleExhibitInteraction()
    {
        
    }

    void handleDoorInteraction()
    {
        if (firstInteraction) {
            GameObject canvasObject = GameObject.FindWithTag("Intro_Canvas");
            
            Debug.Log("handle door interaction");

            if (canvasObject != null) {
                Canvas canvas = canvasObject.GetComponent<Canvas>();

                Debug.Log("found canvas object");

                if (canvas != null) {
                    Debug.Log("in");
                    Text intro_text = canvas.GetComponentInChildren<Text>();
                    intro_text.text = "something something";

                    firstInteraction = false;
                }
            }
        }

        if (currentQuestion.correctExhibitIndexes.Contains(lastInteractedExhibitIndex))
        {
            correctQuestions++;
            // set next question by changing currentQuestion
        }

        if (correctQuestions >= 10)
        {
            // show exit ui and exit
        }

        // show question info etc
    }
}
