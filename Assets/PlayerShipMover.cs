using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PlayerShipMover : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer _playerShip;
    [SerializeField]
    private SpriteRenderer _playerMark;
    [SerializeField]
    private SpriteRenderer _rangeIndicator;
    [SerializeField]
    private TextMeshPro _playerMarkText;

    private GameObject _currentStar;
    public GameObject CurrentStar
    {
        get => _currentStar;
        set => _currentStar = value;
    }

    [SerializeField]
    private float _flyRange = 10f;
    public float FlyRange
    {
        get => _flyRange;
        set {
            _flyRange = value;
            _rangeIndicator.transform.localScale = Vector3.one * _flyRange / 10f;
        }
    }

    private void Awake()
    {
        // Trigger set() hack
        FlyRange = _flyRange;
    }

    [Button]
    public void MoveTo(GameObject destination)
    {
        _currentStar = destination;

        HideMark();

        float destinationRotation = Vector3.SignedAngle(Vector2.up, (destination.transform.position - transform.position), Vector3.forward);

        transform.DORotate(new Vector3(0, 0, destinationRotation), 0.1f, RotateMode.Fast).SetDelay(0.3f);
        transform.DOMove(destination.transform.position, 2f).SetDelay(0.3f).OnComplete(() =>
        {
            transform.rotation = new Quaternion();
            ShowMark();

            if (transform.position == FindObjectOfType<StarGenerator>().GeneratedStars[FindObjectOfType<StarGenerator>().GeneratedStars.Count - 1].transform.position) {
                GameManager.Instance.GameWon();
            }
        });

    }


    [Button]
    private void ShowMark()
    {
        _playerShip.DOFade(0, 0.3f);
        _playerMark.DOFade(1, 0.3f);
        _playerMarkText.DOFade(1, 0.3f);
        _rangeIndicator.DOFade(1, 0.3f);
    }

    [Button]
    private void HideMark()
    {
        _playerShip.DOFade(1, 0.3f);
        _playerMark.DOFade(0, 0.3f);
        _playerMarkText.DOFade(0, 0.3f);
        _rangeIndicator.DOFade(0, 0.3f);
    }
}
