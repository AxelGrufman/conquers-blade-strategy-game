using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RadialMenuController : MonoBehaviour
{
    public GameObject theMenu;
    public GameObject OtherMenu;
    [SerializeField] private PlayerInput playerInput;

    public Vector2 MoveInput;

    public TMP_Text[] Options;
    public Color normalColor, HigelightedColour;
    public int selectedOption;

    public OrderGiver orderGiver;
    public bool IsOrderMenu;

    private int temp = 0;

    public void OnTab(InputAction.CallbackContext ctx)
    {
        theMenu.SetActive(true);
        playerInput.SwitchCurrentActionMap("UI");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (theMenu.activeInHierarchy == true)
        {
            playerInput.SwitchCurrentActionMap("Player");
            orderGiver.GetOrder(selectedOption, IsOrderMenu);
            theMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void OnScroll(InputAction.CallbackContext ctx)
    {
        if (theMenu.activeInHierarchy != true) 
            return;
        temp++;

        if (temp % 2 == 0)
         {
            theMenu.SetActive(false);
            OtherMenu.SetActive(true);
         }
    }

    void Update()
    {
        if (theMenu.activeInHierarchy)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            MoveInput.x = mousePos.x - (Screen.width / 2f);
            MoveInput.y = mousePos.y - (Screen.height / 2f);
            MoveInput.Normalize();

            if (MoveInput != Vector2.zero)
            {
                float angle = Mathf.Atan2(MoveInput.y, -MoveInput.x) / Mathf.PI;
                angle *= 180f;
                angle += 90f;
                if (angle < 0)
                {
                    angle += 360f;
                }

                for (int i = 0; i < Options.Length; i++)
                {
                    float step = 360f / Options.Length;
                    if (angle >= step * i && angle < step * (i + 1))
                    {
                        Options[i].color = HigelightedColour;
                        selectedOption = i;
                    }
                    else
                    {
                        Options[i].color = normalColor;
                    }
                }
            }

        }
    }
}
