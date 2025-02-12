using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiBulletinBoardManager : MonoBehaviour
{
    private BulletinBoardMsgType messageType;

    public UiBulletinContentController bulletinContentsPrefab;
    public GameObject viewPort;
    private List<UiBulletinContentController> prefabList;

    public GameObject bulletinBoardWindow;
    public GameObject enclopediaWindow;

    public Button bulletinBoardButton;
    public Button enclopediaButton;

    public GameObject encloButtonBlinder;

    public GameObject msgBox;
    public TextMeshProUGUI centerMsg;
    public GameObject centerMsgCheckBArea;
    public Button centerMsgCheckB;
    public Button centerMsgcloseB;

    private float openedXPos;
    private float closedXPos;

    public bool isOpened = false;
    public bool isMoving = false;
    private float timer = 0.5f;
    private float accumTime = 0f;

    public void SetInitialPosition()
    {
        gameObject.SetActive(true);
        msgBox.SetActive(false);
        timer = 0.5f;
        accumTime = 0f;
        isOpened = false;
        isMoving = false;
        openedXPos = gameObject.transform.position.x;
        closedXPos = openedXPos - Screen.width * 0.7f;
        gameObject.transform.position = new Vector3(closedXPos, 0, 0);
        enclopediaWindow.SetActive(false);
        if(SaveLoadManager.BaseData.MerchantRank >= MerchantRanks.PromisingMerchant)
        {
            encloButtonBlinder.SetActive(false);
        }
        else
        {
            encloButtonBlinder.SetActive(true);
        }
        SetContents();
        AddListeners();
    }

    private void AddListeners()
    {
        bulletinBoardButton.onClick.AddListener(OnClickBulletinBoardButton);
        enclopediaButton.onClick.AddListener(OnClickEnclopediaButton);
        centerMsgcloseB.onClick.AddListener(OnClickCloseButton);
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

    public void OnClickBoardHandle()
    {
        if (!isMoving)
        {
            //if(!isOpened)
            //{
            //    isOpened = true;
            //    accumTime = 0f;
            //    isMoving = true;
            //}
            //else
            //{
            //
            //}
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

    private void OnClickBulletinBoardButton()
    {   
        if (!isMoving)
        {
            if (isOpened)
            {
                if (bulletinBoardWindow.activeSelf)
                {
                    isOpened = false;
                    accumTime = 0f;
                    isMoving = true;
                }
                else
                {
                    bulletinBoardWindow.SetActive(true);
                    enclopediaWindow.SetActive(false);
                }
            }
            else
            {
                isOpened = true;
                accumTime = 0f;
                isMoving = true;
            }           
        }
        
        foreach (var content in prefabList)
        {
            content.ResetSize();
        }
    }

    private void OnClickEnclopediaButton()
    {
        if (isOpened)
        {
            if (enclopediaWindow.activeSelf)
            {
                isOpened = false;
                accumTime = 0f;
                isMoving = true;
            }
            else
            {
                bulletinBoardWindow.SetActive(false);
                enclopediaWindow.SetActive(true);
            }            
        }
        else
        {
            isOpened = true;
            accumTime = 0f;
            isMoving = true;
        }
    }

    private void OnClickCloseButton()
    {
        msgBox.SetActive(false);
    }

    public void OpenMessage(BulletinBoardMsgType msgType)
    {
        messageType = msgType;
        msgBox.SetActive(true);

        switch (messageType)
        {
            case BulletinBoardMsgType.LackOfItems:
                centerMsgCheckBArea.SetActive(false);
                centerMsg.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999930);
                break;
        }
    }

    
}
