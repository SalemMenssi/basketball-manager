using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    private GridLayoutGroup gridLayout;

    void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        SpawnItems();
        ExpandGridWidth();
        SetPivotToMiddleLeft();
    }

    private void SpawnItems()
    {
        for (int i = 0; i < 40; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, transform);
            newItem.name = "Item " + (i + 1);
        }
    }

    private void ExpandGridWidth()
    {
        int childCount = transform.childCount;
        int rowCount = Mathf.CeilToInt((float)childCount / gridLayout.constraintCount);
        float totalWidth = (gridLayout.cellSize.x - gridLayout.spacing.x*2) * rowCount;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x += totalWidth; 
        rectTransform.sizeDelta = sizeDelta;
    }

    private void SetPivotToMiddleLeft()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 pivot = new Vector2(0f, 0.5f); 
        rectTransform.pivot = pivot;
    }
}
