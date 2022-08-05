using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Play.Review;

public class ShopBuy : MonoBehaviour
{
    public Color tempMoney;
    public int price = 0;
    public int packIndex = 0;
    public GameManager gm;
    public GameObject buyButton;
    public GameObject equipButton;
    public GameObject review;
    // Create instance of ReviewManager
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;


    public Text priceText;
    // Start is called before the first frame update
 
    public void Start()
    {
        priceText.text = "$" + price;
        if (gm.packsEnabled[packIndex])
        {
            buyButton.SetActive(false);
            equipButton.SetActive(true);
        }
        else
        {
            buyButton.SetActive(true);
            equipButton.SetActive(false);
        }

        _reviewManager = new ReviewManager();
    }

    // Update is called once per frame
    public void BuyPack()
    {
        if(gm.Money >= price)
        {
            gm.Money -= price;
            gm.enablePack(packIndex);
            equipPack();
            gm.SaveGame();
            gm.mySM.BuySound();
            buyButton.SetActive(false);
            equipButton.SetActive(true);
            if(gm.isConnectedGooglePlayServices)
            {
                Social.ReportProgress(GPGSIds.achievement_fruity, 100.0f, null);
            }
            if(gm.pack0 && gm.pack1 && gm.pack2 && gm.pack3 && gm.pack4 && gm.pack5 && gm.pack6 && gm.pack7)
            {
                Social.ReportProgress(GPGSIds.achievement_completionist, 100.0f, null);
                gm.allPack = true;
                gm.SaveGame();
            }

            StartCoroutine(Review());

        }
        else
        {
            if(gm.moneyText.color != Color.red)
            {
                tempMoney = gm.moneyText.color;
            }           
            gm.moneyText.color = Color.red;
            gm.mySM.DenySound();
            Invoke("moneyColSwitch", 0.2f);
        }
    }

   public void equipPack()
    {
        gm.selectedPack = gm.Packs[packIndex];
        gm.SwitchPack(packIndex);
        gm.mySM.EquipSound();
    }

    public void OpenReview()
    {

    }

    public void CloseReview()
    {
        review.SetActive(false);
    }

    private void moneyColSwitch()
    {
        gm.moneyText.color = tempMoney;
    }

    IEnumerator Review()
    {
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
}
