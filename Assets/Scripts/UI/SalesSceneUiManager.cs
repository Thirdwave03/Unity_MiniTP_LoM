using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SalesSceneUiManager : MonoBehaviour
{
    public Button settingButton;

    public Button mainSceneButton;
    public Button purchaseSceneButton;
    public Button innSceneButton;

    public GameObject settingWindow;
    public Button settingCloseButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    public GameObject salesInventoryWindow;
    public Button inventoryReturnButton;

    public UiBulletinBoardManager bulletinBoard;

    public TextMeshProUGUI currentCoin;
    public TextMeshProUGUI inventoryStatus;

    private SalesSceneMsgType messageType;

    public GameObject messageBox;
    public GameObject centerMsg;
    public GameObject loanWindow;

    public GameObject centerMsgCheckBArea;

    public Button centerMsgCheckB;
    public Button centerMsgCloseB;

    public Button loanWindowCloseB;
    public Button loanB;

    public TextLocalizer centerMsgLC;
    public TextLocalizer loanTextLC;

    public TextMeshProUGUI loanCostText;

    public GameObject npcSpecialMerchant;
    public GameObject npcBusinessman;

    public UiSpecialSalesWindow specialSalesWindow;      

    private void Start()
    {        
        AddListeners();
        AddLocalizerActions();
        UpdateSalesSceneDisplay();
        bulletinBoard.SetInitialPosition();
        messageBox.SetActive(false);
        BusinessmanUpdate();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        salesInventoryWindow.SetActive(false);
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;
    }  

    public void UpdateSalesSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.coins.ToString();
        inventoryStatus.text = $"{GameManager.Instance.InventoryOccupancy} / {GameManager.Instance.InventoryCapacity}";
    }

    private void AddListeners()
    {
        settingButton.onClick.AddListener(OnClickSetting);
        mainSceneButton.onClick.AddListener(OnClickMainScene);
        purchaseSceneButton.onClick.AddListener(OnClickPurchaseScene);
        innSceneButton.onClick.AddListener(OnClickInnScene);
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickRestartButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        quitButton.onClick.AddListener(OnClickQuit);
        inventoryReturnButton.onClick.AddListener(OnClickInventoryReturn);
        loanWindowCloseB.onClick.AddListener(OnClickCenterMsgClose);
        centerMsgCloseB.onClick.AddListener(OnClickCenterMsgClose);
        loanB.onClick.AddListener(OnClickLoanButton);
        centerMsgCheckB.onClick.AddListener(OnClickCenterMsgCheck);

        // sliders
        bgmSlider.onValueChanged.AddListener(OnValueChangeBGM);
        sfxSlider.onValueChanged.AddListener(OnValueChangeSFX);
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Raycast blocked by UI");
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (!settingWindow.gameObject.activeSelf && !salesInventoryWindow.gameObject.activeSelf)
                {
                    hit.collider.gameObject.GetComponent<NpcButton>().InvokeOnClick();
                }
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("Raycast blocked by UI_Mobile");
                    return;
                }

                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

                if (hit.collider != null)
                {
                    if (!settingWindow.gameObject.activeSelf && !salesInventoryWindow.gameObject.activeSelf)
                    {
                        NpcButton npcButton = hit.collider.gameObject.GetComponent<NpcButton>();
                        if (npcButton != null)
                        {
                            npcButton.InvokeOnClick();
                        }
                    }
                }
            }
        }
#endif
    }

    private void OnClickTemp()
    {

    }

    private void OnClickSetting()
    {
        settingWindow.SetActive(true);
    }
    private void OnClickSettingClose()
    {
        settingWindow.SetActive(false);
    }
    private void OnClickMainScene()
    {
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickPurchaseScene()
    {
        SceneManager.LoadScene((int)SceneIds.PurchaseScene);
    }

    private void OnClickSalesScene()
    {
        SceneManager.LoadScene((int)SceneIds.SalesScene);
    }

    private void OnClickInnScene()
    {
        SceneManager.LoadScene((int)SceneIds.InnScene);
    }

    private void OnClickRestartButton()
    {
        GameManager.Instance.Restart();
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickMainMenu()
    {
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }
    private void OnClickQuit()
    {
        SaveLoadManager.BaseData.bgmVolume = SoundManager.Instance.BgmVolume;
        SaveLoadManager.BaseData.sfxVolume = SoundManager.Instance.SfxVolume;
        SaveLoadManager.SaveBase();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();        
#endif
    }
    private void AddLocalizerActions()
    {
        loanTextLC.customizedFormat += LocalizerActionLoanText;
    }

    private void LocalizerActionLoanText()
    {
        if (!(GameManager.Instance.ifLent && GameManager.Instance.paybackDateCnt > 0))
        {
            loanTextLC.stringId = 999912;
            loanTextLC.formatContents.Clear();
            loanTextLC.formatContents.Add(GameManager.Instance.lentAmount.ToString());
            loanTextLC.formatContents.Add(GameManager.Instance.paybackDateCnt.ToString());
            loanTextLC.formatContents.Add(GameManager.Instance.lentPaybackAmout.ToString());
        }
    }

    public void OpenSpecialMerchant()
    {
        messageBox.SetActive(true);
        loanWindow.SetActive(false);
        salesInventoryWindow.SetActive(false);
        specialSalesWindow.gameObject.SetActive(true);
        centerMsg.SetActive(false);
    }

    public void OpenBuyer()
    {
        salesInventoryWindow.SetActive(true);
    }

    public void OpenBusinessman()
    {
        if(GameManager.Instance.ifLent && GameManager.Instance.paybackDateCnt == 0)
        {
            OpenMessage(SalesSceneMsgType.LoanPickUp);
        }
        else if(GameManager.Instance.ifLent && GameManager.Instance.paybackDateCnt > 0)
        {
            OpenMessage(SalesSceneMsgType.AlreadyLent);
        }
        else if(!GameManager.Instance.ifLent && GameManager.Instance.paybackDateCnt == 0)
        {
            OpenMessage(SalesSceneMsgType.LoanPickedUp);
        }
        else
        {
            messageBox.SetActive(true);
            loanWindow.SetActive(true);
            salesInventoryWindow.SetActive(false);
            specialSalesWindow.gameObject.SetActive(false);
            centerMsg.SetActive(false);
            loanTextLC.OnChangeLanguage(Variables.currentLanguage);
            loanCostText.text = GameManager.Instance.lentAmount.ToString();
        }
    }

    private void OnClickInventoryReturn()
    {
        salesInventoryWindow.GetComponent<UiSalesPanel>().salesItemInfo.blinder.SetActive(true);
        salesInventoryWindow.SetActive(false);
    }

    public void OpenMessage(SalesSceneMsgType msgType)
    {
        messageBox.SetActive(true);
        messageType = msgType;
        specialSalesWindow.gameObject.SetActive(false);
        switch (msgType)
        {
            case SalesSceneMsgType.InsufficientCoin:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                loanWindow.SetActive(false);
                salesInventoryWindow.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999902);
                break;
            case SalesSceneMsgType.LoanPickUp:
                messageBox.SetActive(true);
                loanWindow.SetActive(false);
                salesInventoryWindow.SetActive(false);
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(true);
                centerMsgLC.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999916), GameManager.Instance.lentPaybackAmout);
                break;
            case SalesSceneMsgType.LoanPickedUp:
                messageBox.SetActive(true);
                loanWindow.SetActive(false);
                centerMsgCheckBArea.SetActive(false);
                salesInventoryWindow.SetActive(false);
                centerMsg.SetActive(true);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999917);
                break;
            case SalesSceneMsgType.AlreadyLent:
                messageBox.SetActive(true);
                loanWindow.SetActive(false);
                centerMsgCheckBArea.SetActive(false);
                salesInventoryWindow.SetActive(false);
                centerMsg.SetActive(true);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999918);
                break;
            default:
                break;
        }
        UpdateSalesSceneDisplay();
    }

    private void OnClickCenterMsgCheck()
    {
        switch (messageType)
        {
            case SalesSceneMsgType.LoanPickUp:
                GameManager.Instance.coins += GameManager.Instance.lentPaybackAmout;
                GameManager.Instance.ifLent = false;
                messageBox.SetActive(false);
                GameManager.Instance.CallSave();
                break;
            default:
                break;
        }
        UpdateSalesSceneDisplay();
    }

    private void OnClickCenterMsgClose()
    {
        messageBox.SetActive(false);
    }

    private void OnClickLoanButton()
    {
        if (GameManager.Instance.coins >= GameManager.Instance.lentAmount)
        {
            GameManager.Instance.coins -= GameManager.Instance.lentAmount;
            GameManager.Instance.ifLent = true;
            OpenMessage(SalesSceneMsgType.AlreadyLent);
            UpdateSalesSceneDisplay();
            GameManager.Instance.CallSave();
        }
        else
        {
            OpenMessage(SalesSceneMsgType.InsufficientCoin);
            UpdateSalesSceneDisplay();
        }
    }
    
    private void BusinessmanUpdate()
    {
        if (GameManager.Instance.ifLent && GameManager.Instance.paybackDateCnt > 0)
        { 
            npcBusinessman.SetActive(false); 
        }
        else if (GameManager.Instance.paybackDateCnt == 0)
        {            
            npcBusinessman.SetActive(true);
        }
        else
        {
            if(GameManager.Instance.isBusinessmanAvailable)
            {
                npcBusinessman.SetActive(true);
            }
            else
            {
                npcBusinessman.SetActive(false);
            }
        }
    }

    public void OnClickSpecialSalesReturn()
    {
        messageBox.SetActive(false);
        specialSalesWindow.gameObject.SetActive(false);
    }

    public void OnValueChangeBGM(float val)
    {
        SoundManager.Instance.BgmVolume = val;
    }

    public void OnValueChangeSFX(float val)
    {
        SoundManager.Instance.SfxVolume = val;
    }
}
