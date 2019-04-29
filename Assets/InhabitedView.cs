using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InhabitedView : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _welcomeMessage;
    [SerializeField]
    private CanvasGroup _trading;
    [SerializeField]
    private CanvasGroup _upgrades;

    private Star.PlanetInformation _planetInformation;
    private int minorUpgrade;
    private int majorUpgrade;

    [SerializeField]
    private TextMeshProUGUI _minorLifeCost;
    [SerializeField]
    private TextMeshProUGUI _minorMineralCost;
    [SerializeField]
    private TextMeshProUGUI _minorGasCost;

    [SerializeField]
    private TextMeshProUGUI _majorLifeCost;
    [SerializeField]
    private TextMeshProUGUI _majorMineralCost;
    [SerializeField]
    private TextMeshProUGUI _majorGasCost;

    [SerializeField]
    private Image _minorIcon;
    [SerializeField]
    private Image _majorIcon;

    [SerializeField]
    private TextMeshProUGUI _minorName;
    [SerializeField]
    private TextMeshProUGUI _majorName;

    [SerializeField]
    private TextMeshProUGUI _minorDescription;
    [SerializeField]
    private TextMeshProUGUI _majorDescription;

    [SerializeField]
    private TextMeshProUGUI _minorButtonText;
    [SerializeField]
    private TextMeshProUGUI _majorButtonText;

    [SerializeField]
    private Button _minorButton;
    [SerializeField]
    private Button _majorButton;

    public void OnEnable()
    {
        _welcomeMessage.DOFade(1, 0.3f).OnStart(() => _welcomeMessage.gameObject.SetActive(true));
        _trading.DOFade(0, 0.3f).OnComplete(() => _trading.gameObject.SetActive(false));
        _upgrades.DOFade(0, 0.3f).OnComplete(() => _upgrades.gameObject.SetActive(false));
    }

    public void Trade()
    {
        _welcomeMessage.DOFade(0, 0.3f).OnComplete(() => _welcomeMessage.gameObject.SetActive(false));
        _trading.DOFade(1, 0.3f).OnStart(() => _trading.gameObject.SetActive(true));
        _upgrades.DOFade(0, 0.3f).OnComplete(() => _upgrades.gameObject.SetActive(false));
    }

    public void Upgrade()
    {
        _welcomeMessage.DOFade(0, 0.3f).OnComplete(() => _welcomeMessage.gameObject.SetActive(false));
        _trading.DOFade(0, 0.3f).OnComplete(() => _trading.gameObject.SetActive(false));
        _upgrades.DOFade(1, 0.3f).OnStart(() => _upgrades.gameObject.SetActive(true));
    }

    public void Back()
    {
        GameManager.Instance.Fader.DOFade(1, 0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            GameManager.Instance.Fader.DOFade(0, 0.2f);
        });
    }

    public void Init(Star.PlanetInformation planetInformation)
    {
        // Don't ask
        minorUpgrade = (planetInformation.Variant * 2) % 5 + 7;
        majorUpgrade = (planetInformation.Variant * 3) % 7;

        UpdateUpgradeView();
    }

    public void PurchaseMajor()
    {
        UpgradeManager.Instance.PurchaseUpgrade(majorUpgrade);
        UpdateUpgradeView();
    }

    public void PurchaseMinor()
    {
        UpgradeManager.Instance.PurchaseUpgrade(minorUpgrade);
        UpdateUpgradeView();
    }

    public void UpdateUpgradeView()
    {
        _minorName.text = UpgradeManager.Instance.UpgradeList[minorUpgrade].Name;
        _minorDescription.text = UpgradeManager.Instance.UpgradeList[minorUpgrade].Description;

        _minorLifeCost.text = UpgradeManager.Instance.UpgradeList[minorUpgrade].LifeCost + "";
        _minorMineralCost.text = UpgradeManager.Instance.UpgradeList[minorUpgrade].MineralCost + "";
        _minorGasCost.text = UpgradeManager.Instance.UpgradeList[minorUpgrade].GasCost + "";

        _minorButton.interactable = !UpgradeManager.Instance.IsUpgradeOwned[minorUpgrade] && UpgradeManager.Instance.CanPurchase(minorUpgrade);
        if (UpgradeManager.Instance.IsUpgradeOwned[minorUpgrade])
        {
            _minorButtonText.text = "Owned";
        }
        else if (!UpgradeManager.Instance.CanPurchase(minorUpgrade))
        {
            _minorButtonText.text = "Insufficient Funds";
        } else {
            _minorButtonText.text = "Owned";
        }

        _minorIcon.sprite = UpgradeManager.Instance.UpgradeList[minorUpgrade].icon;


        _majorName.text = UpgradeManager.Instance.UpgradeList[majorUpgrade].Name;
        _majorDescription.text = UpgradeManager.Instance.UpgradeList[majorUpgrade].Description;

        _majorLifeCost.text = UpgradeManager.Instance.UpgradeList[majorUpgrade].LifeCost + "";
        _majorMineralCost.text = UpgradeManager.Instance.UpgradeList[majorUpgrade].MineralCost + "";
        _majorGasCost.text = UpgradeManager.Instance.UpgradeList[majorUpgrade].GasCost + "";

        _majorButton.interactable = !UpgradeManager.Instance.IsUpgradeOwned[majorUpgrade] && UpgradeManager.Instance.CanPurchase(majorUpgrade);
        if (UpgradeManager.Instance.CanPurchase(majorUpgrade))
        {
            _majorButtonText.text = UpgradeManager.Instance.IsUpgradeOwned[majorUpgrade] ? "Owned" : "Purchase";
        }
        else
        {
            _majorButtonText.text = "Insufficient Funds";
        }

        _majorIcon.sprite = UpgradeManager.Instance.UpgradeList[majorUpgrade].icon;
    }
}
