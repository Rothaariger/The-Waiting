using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Array von Nachrichten f�r den Dialog
    public Message[] messages;

    // Array von Schauspielern, die im Dialog vorkommen
    public Actor[] actors;

    // Startet den Dialog, indem er an den DialogueManager weitergeleitet wird
    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
    }

    // Setzt den Index der n�chsten Nachricht im DialogueManager
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

    // Gibt an, ob der n�chste Nachrichtenindex ge�ndert werden soll
    public bool changeNextMessageIndex = false;

    // Der neue Nachrichtenindex, falls ge�ndert
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

    // Index der n�chsten Nachricht, wenn die Option ausgew�hlt wird
    public int nextMessageIndex;
}
