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

    private void Start()
    {        
        AddListeners();
        UpdateSalesSceneDisplay();
        bulletinBoard.SetInitialPosition();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        salesInventoryWindow.SetActive(false);
    }  

    public void UpdateSalesSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.Coins.ToString();
        inventoryStatus.text = $"{GameManager.Instance.InventoryOccupancy} / {GameManager.Instance.inventoryCapacity}";
    }

    private void AddListeners()
    {
        //inventoryButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        purchaseSceneButton.onClick.RemoveAllListeners();
        innSceneButton.onClick.RemoveAllListeners();
        settingCloseButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        inventoryReturnButton.onClick.RemoveAllListeners();

        //inventoryButton.onClick.AddListener(OnClickTemp);
        settingButton.onClick.AddListener(OnClickSetting);
        mainSceneButton.onClick.AddListener(OnClickMainScene);
        purchaseSceneButton.onClick.AddListener(OnClickPurchaseScene);
        innSceneButton.onClick.AddListener(OnClickInnScene);
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickRestartButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        quitButton.onClick.AddListener(OnClickQuit);
        inventoryReturnButton.onClick.AddListener(OnClickInventoryReturn);
    }

    private void Update()
    {
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

    }

    private void OnClickMainMenu()
    {
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }
    private void OnClickQuit()
    {

    }

    public void OpenSpecialMerchant()
    {
        
    }

    public void OpenBuyer()
    {
        salesInventoryWindow.SetActive(true);
    }

    public void OpenBusinessman()
    {

    }
    private void OnClickInventoryReturn()
    {
        salesInventoryWindow.GetComponent<UiSalesPanel>().salesItemInfo.blinder.SetActive(true);
        salesInventoryWindow.SetActive(false);
    }
}
