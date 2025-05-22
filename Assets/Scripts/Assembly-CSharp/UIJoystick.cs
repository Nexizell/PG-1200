using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIJoystick : MonoBehaviour
{
    public Transform target;

    public float radius;

    public Vector2 value;

    private HungerGameController _hungerGameController;

    private bool _isHunger;

    private float? _actualRadius;

    private UIWidget _joystickWidget;

    private int activeTouchId = -1;

    private Vector3 initialPosition;

    public float ActualRadiusButNotFucked
    {
        get
        {
            float divideCoef = 43.0f * 2.0f;
            return ActualRadius * divideCoef;
        }
        set
        {
            _actualRadius = value;
        }
    }

    public float ActualRadius
    {
        get
        {
            float divideCoef = 43.0f * 2.0f;
            return (!_actualRadius.HasValue) ? radius / divideCoef : _actualRadius.Value / divideCoef;
        }
        set
        {
            _actualRadius = value;
        }
    }

    public float ActualRadiusSq
    {
        get
        {
            float actualRadius = ActualRadius;
            return actualRadius * actualRadius;
        }
    }

    private void Awake()
    {
        _joystickWidget = GetComponent<UIWidget>();
        _isHunger = GameConnect.isHunger;
    }

    private void Start()
    {
        ChangeSide();
        PauseNGUIController.PlayerHandUpdated += ChangeSide;
        if (_isHunger)
        {
            _hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
        }
        UpdateVisibility();
        Reset();
    }

    private void OnDestroy()
    {
        PauseNGUIController.PlayerHandUpdated -= ChangeSide;
    }

    private void OnDisable()
    {
        ResetJoystick();
    }

    private void Update()
    {
        ProcessInput();
        UpdateVisibility();
    }

    private void ProcessInput()
    {
        if (!GlobalControls.DoMobile) return;
        if (MobileRelay.touchCount > 0)
        {
            foreach (Touch touch in MobileRelay.touches)
            {
                if (touch.phase == TouchPhase.Began && IsTouchWithinBounds(touch))
                {
                    activeTouchId = touch.fingerId;
                }
                if (touch.fingerId == activeTouchId)
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        Vector3 touchPosition = MyUICamera.cachedCamera.ScreenToWorldPoint(touch.position);
                        touchPosition.z = target.position.z;

                        target.position = touchPosition;
                        float magnitude = target.localPosition.magnitude;
                        if (magnitude > ActualRadiusButNotFucked)
                        {
                            target.localPosition = Vector3.ClampMagnitude(target.localPosition, ActualRadiusButNotFucked);
                        }

                        Vector2 direction = new Vector2(target.localPosition.x / ActualRadiusButNotFucked, target.localPosition.y / ActualRadiusButNotFucked);
                        direction.x = Mathf.Clamp(direction.x, -1f, 1f);
                        direction.y = Mathf.Clamp(direction.y, -1f, 1f);

                        GlobalControls.MovementVector = new Vector2(direction.x, direction.y);
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        ResetJoystick();
                    }
                }
            }
        }
    }

    private UICamera _cachedUICamera;

    public UICamera MyUICamera;

    private bool IsTouchWithinBounds(Touch touch)
    {
        Vector3 touchPosition = MyUICamera.cachedCamera.ScreenToWorldPoint(touch.position);
        touchPosition.z = 0;
        return Vector3.Distance(touchPosition, initialPosition) <= ActualRadius;
    }

    public void SetJoystickActive(bool joyActive)
    {
        base.enabled = joyActive;
        if (!joyActive)
        {
            Reset();
        }
    }

    private void UpdateVisibility()
    {
        bool flag = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != 0 || TrainingController.stepTraining >= TrainingState.TapToMove) && (!_isHunger || _hungerGameController.isGo) && !GameConnect.isSpeedrun;
        _joystickWidget.alpha = ((!flag) ? 0f : 1f);
    }

    private void ChangeSide()
    {
        base.transform.parent.GetComponent<UIAnchor>().side = ((!GlobalGameController.LeftHanded) ? UIAnchor.Side.BottomRight : UIAnchor.Side.BottomLeft);
        Reset();
    }

    public void Reset()
    {
        value = Vector2.zero;
        target.localPosition = Vector3.zero;
        initialPosition = target.position;
        activeTouchId = -1;
    }

    private void ResetJoystick()
    {
        target.position = initialPosition;
        target.localPosition = Vector3.zero;
        GlobalControls.MovementVector = Vector2.zero;
        activeTouchId = -1;
    }
}
