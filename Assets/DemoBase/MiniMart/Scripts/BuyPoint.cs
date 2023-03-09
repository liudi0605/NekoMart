using UnityEngine;
using RDG;
using TMPro;
using DG.Tweening;

public class BuyPoint : MonoBehaviour
{
    public int srNo, purchaseAmount;
    private GameManager _GameManager;
    private float countAnimSpeed = 0.1f;
    private float animDuration = 0.5f;
    private TextMeshPro moneyAmountText;
    public GameObject objectToUnlock;
    private PlayerCtrl _PlayerController;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(srNo + "Unlocked"))
        {
            if (objectToUnlock.GetComponent<FoodPlaceManager>())
                objectToUnlock.GetComponent<BoxCollider>().enabled = true;

            UnlockObject();
        }

        _PlayerController = FindObjectOfType<PlayerCtrl>();
        _GameManager = FindObjectOfType<GameManager>();

        purchaseAmount = PlayerPrefs.GetInt(srNo+"PurchaseAmount", purchaseAmount);

        moneyAmountText = GetComponentInChildren<TextMeshPro>();

        ShowPurchaseAmount();
    }

    private void ShowPurchaseAmount()
    {
        moneyAmountText.text = purchaseAmount.ToString();
    }

    public void StartSpend()
    {
        if (purchaseAmount > 100)
            countAnimSpeed = 0.05f;
         else if (purchaseAmount > 500)
            countAnimSpeed = 0.01f;

        InvokeRepeating("Spend", countAnimSpeed, countAnimSpeed);
    }

    private void Spend()
    {
        if (_GameManager.collectedMoney > 0)
        {       
            AudioManager.Instance.Play("BuyPoint");

            Vibration.Vibrate(30);
            purchaseAmount--;
            PlayerPrefs.SetInt(srNo + "PurchaseAmount", purchaseAmount);

            _GameManager.LessMoney();
            ShowPurchaseAmount();

            if (purchaseAmount == 0)
            {
                PlayerPrefs.SetString(srNo + "Unlocked", "True");

                _PlayerController.SidePos();
                objectToUnlock.transform.DOPunchScale(new Vector3(0.1f, 1, 0.1f), animDuration, 7).OnComplete(() => Destroy(this.gameObject)); ;
                UnlockObject();

                CustomerSpawner[] custSawners = FindObjectsOfType<CustomerSpawner>();
                custSawners[Random.Range(0, custSawners.Length)].SpawnCustomer();

                AudioManager.Instance.Play("Unlock");
                ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
                particle.transform.parent = null;
                particle.Play();
            }
        }
        else
        {
            CancelInvoke("Spend");
        }
    }

    private void UnlockObject()
    {
        objectToUnlock.SetActive(true);
        DOTween.Kill(this.gameObject);      
        Destroy(this.gameObject);
    }

    public void StopSpend()
    {
        CancelInvoke("Spend");
    }
}
