using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiSettlementSceneManager : MonoBehaviour
{
    public GameObject settlementWindow;       
    
    public TextMeshProUGUI gameMode;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI mostBuyed;
    public TextMeshProUGUI purchaseAmt;
    public TextMeshProUGUI soldAmt;
    public TextMeshProUGUI diamondsText;

    public Image coinImg;
    public Image diaImg;
    public Image itemIcon;

    public AudioClip hitSoundClip;
    public AudioClip coinSoundClip;

    public Button exitButton;

    private int mostBuyedItemId;
    private string mostBuyedItemString;
    private string gameModeString;
    private string coinsString;
    private int coins;
    private int pAmt;
    private int sAmt;
    private int diamonds;

    private bool isStartCoinEffect;
    private float lerpVal;
    private float lerpTargetVal;


    private void Start()
    {
        isStartCoinEffect = false;
        lerpVal = 0f;
        lerpTargetVal = 4.5f;
        SetSettlementWindow();
        gameMode.text = "";
        coinsText.text = "";
        mostBuyed.text = "";
        purchaseAmt.text = "";
        soldAmt.text = "";
        diamondsText.text = "";

        exitButton.gameObject.SetActive(false);
        coinImg.gameObject.SetActive(false);
        diaImg.gameObject.SetActive(false);
        itemIcon.sprite = Resources.Load<Sprite>(GameInfos.blankImagePath);
    }

    public void SetSettlementWindow()
    {
        mostBuyedItemId = GameManager.Instance.MostBuyedItemId;
        gameModeString = DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(DataTableManager.GameModeTable.Get(
            GameManager.Instance.CurrentGameMode).StringId);
        coins = GameManager.Instance.coins;
        coinsString = DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
            .Get(999968);
        mostBuyedItemId = GameManager.Instance.MostBuyedItemId;
        mostBuyedItemString = string.Format(
            DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999969),
            DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
            .Get(DataTableManager.ItemTable.Get(mostBuyedItemId).StringId));

        pAmt = GameManager.Instance.entireItemDict[mostBuyedItemId].totalPurchasedAmount;
        sAmt = GameManager.Instance.entireItemDict[mostBuyedItemId].totalSoldAmount;
        diamonds = GameManager.Instance.bonusDiamonds;

        StartCoroutine(SetItemsWithDelay());
    }
    
    public IEnumerator SetItemsWithDelay()
    {
        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.PlaySfx(hitSoundClip);

        gameMode.text = gameModeString;

        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.PlaySfx(hitSoundClip);

        mostBuyed.text = mostBuyedItemString;

        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.PlaySfx(hitSoundClip);

        itemIcon.sprite = DataTableManager.ItemTable.Get(mostBuyedItemId).IconSprite;

        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.PlaySfx(hitSoundClip);

        purchaseAmt.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999970), pAmt.ToString());
        soldAmt.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999971), sAmt.ToString());

        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.PlaySfx(coinSoundClip);

        isStartCoinEffect = true;
        coinImg.gameObject.SetActive(true);
        coinsText.text = string.Format(coinsString, 0);

        yield return new WaitForSeconds(5);
        SoundManager.Instance.PlaySfx(hitSoundClip);

        string tempStr = "";
        if(diamonds == 0)
        {
            tempStr = string.Format(
                DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999973), DataTableManager.GameModeTable.Get(GameManager.Instance.CurrentGameMode)
                .DiamondGoal.ToString());
        }

        diaImg.gameObject.SetActive(true);
        diamondsText.text = "\n"+string.Format(
            DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999972), diamonds.ToString())+tempStr;

        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.PlaySfx(hitSoundClip);

        exitButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(isStartCoinEffect)
        {
            lerpVal += Time.deltaTime;

            float t = Mathf.Clamp01(lerpVal / lerpTargetVal);
            int lerpedCoinVal = (int)Mathf.Lerp(0,coins,t);                        
            
            coinsText.text = string.Format(coinsString, lerpedCoinVal.ToString());
            if (lerpVal >= lerpTargetVal)
            {
                lerpVal = lerpTargetVal;
                isStartCoinEffect = false;
                coinsText.text = string.Format(coinsString, coins.ToString());
            }
        }



    }


    public void OnClickExit()
    {
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }
}
