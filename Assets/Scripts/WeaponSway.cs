using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponSway : MonoBehaviour
{
    private FirstPersonControlSharp control;

    private Player_move_c pmc;

    private Vector3 cameraOffset
    {
        get
        {
            return new Vector3(0f, 0f, 0f);
        }
    }

    public Vector3 currentPosition { get; set; }

    public Vector3 currentRotation { get; set; }

    public Vector3 sway { get; set; }

    public Camera gunCamera;

    public static WeaponSway instance;

    private bool wasUngrounded;

    public float bobSpeed, bobMultiplier, lerpSpeed;

    public float shake { get; set; }

    private float time;

    private void Start()
    {
        pmc = GetComponentInChildren<Player_move_c>();

        if (pmc != WeaponManager.sharedManager.myPlayerMoveC)
        {
            Destroy(this);
            return;
        }

        instance = this;

        control = GetComponent<FirstPersonControlSharp>();
    }

    private float RandomShake(bool allowedReverse = true)
    {
        return (Random.Range(0, 2) == 1 || !allowedReverse) ? (allowedReverse ? Random.Range(shake - (shake * 0.25f), shake) : -Mathf.Clamp(Random.Range(shake - (shake * 0.25f), shake), 0, shake)) : Random.Range(-shake, -shake + (shake * 0.25f));
    }

    private bool oldjumpPressed;

    private void Update()
    {
        if (!control.character.enabled)
        {
            return;
        }

        currentRotation = new Vector3(Mathf.Clamp(currentRotation.x, -15, 15), Mathf.Clamp(currentRotation.y, -10, 10), currentRotation.z);
        sway = new Vector3(Mathf.Clamp(sway.x, -15, 15), Mathf.Clamp(sway.y, -10, 10), sway.z);

        shake = Mathf.Clamp(shake, 0, 7.5f);
        shake = Mathf.Lerp(shake, 0f, Time.deltaTime * 10f);
        Vector3 randomShake = new Vector3(RandomShake(), RandomShake(false) * 2.53f, RandomShake());

        gunCamera.transform.localPosition = Vector3.Slerp(gunCamera.transform.localPosition + randomShake, cameraOffset + currentPosition - new Vector3(Easing.EaseInOut.Sine(time * 0.5f) * bobMultiplier * 0.5f, Easing.EaseInOut.Sine(time) * bobMultiplier) + randomShake, Time.deltaTime * bobSpeed * lerpSpeed);
        gunCamera.transform.localRotation = Quaternion.Slerp(gunCamera.transform.localRotation, Quaternion.Euler(-(currentRotation.x + sway.x), -(currentRotation.y + sway.y), -(Easing.EaseInOut.Sine(time * 0.5f) * bobMultiplier * 0.5f * 100f + (currentRotation.z + sway.z) + (-JoystickController.leftJoystick.value.x * 5f))), Time.deltaTime * bobSpeed * lerpSpeed);

        if (control.character.isGrounded)
        {
            currentPosition = Vector3.Slerp(currentPosition, Vector3.zero, Time.deltaTime * bobSpeed * lerpSpeed);
            currentRotation = Vector3.Slerp(currentRotation, Vector3.zero, Time.deltaTime * bobSpeed * lerpSpeed);

            if (wasUngrounded)
            {
                sway = new Vector3(7.5f, 0f, sway.z + Random.Range(-5f, 5f));
                currentRotation = Vector3.zero;
                wasUngrounded = false;
            }
        }

        sway = Vector3.Slerp(sway, Vector3.zero, Time.deltaTime * bobSpeed * lerpSpeed);

        if (control.character.isGrounded && JoystickController.leftJoystick.value != Vector2.zero)
        {
            time += Time.deltaTime * bobSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * (JoystickController.leftJoystick.value.y == 0 ? JoystickController.leftJoystick.value.x : JoystickController.leftJoystick.value.y) * 1.25f;
        }
        else if (!control.character.isGrounded && InGameGUI.sharedInGameGUI && InGameGUI.sharedInGameGUI.playerMoveC)
        {
            if (Defs.isJetpackEnabled)
            {
                if (JoystickController.rightJoystick.jumpPressed)
                {
                    currentRotation += Vector3.right * (Time.deltaTime * 7.55f);
                }
                else
                {
                    currentRotation -= Vector3.right * (Time.deltaTime * 2.5f);
                }

                if (oldjumpPressed != JoystickController.rightJoystick.jumpPressed)
                {
                    if (oldjumpPressed && !JoystickController.rightJoystick.jumpPressed)
                        currentRotation = Vector3.zero;
                    oldjumpPressed = JoystickController.rightJoystick.jumpPressed;
                }
            }
            else
            {
                oldjumpPressed = JoystickController.rightJoystick.jumpPressed;
                currentRotation -= Vector3.right * (Time.deltaTime * 2.5f);
            }

            if (JoystickController.leftJoystick.value.y > 0)
            {
                sway += new Vector3(-35f * Time.deltaTime, 0, 0);
            }
            else if (JoystickController.leftJoystick.value.y < 0)
            {
                sway += new Vector3(35f * Time.deltaTime, 0, 0);
            }

            wasUngrounded = true;
        }
        else
        {
            time += Time.deltaTime * 0.15f;
        }

        sway += new Vector3(control.delta.y * 0.035f, control.delta.x * 0.015f, -control.delta.x * 0.015f);
    }
}
