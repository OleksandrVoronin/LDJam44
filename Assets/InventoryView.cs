using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _fuelAmountText;
    [SerializeField]
    private TextMeshProUGUI _fuelWeightText;
    [SerializeField]
    private TextMeshProUGUI _fuelTotalWeightText;

    [SerializeField]
    private TextMeshProUGUI _provisioningAmountText;
    [SerializeField]
    private TextMeshProUGUI _provisioningWeightText;
    [SerializeField]
    private TextMeshProUGUI _provisioningTotalWeightText;

    [SerializeField]
    private TextMeshProUGUI _mineralsAmountText;
    [SerializeField]
    private TextMeshProUGUI _mineralsWeightText;
    [SerializeField]
    private TextMeshProUGUI _mineralsTotalWeightText;

    [SerializeField]
    private TextMeshProUGUI _gasAmountText;
    [SerializeField]
    private TextMeshProUGUI _gasWeightText;
    [SerializeField]
    private TextMeshProUGUI _gasTotalWeightText;

    [SerializeField]
    private TextMeshProUGUI _capacityText;
    [SerializeField]
    private GameObject _tooHeavy;

    void OnEnable()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        _fuelAmountText.text = "x" + GameManager.Instance.Fuel;
        _provisioningAmountText.text = "x" + GameManager.Instance.Provisioning;
        _mineralsAmountText.text = "x" + GameManager.Instance.Minerals;
        _gasAmountText.text = "x" + GameManager.Instance.Gas;

        _fuelWeightText.text = "" + GameManager.Instance.FuelWeight;
        _provisioningWeightText.text = "" + GameManager.Instance.ProvisioningWeight;
        _mineralsWeightText.text = "" + GameManager.Instance.MineralsWeight;
        _gasWeightText.text = "" + GameManager.Instance.GasWeight;

        _fuelTotalWeightText.text = "" + Mathf.Round(GameManager.Instance.FuelWeight * GameManager.Instance.Fuel);
        _provisioningTotalWeightText.text = "" + Mathf.Round(GameManager.Instance.ProvisioningWeight * GameManager.Instance.Provisioning);
        _mineralsTotalWeightText.text = "" + Mathf.Round(GameManager.Instance.MineralsWeight * GameManager.Instance.Minerals);
        _gasTotalWeightText.text = "" + Mathf.Round(GameManager.Instance.GasWeight * GameManager.Instance.Gas);

        if (_capacityText != null)
        {
            _capacityText.color = GameManager.Instance.ShipCapacity > GameManager.Instance.ShipMaxCapacity ? Color.red : Color.white;
            _capacityText.text = "Total capacity: " + GameManager.Instance.ShipCapacity + "/" + GameManager.Instance.ShipMaxCapacity;
        }

        if (_tooHeavy != null)
        {
            _tooHeavy.SetActive(GameManager.Instance.ShipCapacity > GameManager.Instance.ShipMaxCapacity);
        }
    }

    public void DumpFuel()
    {
        if (GameManager.Instance.Fuel > 0)
        {
            GameManager.Instance.Fuel--;
            UpdateView();
        }
    }

    public void DumpProvisioning()
    {
        if (GameManager.Instance.Provisioning > 0)
        {
            GameManager.Instance.Provisioning--;
            UpdateView();
        }
    }

    public void DumpMinerals()
    {
        if (GameManager.Instance.Minerals > 0)
        {
            GameManager.Instance.Minerals--;
            UpdateView();
        }
    }

    public void DumpGas()
    {
        if (GameManager.Instance.Gas > 0)
        {
            GameManager.Instance.Gas--;
            UpdateView();
        }
    }
}
