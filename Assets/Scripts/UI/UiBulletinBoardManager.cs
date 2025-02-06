using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiBulletinBoardManager : MonoBehaviour
{
    public UiBulletinContentController bulletinContentsPrefab;
    public GameObject viewPort;
    private List<UiBulletinContentController> prefabList;

    private float openedXPos;
    private float closedXPos;

    private bool isOpened = false;
    private bool isMoving = false;
    private float timer = 0.5f;
    private float accumTime = 0f;

    public void SetInitialPosition()
    {
        gameObject.SetActive(true);
        timer = 0.5f;
        accumTime = 0f;
        isOpened = false;
        isMoving = false;
        openedXPos = gameObject.transform.position.x;
        closedXPos = openedXPos - Screen.width * 0.7f;
        gameObject.transform.position = new Vector3(closedXPos, 0, 0);
        SetContents();        
    }

    private void SetContents()
    {
        prefabList = new List<UiBulletinContentController>();
        for(int i = 0; i < GameManager.Instance.bulletinBoardContentsId.Count; ++i)
        {
            var content = Instantiate(bulletinContentsPrefab, viewPort.transform);
            content.SetContentWithId(GameManager.Instance.bulletinBoardContentsId[i]);
            prefabList.Add(content);
        }
    }

    public void OnClick()
    {
        if (!isMoving)
        {
            isOpened = !isOpened;
            accumTime = 0f;
            isMoving = true;
        }
        foreach(var content in prefabList)
        {
            content.ResetSize();
        }
    }

    public void Update()
    {       
        if (isMoving)
        {
            UpdateBulletinBoardPos();
        }

        //for test only
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            prefabList.Add(Instantiate(bulletinContentsPrefab, viewPort.transform));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var item in prefabList)
            {
                Destroy(item.gameObject);
            }
            prefabList.Clear();
        }
    }

    private void UpdateBulletinBoardPos()
    {
        float xPosLerp = 0;

        accumTime += Time.deltaTime;
        if (accumTime >= timer)
        {
            accumTime = timer;
            isMoving = false;
        }
        float calibration = 1 / timer;
        float calibratedAccumTime = calibration * accumTime;        
        
        if (isOpened)
        {
            xPosLerp = Mathf.Lerp(closedXPos, openedXPos, calibratedAccumTime);
        }
        else
        {
            xPosLerp = Mathf.Lerp(openedXPos, closedXPos, calibratedAccumTime);
        }       

        gameObject.transform.position = new Vector3(xPosLerp, 0, 0);
    }
}
