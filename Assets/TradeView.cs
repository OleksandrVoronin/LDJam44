using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeView : MonoBehaviour
{


    private InventoryView _inventoryView;

    // Start is called before the first frame update
    void Start()
    {
        _inventoryView = transform.parent.GetComponentInChildren<InventoryView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TradeMineralsFuel() {
        if (GameManager.Instance.Minerals > 0)
        {
            GameManager.Instance.Minerals--;
            GameManager.Instance.Fuel += 2;
            _inventoryView.UpdateView();
        }
    }

    public void TradeMineralsProvision()
    {
        if (GameManager.Instance.Minerals > 0)
        {
            GameManager.Instance.Minerals--;
            GameManager.Instance.Provision += 3;
            _inventoryView.UpdateView();
        }
    }

    public void TradeGasFuel()
    {
        if (GameManager.Instance.Gas > 0)
        {
            GameManager.Instance.Gas--;
            GameManager.Instance.Fuel++;
            _inventoryView.UpdateView();
        }
    }

    public void TradeGasProvision()
    {
        if (GameManager.Instance.Gas > 0)
        {
            GameManager.Instance.Gas--;
            GameManager.Instance.Provision += 2;
            _inventoryView.UpdateView();
        }
    }

}
