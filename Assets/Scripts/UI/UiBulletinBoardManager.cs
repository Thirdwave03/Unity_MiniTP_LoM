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
    private float speed = 600f;

    public void SetInitialPosition()
    {
        isOpened = false;
        openedXPos = gameObject.transform.position.x;
        closedXPos = openedXPos - Screen.width * 0.7f;
        prefabList = new List<UiBulletinContentController>();
        gameObject.transform.position = new Vector3(closedXPos, 0,0);
    }

    public void OnClick()
    {
        isOpened = !isOpened;
    }

    public void Update()
    {
        if(isOpened && gameObject.transform.position.x < openedXPos)
        {
            gameObject.transform.position += new Vector3(Time.deltaTime * speed, 0,0);
        }
        else if (!isOpened && gameObject.transform.position.x > closedXPos)
        {
            gameObject.transform.position -= new Vector3(Time.deltaTime * speed, 0,0);
        }

        //for test only
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            prefabList.Add(Instantiate(bulletinContentsPrefab, viewPort.transform));
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach(var item in prefabList)
            {
                Destroy(item.gameObject);
            }
            prefabList.Clear();
        }
    }


}
