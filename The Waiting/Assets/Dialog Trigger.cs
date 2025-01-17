using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Array von Nachrichten für den Dialog
    public Message[] messages;

    // Array von Schauspielern, die im Dialog vorkommen
    public Actor[] actors;

    // Startet den Dialog, indem er an den DialogueManager weitergeleitet wird
    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
    }

    // Setzt den Index der nächsten Nachricht im DialogueManager
    public void SetNextMessageIndex(int index)
    {
        FindObjectOfType<DialogueManager>().SetNextMessageIndex(index);
    }
}

[System.Serializable]
public class Message
{
    // ID des Schauspielers, der die Nachricht spricht
    public int actorID;

    // Inhalt der Nachricht
    public string message;

    // Optionen, die dem Spieler zur Auswahl stehen
    public Option[] options;

    // Gibt an, ob der nächste Nachrichtenindex geändert werden soll
    public bool changeNextMessageIndex = false;

    // Der neue Nachrichtenindex, falls geändert
    public int newNextMessageIndex = -1;
}

[System.Serializable]
public class Actor
{
    // Name des Schauspielers
    public string name;

    // Sprite des Schauspielers
    public Sprite sprite;
}

[System.Serializable]
public class Option
{
    // Text, der auf dem Button der Option angezeigt wird
    public string optionText;

    // Index der nächsten Nachricht, wenn die Option ausgewählt wird
    public int nextMessageIndex;
}
