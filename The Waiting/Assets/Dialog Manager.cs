using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class DialogueManager : MonoBehaviour
{
    // UI-Elemente für den Dialog
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;
    public GameObject optionsPanel;
    public Button optionButtonPrefab;

    // Aktuelle Nachrichten und Schauspieler
    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    public bool isDialogueActive = false;
    private bool showOptionsNext = false;
    private bool areOptionsDisplayed = false;

    // Steuerung für den nächsten Nachrichtenindex
    private bool isNextMessageIndexChanged = false;
    private int nextMessageIndex = -1;

    // Öffnet den Dialog mit Nachrichten und Schauspielern
    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        isDialogueActive = true;
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;

        Debug.Log("Opening dialogue with " + messages.Length);
        DisplayMessage();

        backgroundBox.LeanScale(Vector2.one, 0.5f).setEaseInOutExpo();
    }

    // Zeigt die aktuelle Nachricht an
    private void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        AnimateTextColor();

        if (messageToDisplay.options != null && messageToDisplay.options.Length > 0)
        {
            showOptionsNext = true;
            optionsPanel.SetActive(false);
        }
        else
        {
            optionsPanel.SetActive(false);
        }

        // Überprüfen, ob der nächste Nachrichtenindex geändert werden soll
        if (messageToDisplay.changeNextMessageIndex)
        {
            SetNextMessageIndex(messageToDisplay.newNextMessageIndex);
        }
    }

    // Zeigt die Optionen für die aktuelle Nachricht an
    private void DisplayOptions(Option[] options)
    {
        areOptionsDisplayed = true;
        optionsPanel.SetActive(true);

        // Entfernt alle existierenden Optionselemente
        foreach (Transform child in optionsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Erstellt neue Buttons für jede Option
        foreach (Option option in options)
        {
            Button optionButton = Instantiate(optionButtonPrefab, optionsPanel.transform);
            optionButton.GetComponentInChildren<Text>().text = option.optionText;
            optionButton.onClick.AddListener(() => OnOptionSelected(option.nextMessageIndex));
        }
    }

    // Wird aufgerufen, wenn eine Option ausgewählt wird
    private void OnOptionSelected(int nextMessageIndex)
    {
        areOptionsDisplayed = false;
        activeMessage = nextMessageIndex;
        DisplayMessage();
    }

    // Zeigt die nächste Nachricht an oder beendet den Dialog
    public void NextMessage()
    {
        if (showOptionsNext)
        {
            Message messageToDisplay = currentMessages[activeMessage];
            DisplayOptions(messageToDisplay.options);
            showOptionsNext = false;
        }
        else
        {
            if (isNextMessageIndexChanged)
            {
                activeMessage = nextMessageIndex;
                isNextMessageIndexChanged = false; // Setzt das Flag zurück
            }
            else
            {
                activeMessage++;
            }

            if (activeMessage < currentMessages.Length)
            {
                DisplayMessage();
            }
            else
            {
                Debug.Log("End of dialogue");
                isDialogueActive = false;
                backgroundBox.LeanScale(Vector2.zero, 0.5f).setEaseInOutExpo();
            }
        }
    }

    // Setzt den nächsten Nachrichtenindex
    public void SetNextMessageIndex(int index)
    {
        nextMessageIndex = index;
        isNextMessageIndexChanged = true;
    }

    // Initialisiert den Dialogmanager beim Start
    private void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
        optionsPanel.SetActive(false);
    }

    // Animiert die Textfarbe der Nachricht
    private void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    // Überprüft in jedem Frame, ob der Nutzer weiterklicken möchte
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDialogueActive && !areOptionsDisplayed)
        {
            NextMessage();
        }
    }
}
