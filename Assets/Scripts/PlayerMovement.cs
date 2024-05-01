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

    // very stupid and hacky way to do it but yolo lmfao
    public GameObject intro;
    public GameObject instr;
    public GameObject exit;
    public GameObject wrong;
    // cheems addition
    public GameObject press_door;

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
        Gizmos.DrawRay( gameObject.GetComponentInChildren<Camera>().transform.position, 
                        gameObject.GetComponentInChildren<Camera>().transform.forward * 80);
        Gizmos.DrawSphere(rayInfo.point, 0.5f);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

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

        Vector3 pos = gameObject.GetComponentInChildren<Camera>().transform.position;
        Vector3 forw = gameObject.GetComponentInChildren<Camera>().transform.forward;

        if (Input.GetButtonDown("KB Interact"))
        {
            if (Physics.Raycast(pos, forw, out rayInfo, 80.0f))
            {
                if (rayInfo.transform.gameObject.CompareTag("Exhibit"))
                {
                    // store the painting info for the question
                    handleExhibitInteraction(rayInfo.transform.gameObject);
                } 
                else if (rayInfo.transform.gameObject.CompareTag("Door"))
                {
                    handleDoorInteraction();
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
        // cheems addition
        questions.Add(new Question("Βρες ένα πίνακα που απεικονίζει ένα κόκορα", new int[] { 9 } ));
        questions.Add(new Question("Βρές ένα πίνακα με λουλούδια", new int[] {29, 20, 8} ));
        questions.Add(new Question("Βρές ένα πίνακα με μια μοτοσικλέτα", new int[] { 41 } ));
        questions.Add(new Question("Βρές ένα πίνακα με 3 πορτρέτα", new int[] { 1, 31, 42 } ));
        questions.Add(new Question("Βρές ένα πίνακα με μια ξαπλωμένη γυναίκα", new int[] { 2, 43 } ));
        questions.Add(new Question("Βρές ένα πίνακα που περιέχει 2 παιδιά που παίζουν ποδόσφαιρο", new int[] { 36 } ));
        questions.Add(new Question("Βρές ένα πίνακα με μια βάρκα έξω από το νερό", new int[] { 37 } ));
        questions.Add(new Question("Βρές ένα πίνακα με ένα ναύτη", new int[] { 40, 5 } ));
        questions.Add(new Question("Βρές ένα πίνακα με 3 γυναικείες μορφές", new int[] { 42 } ));
        questions.Add(new Question("Βρές ένα πίνακα με ένα αυτοκίνητο Formula 1", new int[] { 45 } ));
        questions.Add(new Question("Βρές ένα πίνακα με ένα ζευγάρι", new int[] { 48, 17 } ));
        questions.Add(new Question("Βρές ένα πίνακα με κεντρικό θέμα κάποια έπιπλα", new int[] { 49 } ));
        questions.Add(new Question("Βρές ένα πίνακα που απεικονίζει μια ναυμαχία", new int[] { 50 } ));
        questions.Add(new Question("Βρές ένα πίνακα με φρούτα", new int[] { 51 } ));
        questions.Add(new Question("Βρές ένα πίνακα με μια γυναίκα σε ένα ποδήλατο", new int[] { 22 } ));
        questions.Add(new Question("Βρές ένα πίνακα με χρυσό χρώμα", new int[] { 17 } ));
        questions.Add(new Question("Βρές ένα πίνακα με μια γυναίκα σε προφίλ", new int[] { 16 } ));

        questions.Add(new Question("Βρές ένα πίνακα ζωγραφισμένο με πενάκι", new int[] { 1, 2, 18} ));
        questions.Add(new Question("Βρές ένα πίνακα ζωγραφισμένο με ακρυλικό", new int[] { 17, 23, 24, 25, 32, 34, 37, 41, 45, 48} ));

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
        Debug.Log(currentQuestion.correctExhibitIndexes.ToString());

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
        else
        {
            // cheems modification 
            if (!firstInteraction) {
                wrong.SetActive(true);
                StopAllCoroutines();
                StartCoroutine(deactivateAfterDelay(wrong, 2f));
            } else {
                // cheems addition
                print("go press the door");
                press_door.SetActive(true);
                StopAllCoroutines();
                StartCoroutine(deactivateAfterDelay(press_door, 2f));
            }
            
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

    private IEnumerator deactivateAfterDelay(GameObject g, float seconds) {
        yield return new WaitForSeconds(seconds);
        g.SetActive(false);
    }
   
}
