using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField]
    private List<Upgrade> _upgradesList = new List<Upgrade>();
    public List<Upgrade> UpgradeList
    {
        get => _upgradesList;
    }

    public bool[] IsUpgradeOwned;

    public bool CanPurchase(int index)
    {
        return GameManager.Instance.Minerals >= UpgradeList[index].MineralCost &&
                GameManager.Instance.Gas >= UpgradeList[index].GasCost &&
                GameManager.Instance.CatLives >= UpgradeList[index].LifeCost;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsUpgradeOwned = new bool[_upgradesList.Count];
    }

    public void PurchaseUpgrade(int index)
    {
        if (GameManager.Instance.Minerals >= UpgradeList[index].MineralCost &&
            GameManager.Instance.Gas >= UpgradeList[index].GasCost &&
            GameManager.Instance.CatLives >= UpgradeList[index].LifeCost)
        {
            GameManager.Instance.Minerals -= UpgradeList[index].MineralCost;
            GameManager.Instance.Gas -= UpgradeList[index].GasCost;
            GameManager.Instance.CatLives -= UpgradeList[index].LifeCost;

            IsUpgradeOwned[index] = true;

            switch (index)
            {
                case 0:
                    GameManager.Instance.LongRangeScan = true;
                    break;

                case 1:
                    GameManager.Instance.ShipMaxCapacity += 15;
                    break;

                case 2:
                    GameManager.Instance.PlayerShipMover.FlyRange += 4;
                    break;

                case 3:
                    GameManager.Instance.ExtraMineralsMined = 2;
                    break;

                case 4:
                    GameManager.Instance.ExtraGasMined = 2;
                    break;

                case 5:
                    GameManager.Instance.ExtraLowerFuelConsumption += 2;
                    break;

                case 6:
                    GameManager.Instance.Luck += 5;
                    break;

                case 7:
                    GameManager.Instance.PlayerShipMover.FlyRange += 1;
                    break;

                case 8:
                    GameManager.Instance.Evasion += 2;
                    break;

                case 9:
                    GameManager.Instance.CombatAbility += 2;
                    break;

                case 10:
                    GameManager.Instance.ShipMaxCapacity += 5;
                    break;

                case 11:
                    GameManager.Instance.ShipMaxCapacity += 10;
                    break;

            }
        }
    }

    [Serializable]
    public class Upgrade
    {
        public string Name;
        public string Description;

        public Sprite icon;

        public int LifeCost;
        public int MineralCost;
        public int GasCost;
    }
}
