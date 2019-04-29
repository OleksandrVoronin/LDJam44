using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPurchase : MonoBehaviour
{
    private int[] _flyRanges = {
        16, 12, 14
    };

    private int[] _maxCapacity = {
        40, 50, 70
    };

    private int[] _combatAbility = {
        6, 12, 9
    };

    private float[] _fuelConsumption = {
        1f, 1.2f, 1.5f
    };

    private int[] _evasion = {
        18, 12, 6
    };

    public void PurchaseShip(int variant)
    {
        GameManager.Instance.CatLives -= 2;

        GameManager.Instance.ShipVariant = variant;
        GameManager.Instance.Fader.DOFade(1, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            GameManager.Instance.Fader.DOFade(0, 0.3f).SetDelay(0.5f);

            GameManager.Instance.Fuel = 35;
            GameManager.Instance.Provision = 30;
            GameManager.Instance.ShipMaxCapacity = _maxCapacity[variant];

            GameManager.Instance.CombatAbility = _combatAbility[variant];
            GameManager.Instance.FuelConsumption = _fuelConsumption[variant];
            GameManager.Instance.Evasion = _evasion[variant];

            // Upgrades cleared
            GameManager.Instance.LongRangeScan = false;
            GameManager.Instance.ExtraMineralsMined = 0;
            GameManager.Instance.ExtraGasMined = 0;
            GameManager.Instance.ExtraLowerFuelConsumption = 0;

            FindObjectOfType<PlayerShipMover>().FlyRange = _flyRanges[variant];
        });

    }
}
