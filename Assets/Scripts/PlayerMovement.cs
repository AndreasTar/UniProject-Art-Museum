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
    public float sensitivity = 3f;
    public CharacterController charCont;

    public GameObject canvas_intro;

    [SerializeField] float lockedHeight = 0;
    Vector3 direction = Vector3.zero;

    int correctQuestions = 0;
    bool firstInteraction = true;
    int lastInteractedExhibitIndex = -1;

    Question[] questions;
    Question currentQuestion;

    Vector3 pos;    // FOR DEBUG, WILL REMOVE
    Vector3 rot;    // FOR DEBUG, WILL REMOVE
    Vector3 forw;   // FOR DEBUG, WILL REMOVE

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor inside the window
        Cursor.lockState = CursorLockMode.Locked;

        // cheems addition
        canvas_intro = GameObject.FindWithTag("Intro_Canvas");
        if (canvas_intro != null){
            canvas_intro.SetActive(false);
        }

        makeAllQuestions();
    }

    private void OnDrawGizmos()
    {
        //print("drawing gizmo");
        //Gizmos.DrawLine(transform.position, transform.forward * 30);
        Gizmos.DrawRay(pos, forw*30);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;               // FOR DEBUG, WILL REMOVE
        rot = transform.rotation.eulerAngles;   // FOR DEBUG, WILL REMOVE
        forw = transform.forward;               // FOR DEBUG, WILL REMOVE

        if (Input.GetButtonDown("KB Interact"))
        {
            RaycastHit rayInfo;

            if (Physics.Raycast(pos, forw, out rayInfo, 80.0f))
            {
                Debug.Log("HIT");
                if (rayInfo.transform.gameObject.CompareTag("Exhibit"))
                {
                    Debug.Log("found exhibit");
                    // store the painting info for the question
                    handleExhibitInteraction();
                } 
                else if (rayInfo.transform.gameObject.CompareTag("Door"))
                {
                    // if first time, show message
                    Debug.Log("found door");

                    // if not first time, compare stored painting, show question etc
                    handleDoorInteraction();
                }
            }
        }

        float horizontal = Input.GetAxisRaw("KB Horizontal");
        float vertical = Input.GetAxisRaw("KB Vertical");

        direction = transform.forward * vertical + transform.right * horizontal;

        charCont.Move(direction.normalized * speed * Time.deltaTime);

        turn.x += sensitivity * Input.GetAxis("Mouse X");
        if (turn.x > 370) turn.x = 10;
        if (turn.x < 0) turn.x = 360;
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
        questions.Append(new Question("find a painting with a kokori", new int[] { 9 } ));
    }

    void handleExhibitInteraction()
    {
        Debug.Log("handle exhibit");
    }

    void handleDoorInteraction()
    {
        if (firstInteraction) {            

            if (canvas_intro != null) {
                canvas_intro.SetActive(true);

                firstInteraction = false;
                // intro text displayed here
                StartCoroutine(intro_delay(canvas_intro,4));
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

    private IEnumerator intro_delay(GameObject g, int seconds) {
        yield return new WaitForSeconds(seconds);
        g.SetActive(false);
    }
   
}
