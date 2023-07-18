using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
    public GameObject arrowPrefab;

    private GameObject gameManager;
    private Player  playerData;
    private GameObject arrowInstance;
    private Card cardData;
    private DecksAndDraw drawManager;
    private CardListener fightRoom;
    private ArrowCollisions arrowCollisions;
    private GridLayoutGroup gridLayoutGroup;
    private Vector3 initialMousePosition;
    private Vector2 initialPosition;
    private bool dragging;
    private bool isColliding = true;

    public void Start()
    {
        gameManager = GameObject.Find("Game Manager");
        playerData = GameObject.Find("Player").GetComponent<Player>();
        drawManager = gameManager.GetComponent<DecksAndDraw>();
        fightRoom = gameManager.GetComponent<CardListener>();

        cardData = GetComponent<CardHUD>().card;


        // Calculate the initial position of the card and activate BoxCollider2D
        if (cardData.target == Target.Player)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            gridLayoutGroup = rectTransform.GetComponentInParent<GridLayoutGroup>();

            Vector2 cellSizeWithSpacing = new Vector2(
                gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x,
                gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y
            );

            int columnCount = gridLayoutGroup.constraintCount;
            int rowIndex = rectTransform.GetSiblingIndex() / columnCount;
            int columnIndex = rectTransform.GetSiblingIndex() % columnCount;

            Vector2 localPosition = new Vector2(
                columnCount > 1 ? columnIndex * cellSizeWithSpacing.x : 0f,
                -rowIndex * cellSizeWithSpacing.y
            );

            Vector2 initialPosition = rectTransform.parent.TransformPoint(localPosition);

            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cardData.target == Target.Enemy && arrowInstance == null)
        {
            arrowInstance = Instantiate(arrowPrefab, transform);
            arrowCollisions = arrowInstance.GetComponent<Arrow.Bezier.BezierArrows>().arrowHeadInstance.GetComponent<ArrowCollisions>();
            arrowInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            initialMousePosition = Input.mousePosition;
            
        }

        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging && cardData.target == Target.Player)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;

        if (arrowCollisions != null && arrowCollisions.IsColliding)
        {
            Debug.Log("Colisión detectada");

            if (arrowCollisions.enemy != null)
            {
                fightRoom.CallApplications(cardData, playerData, arrowCollisions.enemy);
            } 
            
            if (!arrowCollisions.IsColliding)
            {
                arrowCollisions.enemy = null;
            }

            drawManager.MoveOneToDiscardDeck(gameObject);

        }

        // Cards with target = Self
        if (isColliding == false)
        {
            fightRoom.CallApplications(cardData, playerData);
            drawManager.MoveOneToDiscardDeck(gameObject);
        } 
        else if (cardData.target == Target.Player)
        {
            
            GetComponent<RectTransform>().anchoredPosition = initialPosition;

            if (gridLayoutGroup != null)
            {
                gridLayoutGroup.enabled = false;
                gridLayoutGroup.enabled = true;
            }
        }

        Destroy(arrowInstance);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isColliding = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
    }

    
}
