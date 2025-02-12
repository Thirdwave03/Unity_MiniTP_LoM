using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutCellSizeFitter : MonoBehaviour
{
    public RectTransform viewport; // Scroll View의 Viewport를 할당하세요.
    private GridLayoutGroup gridLayoutGroup;
    public float viewportHeight;
    public float verticalPadding;
    public float cellDimension;

    private void Awake()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        StartCoroutine(ResetSizeAfterOneFrame());
     
    }

    private IEnumerator ResetSizeAfterOneFrame()
    {
        yield return null;

        // Viewport의 높이를 가져옵니다.
        viewportHeight = viewport.rect.height;

        // padding (위와 아래)만큼 제외합니다.
        verticalPadding = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

        // 정사각형의 한 변의 길이를 계산합니다.
        cellDimension = viewportHeight - verticalPadding;

        // 계산한 값을 cellSize로 설정합니다.
        gridLayoutGroup.cellSize = new Vector2(cellDimension, cellDimension);
    }

    //// 필요에 따라 Update() 대신 OnRectTransformDimensionsChange() 등으로 최적화 가능
    //void Update()
    //{
    //    // Viewport의 높이를 가져옵니다.
    //    float viewportHeight = viewport.rect.height;
    //
    //    // padding (위와 아래)만큼 제외합니다.
    //    float verticalPadding = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
    //
    //    // 정사각형의 한 변의 길이를 계산합니다.
    //    float cellDimension = viewportHeight - verticalPadding;
    //
    //    // 계산한 값을 cellSize로 설정합니다.
    //    gridLayoutGroup.cellSize = new Vector2(cellDimension, cellDimension);
    //}
}

