using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private RectTransform _rect;

    [SerializeField]
    private Sprite _onScreenSprite;
    [SerializeField]
    private Sprite _offScreenSprite;

    private Image _thisImage;

    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _thisImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            StarGenerator starGenerator = FindObjectOfType<StarGenerator>();

            if (starGenerator != null && starGenerator.GeneratedStars.Count > 0)
            {
                _target = starGenerator.GeneratedStars[starGenerator.GeneratedStars.Count - 1];
            }
        }
        else
        {
            Vector3 toPosition = _target.transform.position;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;

            

            float borderSize = Screen.height / 10f;

            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_target.transform.position);
            bool isOffscreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffscreen)
            {
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);
                _rect.position = cappedTargetScreenPosition.NewZ(0);
                if(_thisImage.sprite != _offScreenSprite)
                    _thisImage.sprite = _offScreenSprite;   

                float angle = Vector2.SignedAngle(Vector2.up, dir);
                _rect.localEulerAngles = new Vector3(0, 0, angle);
            }
            else
            {
                _rect.position = targetPositionScreenPoint.NewZ(0);
                if (_thisImage.sprite != _onScreenSprite)
                    _thisImage.sprite = _onScreenSprite;

                float angle = 0;
                _rect.localEulerAngles = new Vector3(0, 0, angle);
            }
        }
    }
}
