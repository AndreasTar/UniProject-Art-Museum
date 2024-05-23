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
    readonly int MAX_QUESTIONS = 10;

    Vector2 turn;

    public float speed = 50f;
    public float sensitivity = 3f;
    public CharacterController charCont;

    [SerializeField] float lockedHeight = 0;
    Vector3 direction = Vector3.zero;

    int correctQuestions = 0;

    List<Question> questions = new List<Question>();
    Question currentQuestion;

    // on start, show
    //   - 

    // very stupid and hacky way to do it but yolo lmfao
    public GameObject intro;
    public GameObject controls;
    public GameObject instructions;
    public GameObject escape;
    public GameObject wrong;
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
    public GameObject preexit;

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

        intro.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay( gameObject.GetComponentInChildren<Camera>().transform.position, 
                        gameObject.GetComponentInChildren<Camera>().transform.forward * 140);
        Gizmos.DrawSphere(rayInfo.point, 0.5f);
    }


    // Update is called once per frame
    void Update()
    {
        if (manageInput()) return;

        float horizontal = Input.GetAxisRaw("KB Horizontal");
        float vertical = Input.GetAxisRaw("KB Vertical");

        direction = new Vector3(transform.forward.x, 0, transform.forward.z) * vertical + transform.right * horizontal;

        speed = Input.GetButton("KB Sprint") ? 100f : 50f;

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
        questions.Add(new Question("Βρές ένα πίνακα με μια ξαπλωμένη γυναίκα", new int[] { 3, 43 } ));
        // questions.Add(new Question("Βρές ένα πίνακα που περιέχει 2 παιδιά που παίζουν ποδόσφαιρο", new int[] { 36 } ));
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

        questions.Add(new Question("Βρές ένα πίνακα του Τσαρούχη", new int[] { 28} ));
        questions.Add(new Question("Βρές ένα πίνακα του Φασιανού", new int[] { 17, 23, 24, 25} ));
        questions.Add(new Question("Βρές ένα πίνακα του Μυταρά", new int[] { 47 } ));

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

        string t = "";
        foreach (int i in currentQuestion.correctExhibitIndexes)
        {
            t += i+" ";
        }
        Debug.Log(t);

        if (currentQuestion.correctExhibitIndexes.Contains(ind))
        {
            keyUI[correctQuestions].SetActive(false);
            correctQuestions ++;
            isPlayerActive = false;
            Cursor.lockState = CursorLockMode.Confined;
            if (correctQuestions >= MAX_QUESTIONS)
            {
                // set door as exitable
                currentQuestion = new Question("", new int[]{});
                exitable = true;
                preexit.SetActive(true);
                return;
            }
            Debug.Log("CORRECT " + correctQuestions);
            questions.Remove(currentQuestion);
            currentQuestion = questions[Range(0, questions.Count)];

            // show next question

            GameObject ui = keyUI[correctQuestions];
            ui.GetComponentInChildren<TextMeshProUGUI>(true).SetText(currentQuestion.question);
            ui.SetActive(true);

        }
        else if (correctQuestions < MAX_QUESTIONS)
        {
            wrong.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(deactivateAfterDelay(wrong, 2f));
        }
    }

    void handleDoorInteraction()
    {
        if (exitable)
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
        instructions.SetActive(false);
        controls.SetActive(true);
    }

    public void handleIntro()
    {
        intro.SetActive(false);
        controls.SetActive(true);
    }

    public void handleExit()
    {
        Debug.Log("exiting...");
        Application.Quit();
    }

    bool first = true;

    public void handleControls()
    {
        controls.SetActive(false);

        if (first)
        {
            k0.GetComponentInChildren<TextMeshProUGUI>(true).SetText(currentQuestion.question);
            k0.SetActive(true);
            first = false;
        }
        else
        {
            isPlayerActive = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private IEnumerator deactivateAfterDelay(GameObject g, float seconds) {
        yield return new WaitForSeconds(seconds);
        g.SetActive(false);
    }

    private bool manageInput()
    {

        if (isPlayerActive)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                isPlayerActive = false;
                Cursor.lockState = CursorLockMode.Confined;

                if (correctQuestions < 10) keyUI[correctQuestions].SetActive(false);

                instructions.SetActive(true);
            }

            if (Input.GetButtonDown("KB Question"))
            {
                isPlayerActive = false;
                Cursor.lockState = CursorLockMode.Confined;

                GameObject ui = keyUI[correctQuestions];
                ui.GetComponentInChildren<TextMeshProUGUI>(true).SetText(currentQuestion.question);
                ui.SetActive(true);
            }

            Vector3 pos = gameObject.GetComponentInChildren<Camera>().transform.position;
            Vector3 forw = gameObject.GetComponentInChildren<Camera>().transform.forward;

            if (Input.GetButtonDown("KB Interact"))
            {
                if (Physics.Raycast(pos, forw, out rayInfo, 140.0f))
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

            return false;
        }
        else
        { 
            if (Input.GetButtonDown("Cancel"))
            {
                if (instructions.activeSelf)
                {
                    instructions.SetActive(false);
                    isPlayerActive = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if ((Input.GetButtonDown("KB Interact") || Input.GetButtonDown("KB Question")) && !intro.activeSelf && !instructions.activeSelf && !controls.activeSelf)
            {
                preexit.SetActive(false);
                isPlayerActive = true;
                Cursor.lockState = CursorLockMode.Locked;

                if (correctQuestions < 10 && keyUI[correctQuestions].activeSelf)
                    keyUI[correctQuestions].SetActive(false);
            }

            return true;
        }        
    }
   
}
