using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectorSpriteRendererGO;
    private SpriteRenderer _selectorSpriteRenderer;

    [SerializeField]
    private LayerMask _selectorLayer;

    [SerializeField]
    private CanvasGroup _selectionUICanvasGroup;
    [SerializeField]
    private TextMeshProUGUI _destinationTargetText;
    [SerializeField]
    private TextMeshProUGUI _actionButtonText;
    [SerializeField]
    private Button _actionButton;

    [SerializeField]
    private CanvasGroup _costAlert;
    [SerializeField]
    private TextMeshProUGUI _fuelCostText;
    [SerializeField]
    private TextMeshProUGUI _provisioningCostText;

    private GameObject _currentSelection = null;

    private bool _selectionBusy;

    private PlayerShipMover _playerShipMover;

    [SerializeField]
    private float _animationSpeed = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        _playerShipMover = FindObjectOfType<PlayerShipMover>();
        _selectorSpriteRenderer = _selectorSpriteRendererGO.GetComponent<SpriteRenderer>();

        _currentSelection = _playerShipMover.CurrentStar;
        _selectorSpriteRendererGO.transform.DOMove(_currentSelection.transform.position, 0f);
        ShowSelector(true, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject selectedStar = null;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 worldSpaceClickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            RaycastHit2D raycastHit2D = Physics2D.Raycast(worldSpaceClickPosition, worldSpaceClickPosition + new Vector3(0, 0, 1), 999, _selectorLayer);

            if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.GetComponent<Star>() != null)
            {
                selectedStar = raycastHit2D.collider.gameObject;
            }

            if (selectedStar != _currentSelection)
            {
                if (!_selectionBusy)
                {
                    if (selectedStar == null)
                    {
                        //_selectionBusy = true;
                        //_currentSelection = null;
                        //HideSelector(true, 0f);
                    }
                    else
                    {
                        _selectionBusy = true;

                        _currentSelection = selectedStar;

                        HideSelector(false, 0f);

                        _selectorSpriteRendererGO.transform.DOMove(selectedStar.transform.position, _animationSpeed).SetDelay(_animationSpeed);
                        ShowSelector(true, _animationSpeed * 2);
                    }
                }
            }
        }

    }

    public void SelectedAction()
    {
        if (_currentSelection == null || _selectionBusy) return;

        if (_currentSelection == _playerShipMover.CurrentStar)
        {
            GameManager.Instance.SwitchToStarSystemView();
            StarSystemController.Instance.Randomize(_currentSelection.GetComponent<Star>());

            if (!_currentSelection.GetComponent<Star>().Entered)
            {
                RandomEventMaster.Instance.RollEvent();
                _currentSelection.GetComponent<Star>().Entered = true;
            }
        }
        else
        {
            _actionButtonText.text = "Warping..";
            _actionButton.interactable = false;

            _costAlert.DOFade(0, _animationSpeed);

            _playerShipMover.MoveTo(_currentSelection);

            _selectionBusy = true;

            GameManager.Instance.SpendFuel((int)GetSelectionFuelCost(), 1.5f);
            GameManager.Instance.SpendProvisioning((int)GetSelectionProvisioningCost(), 1.5f);

            ShowSelector(true, 2.6f);
        }
    }

    [Button]
    private void HideSelector(bool resetBusy, float delay)
    {
        _costAlert.DOFade(0, _animationSpeed).SetDelay(delay);
        _selectorSpriteRenderer.DOFade(0, _animationSpeed).SetDelay(delay);
        _selectionUICanvasGroup.DOFade(0, _animationSpeed).SetDelay(delay).OnComplete(() =>
        {
            if (resetBusy)
            {
                _selectionBusy = false;
            }
        });
    }

    [Button]
    private void ShowSelector(bool resetBusy, float delay)
    {
        _selectorSpriteRenderer.DOFade(1, _animationSpeed).SetDelay(delay).OnStart(() =>
        {
            if (_currentSelection == _playerShipMover.CurrentStar)
            {
                _actionButtonText.text = "View";
                _actionButton.interactable = true;

                _costAlert.DOFade(0, _animationSpeed);
            }
            else
            {
                if ((_currentSelection.transform.position - _playerShipMover.transform.position).magnitude > _playerShipMover.FlyRange)
                {
                    _actionButtonText.text = "Too Far..";
                    _actionButton.interactable = false;

                    _costAlert.DOFade(0, _animationSpeed);
                }
                else
                {
                    _actionButtonText.text = "Warp";
                    _actionButton.interactable = true;

                    _costAlert.DOFade(1, _animationSpeed);
                    _fuelCostText.text = GetSelectionFuelCost() + "";
                    _provisioningCostText.text = GetSelectionProvisioningCost() + "";
                }
            }

            _destinationTargetText.text = _currentSelection.GetComponent<Star>().Name;
        });

        _selectionUICanvasGroup.DOFade(1, _animationSpeed).SetDelay(delay).OnComplete(() =>
        {
            if (resetBusy)
            {
                _selectionBusy = false;
            }
        });
    }

    private float GetSelectionFuelCost()
    {
        float distance = (_currentSelection.transform.position - _playerShipMover.transform.position).magnitude;

        return Mathf.Round(distance * 1f * GameManager.Instance.FuelConsumption - GameManager.Instance.ExtraLowerFuelConsumption);
    }

    private float GetSelectionProvisioningCost()
    {
        float distance = (_currentSelection.transform.position - _playerShipMover.transform.position).magnitude;

        return Mathf.Round(distance * 0.5f);
    }
}
