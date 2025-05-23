using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Rilisoft;

public class GlobalControls : MonoBehaviour
{
    public static bool MouseLocked
    {
        get
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }
        set
        {
            Cursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None);
        }
    }

    public static bool IsOnPC
    {
        get
        {
            return !Application.isMobilePlatform && !Application.isConsolePlatform;
        }
    }

    public static bool DoMobile
    {
        get => !IsOnPC;
    }

    private static Vector2 _movementVector;

    public static Vector2 MovementVector
    {
        get
        {
            if (!DoMobile)
            {
                int y = 0;
                int x = 0;
                if (Input.GetKey(KeyCode.W))
                {
                    y = 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    y = -1;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    x = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    x = 1;
                }
                _movementVector = new Vector2(x, y);
                return _movementVector;
            }
            return _movementVector;
        }
        set
        {
            _movementVector = value;
        }
    }

    private static Vector2 _lookDelta;
    public static Vector2 LookDelta
    {
        get
        {
            if (!DoMobile)
            {
                if (!MouseLocked) return Vector2.zero;
                float mult = 8.36f;
                _lookDelta = new Vector2(Input.GetAxisRaw("Mouse X") * mult, Input.GetAxisRaw("Mouse Y") * mult);
            }
            return _lookDelta;
        }
        set
        {
            _lookDelta = value;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetKeyDown(KeyCode.F1)) MouseLocked = !MouseLocked;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            BankController.AddCoins(50);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            BankController.AddGems(50);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ExperienceController.sharedController.AddExperience(10);
        }
    }
}