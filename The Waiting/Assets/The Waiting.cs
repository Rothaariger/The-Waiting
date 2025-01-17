using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour


{
    private int keyCount = 0;

    public GameObject canvas;
    // Position der Maus auf dem Bildschirm
    public Vector3 mousePosition;

    // Referenz auf die Hauptkamera
    public Camera mainCamera;

    // Weltkoordinaten der Mausposition
    public Vector3 mousePositionWorld;

    // 2D-Version der Weltkoordinaten der Mausposition
    public Vector2 mousePositionWorld2D;

    // Treffer bei einem Raycast im 2D-Raum
    private RaycastHit2D hit2D;

    // Referenz auf das Spieler-GameObject
    public GameObject player;

    // Zielposition, zu der sich der Spieler bewegen soll
    public Vector2 target;

    // Bewegungsgeschwindigkeit des Spielers
    public float speed = 0.1f;

    // Gibt an, ob der Spieler sich gerade bewegt
    public bool isMoving = false;

    // Anzahl gesammelter Pillen
    public int pills = 0;

    // Anzahl gesammelter Keys
    public int Key = 0;

    // Callback, das aufgerufen wird, wenn das Ziel erreicht ist
    private System.Action targetCallback;

    // Layer-Maske zur Bestimmung der kollidierbaren Objekte
    public LayerMask layerMask;

    // Referenz auf den Dialog-Trigger
    public DialogueTrigger dialogueTrigger;

    // Referenz auf den Dialog-Manager
    public DialogueManager dialogueManager;

    // Wird beim Start des Spiels einmal aufgerufen
    void Start()
    {
        // Initialisiere das Ziel als aktuelle Position des Spielers
        target = player.transform.position;
    }

    // Wird in jedem Frame aufgerufen
    void Update()
    {
        // Wenn ein Dialog aktiv ist, keine Eingaben verarbeiten
        if (dialogueManager != null && dialogueManager.isDialogueActive)
        {
            return;
        }

        // Überprüfe, ob die linke Maustaste gedrückt wurde
        if (Input.GetMouseButtonDown(0))
        {
            // Erhalte die Mausposition und konvertiere sie in Weltkoordinaten
            mousePosition = Input.mousePosition;
            mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);
            mousePositionWorld2D = new Vector2(mousePositionWorld.x, mousePositionWorld.y);

            // Führe einen 2D-Raycast aus
            hit2D = Physics2D.Raycast(mousePositionWorld2D, Vector2.zero, Mathf.Infinity, ~layerMask);

            if (hit2D.collider != null)
            {
                Debug.Log(hit2D.collider.gameObject.name);
                // Verarbeite die verschiedenen Tags, die getroffen wurden
                switch (hit2D.collider.gameObject.tag)
                {
                    case "floor":
                        target = hit2D.point;
                        isMoving = true;
                        FlipSprite();
                        break;

                    case "pills":
                        MoveToTargetBox(hit2D.collider.gameObject.transform.position, hit2D.collider, () =>
                        {
                            hit2D.collider.gameObject.SetActive(false);
                            pills++;
                        });
                        break;

                    case "Key":
                        MoveToTargetBox(hit2D.collider.gameObject.transform.position, hit2D.collider, () =>
                        {
                            hit2D.collider.gameObject.SetActive(false);
                            Transform childTransform = canvas.transform.GetChild(keyCount);
                            childTransform.gameObject.SetActive(true);
                            Key++;
                            keyCount++;
                            Debug.Log(hit2D.collider.gameObject.name);
                        });
                        break;

                    case "NPC":
                        MoveToTargetBox(hit2D.collider.gameObject.transform.position, hit2D.collider, () =>
                        {
                            dialogueTrigger.StartDialogue();
                        });
                        break;

                    case "Chamber1":
                        MoveToTargetBox(hit2D.collider.gameObject.transform.position, hit2D.collider, () =>
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        });
                        break;

                    default:
                        break;
                }
            }
        }
    }

    // Funktion zum Umdrehen des Sprites basierend auf der Zielposition
    void FlipSprite()
    {
        if (player.transform.position.x < target.x)
        {
            player.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Physik-Update, wird in regelmäßigen Abständen aufgerufen
    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Bewege den Spieler in Richtung des Ziels
            player.transform.position = Vector2.MoveTowards(player.transform.position, target, speed);

            // Überprüfe, ob das Ziel erreicht wurde
            if (Vector2.Distance(player.transform.position, target) < 0.01f)
            {
                isMoving = false;
                targetCallback?.Invoke();
                targetCallback = null;
            }
        }
    }

    // Bewege den Spieler zu einer Zielposition und führe eine Aktion aus, wenn das Ziel erreicht ist
    private void MoveToTargetBox(Vector2 targetPosition, Collider2D targetCollider, System.Action onReach)
    {
        // Berechne die Richtung und die angepasste Zielposition
        Vector2 direction = (targetPosition - (Vector2)player.transform.position).normalized;
        Vector2 adjustedTarget = targetPosition - direction * (targetCollider.bounds.extents.magnitude + player.GetComponent<Collider2D>().bounds.extents.magnitude);

        target = adjustedTarget;
        isMoving = true;
        targetCallback = onReach;
        FlipSprite();
    }
}


