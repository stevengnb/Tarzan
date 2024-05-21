using UnityEngine;
using System.Collections;
using Cinemachine;
using TMPro;

public class NpcInteract : MonoBehaviour
{
    private bool interacting = false;
    private bool inRange = false;
    [SerializeField] private GameObject ShowInteract;
    [SerializeField] private GameObject InteractName;
    [SerializeField] private GameObject InteractButton;
    [SerializeField] private TextMeshProUGUI Interact;

    [SerializeField] private CinemachineVirtualCamera normal;
    [SerializeField] private CinemachineVirtualCamera npc;
    private PlayerMovement playerMovement;

    private string interact1Text = "Welcome 23-2!";
    private string interact2Text = "Do you want to help the village to gain some experience point?";
    private float typingSpeed = 0.05f;
    private float typingSpeed2 = 0.005f;
    private float deleteSpeed = 0.0015f;

    [SerializeField] private Animator animator;
    private int isTalkHash;
    private int isTalkHash1;

    private GameState currentGameState;

    private void Awake()
    {   
        playerMovement = FindObjectOfType<PlayerMovement>();
        animator = GetComponent<Animator>();
        isTalkHash = Animator.StringToHash("isTalk");
        isTalkHash1 = Animator.StringToHash("isTalk2");
        npc.Priority = 0;
    }

    private void Start()
    {
        currentGameState = TowerDefense.instance.CurrState;
    }

    private void Update()
    {
        if(TowerDefense.instance.isChanging)
        {
            currentGameState = TowerDefense.instance.CurrState;
            TowerDefense.instance.isChanging = false;
        }

        if (currentGameState == GameState.NormalMode)
        {
            if (inRange && Input.GetKeyUp(KeyCode.F))
            {
                if (!interacting)
                {
                    StartInteract();
                }
                else if (Input.GetKeyUp(KeyCode.F) && interacting)
                {
                    StopInteract();
                }
            }
        }
    }

    public void StartInteract()
    {
        normal.Priority = 0;
        npc.Priority = 10;
        playerMovement.enabled = false;
        interacting = true;

        StartCoroutine(InteractNameTrue(0.7f));
        StartCoroutine(InteractTwoTrue(4.2f));
        StartCoroutine(InteractButtonTrue(5f));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopInteract()
    {
        normal.Priority = 10;
        npc.Priority = 0;
        playerMovement.enabled = true;
        interacting = false;

        Interact.enabled = false;
        Interact.text = "";
        InteractName.SetActive(false);
        InteractButton.SetActive(false);
        animator.SetBool(isTalkHash, false);
        animator.SetBool(isTalkHash1, false);

        StopAllCoroutines();
        AudioManagerGame.instance.StopTalking();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator InteractNameTrue(float duration)
    {
        yield return new WaitForSeconds(duration);
        InteractName.SetActive(true);
        Interact.enabled = true;
        AudioManagerGame.instance.Talking(0);
        StartCoroutine(TextAnimationAdd(interact1Text, typingSpeed));
        StartCoroutine(TextAnimationMin(interact1Text));
        animator.SetBool(isTalkHash, true);
    }

    private IEnumerator TextAnimationAdd(string text, float speed)
    {
        foreach(char c in text)
        {
            Interact.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    private IEnumerator TextAnimationMin(string text)
    {
        yield return new WaitForSeconds(2.5f);
        int textLength = text.Length;
        for (int i = textLength; i >= 0; i--)
        {
            Interact.text = text.Substring(0, i);
            yield return new WaitForSeconds(deleteSpeed);
        }
    }

    private IEnumerator InteractTwoTrue(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isTalkHash1, true);
        AudioManagerGame.instance.Talking(1);
        StartCoroutine(TextAnimationAdd(interact2Text, typingSpeed2));
    }

    private IEnumerator InteractButtonTrue(float duration)
    {
        yield return new WaitForSeconds(duration);
        InteractButton.SetActive(true);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!interacting)
            {
                ShowInteract.SetActive(true);
            }
            else
            {
                ShowInteract.SetActive(false);
            }
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ShowInteract.SetActive(false);
        inRange = false;
    }
}
