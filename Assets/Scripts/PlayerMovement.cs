using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Random;


public class PlayerMovement : MonoBehaviour
{

    Vector2 turn;

    public float speed = 50f;
    public float sensitivity = 3f;
    public CharacterController charCont;

    [SerializeField] float lockedHeight = 0;
    Vector3 direction = Vector3.zero;

    int correctQuestions = 0;
    bool firstInteraction = true;

    //Question[] questions;
    List<Question> questions = new List<Question>();
    Question currentQuestion;

    Vector3 pos;    // FOR DEBUG, WILL REMOVE
    Vector3 rot;    // FOR DEBUG, WILL REMOVE
    Vector3 forw;   // FOR DEBUG, WILL REMOVE

    // very stupid and hacky way to do it but yolo lmfao
    public GameObject intro;
    public GameObject instr;
    public GameObject exit;

    public GameObject k0;
    public GameObject k1;
    public GameObject k2;
    public GameObject k3;
    public GameObject k4;
    public GameObject k5;
    public GameObject k6;
    public GameObject k7;
    public GameObject k8;
    public GameObject k9;

    GameObject[] keyUI;

    bool isPlayerActive = false;
    bool exitable = false;

    RaycastHit rayInfo;


    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor inside the window
        Cursor.lockState = CursorLockMode.Confined;
        keyUI = new GameObject[10]{ k0, k1, k2, k3, k4, k5, k6, k7, k8, k9 };

        makeAllQuestions();

        instr.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(pos, forw*80);
        Gizmos.DrawSphere(rayInfo.point, 0.5f);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isPlayerActive)
        {
            if (Input.GetButtonDown("KB Interact") && !intro.activeSelf && !instr.activeSelf)
            {
                keyUI[correctQuestions].SetActive(false);
                isPlayerActive = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            return;
        }

        pos = gameObject.GetComponentInChildren<Camera>().transform.position;               // FOR DEBUG, WILL REMOVE
        rot = transform.rotation.eulerAngles;   // FOR DEBUG, WILL REMOVE
        forw = gameObject.GetComponentInChildren<Camera>().transform.forward;               // FOR DEBUG, WILL REMOVE

        if (Input.GetButtonDown("KB Interact"))
        {
            Debug.Log("hmmmmmmm?");
            if (Physics.Raycast(pos, forw, out rayInfo, 80.0f))
            {
                Debug.Log("pew");
                if (rayInfo.transform.gameObject.CompareTag("Exhibit"))
                {
                    // store the painting info for the question
                    handleExhibitInteraction(rayInfo.transform.gameObject);
                } 
                else if (rayInfo.transform.gameObject.CompareTag("Door"))
                {
                    handleDoorInteraction();
                }
                else if (rayInfo.transform.gameObject.CompareTag("Player"))
                {
                    Debug.Log("hitting the player");
                }
                else { 
                    Debug.Log("idk what i hit"); 
                }
            }
        }

        float horizontal = Input.GetAxisRaw("KB Horizontal");
        float vertical = Input.GetAxisRaw("KB Vertical");

        direction = new Vector3(transform.forward.x, 0, transform.forward.z) * vertical + transform.right * horizontal;

        charCont.Move(direction.normalized * speed * Time.deltaTime);

        turn.x += sensitivity * Input.GetAxis("Mouse X");
        if (turn.x > 370) turn.x = 10;
        if (turn.x < 0) turn.x = 360;
        turn.y += sensitivity * Input.GetAxis("Mouse Y");
        turn.y = Mathf.Clamp(turn.y, -65.0f, 65.0f);
        transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);

        transform.SetLocalPositionAndRotation(
            new Vector3(transform.position.x, lockedHeight, transform.position.z),
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
        questions.Add(new Question("find a painting with a kokori", new int[] { 9 } ));

        // at the end
        currentQuestion = questions[Range(0, questions.Count)];

    }

    void handleExhibitInteraction(GameObject go)
    {
        string name = go.name;

        name = name.TrimStart("painting");
        name = name.TrimStart("tag");

        int ind = Int16.Parse(name);
        Debug.Log("LOOKING " + ind);
        Debug.Log(currentQuestion.correctExhibitIndexes);

        if (currentQuestion.correctExhibitIndexes.Contains(ind))
        {
            keyUI[correctQuestions].SetActive(false);
            correctQuestions ++;
            if (correctQuestions == 10) // random number
            {
                // set door as exitable
                currentQuestion = new Question("", new int[]{});
                exitable = true;
                return;
            }
            Debug.Log("CORRECT " + correctQuestions);
            questions.Remove(currentQuestion);
            currentQuestion = questions[Range(0, questions.Count)];

            // show next question
            GameObject ui = keyUI[correctQuestions];
            ui.GetComponentInChildren<TextMeshPro>(true).SetText(currentQuestion.question);
            ui.SetActive(true);

        }
    }

    void handleDoorInteraction()
    {
        if (firstInteraction) {

            Cursor.lockState = CursorLockMode.Confined;
            isPlayerActive = false;

            intro.SetActive(true);

            firstInteraction = false;
            // intro text displayed here
            //StartCoroutine(deactivateAfterDelay(intro,4));
        }
        else if (exitable)
        {
            // if all questions answered, show exit and exit
            Cursor.lockState = CursorLockMode.Confined;
            isPlayerActive = false;

            k9.SetActive(false);
            exit.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            isPlayerActive = false;

            keyUI[correctQuestions].SetActive(true);
        }
    }

    public void handleInstructions()
    {
        instr.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isPlayerActive = true;
    }

    public void handleIntro()
    {
        intro.SetActive(false);

        k0.GetComponentInChildren<TextMeshProUGUI>(true).SetText(currentQuestion.question);
        k0.SetActive(true);

        //isPlayerActive = true;
        //Cursor.lockState = CursorLockMode.Locked;

    }

    public void handleExit()
    {
        //temp
        Debug.Log("exititiiningigg");
        Application.Quit();
        
    }

    private IEnumerator deactivateAfterDelay(GameObject g, int seconds) {
        yield return new WaitForSeconds(seconds);
        g.SetActive(false);
    }
   
}
