using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private Line[] lines;

    private TextMeshProUGUI textGui;
    private int currentTextLine;
    private EventListener dialogueAdvanceListener;

    private Coroutine textCoroutine;

    private bool advanceOnPlayerInput = false;

    public UnityEvent DialogueFinished = new();
    public UnityEvent<int> TextAdvanced = new();
    [SerializeField] private float delay = 0.05f;
    [Tooltip("If this is set to true, the text will consider the last line as the end of the dialogue and will continue showing the text after finishing")]
    [SerializeField] private bool stopOnLastLine = false;

    // Update is called once per frame
    void Awake()
    {
        dialogueAdvanceListener = GetComponent<EventListener>();
        textGui = GetComponent<TextMeshProUGUI>();
        currentTextLine = -1;
        AdvanceText();

        dialogueAdvanceListener.response.AddListener(AdvanceText);
    }

    private void AdvanceText()
    {
        currentTextLine++;
        if (currentTextLine == lines.Length && stopOnLastLine)
        {
            DialogueFinished?.Invoke();
            if (!stopOnLastLine)
            {
                if (textCoroutine != null)
                {
                    StopCoroutine(textCoroutine);
                }
                textGui.text = "";
            }
        }
        else if (currentTextLine < lines.Length)
        {
            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
            }
            textCoroutine = StartCoroutine(GraduallyAddText(lines[currentTextLine].text));
            TextAdvanced?.Invoke(currentTextLine);
            if (lines[currentTextLine].alternateTextAdvanceEvent != null)
            {
                advanceOnPlayerInput = false;
                dialogueAdvanceListener.setBroadcastToListen(lines[currentTextLine].alternateTextAdvanceEvent);
            } else
            {
                advanceOnPlayerInput = true;
                dialogueAdvanceListener.disableBroadcastListening();
            } 
        }
    }

    private IEnumerator GraduallyAddText(string displayText)
    {
        textGui.text = "";
        for (int i = 0; i < displayText.Length; i++)
        {
            textGui.text += displayText[i];
            yield return new WaitForSeconds(delay);
        }
    }

    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        if (advanceOnPlayerInput)
        {
            AdvanceText();
        }
    }

    void OnEnable()
    {
        StartCoroutine(WaitToBindAction());

        // when a scene is initially loaded, we have to wait until Awake() on Pluto is called before we can access the input handler
        // so I just wait for a bit until the action is bound
        IEnumerator WaitToBindAction() {
            yield return new WaitForEndOfFrame();
            GameManager.instance.pluto.plutoInputHandler.playerInputActions.Pluto.Interact.performed += HandleInteractInput;
        }
    }

    private void OnDisable()
    {
        GameManager.instance.pluto.plutoInputHandler.playerInputActions.Pluto.Interact.performed -= HandleInteractInput;
    }
}

[System.Serializable]
struct Line
{
    public string text;
    public BroadcastEvent alternateTextAdvanceEvent;
}