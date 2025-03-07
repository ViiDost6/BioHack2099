using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject[] weapons;
    public int currentWeaponIndex = 0;
    public GameObject currentWeapon;
    public float weaponSwitchTime = 0.5f;
    public GameObject playerObj;
    public GameObject player;


    [Header("Keybinds")]
    //Keyboard keys for weapon switching
    public KeyCode lastWeaponKey = KeyCode.Q;
    public KeyCode nextWeaponKey = KeyCode.E;
    
    private void Start()
    {
        EquipWeapon(currentWeaponIndex);
        //Disable all weapons except the first one
        for (int i = 1; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
    }

    private void Update()
    {
        var gamepad = Gamepad.current;

        //Switch weapons with keyboard keys
        if (Input.GetKeyDown(lastWeaponKey))
        {
            SwitchWeapon(-1);
        }
        if (Input.GetKeyDown(nextWeaponKey))
        {
            SwitchWeapon(1);
        }
        //Switch weapons with gamepad buttons
        if (gamepad != null)
        {
            if (gamepad.leftShoulder.wasPressedThisFrame)
            {
                SwitchWeapon(-1);
            }
            if (gamepad.rightShoulder.wasPressedThisFrame)
            {
                SwitchWeapon(1);
            }
        }

        //reloads double jump
        if (currentWeaponIndex == 2 && player.GetComponent<PlayerMovement>().grounded)
        {
            player.GetComponent<PlayerMovement>().isDoubleJump = true;
        }
    }

    private void SwitchWeapon(int direction)
    {
        currentWeaponIndex += direction;
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        if (currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }

        //Current weapon GreatSword
        if (currentWeaponIndex == 0)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 6;
            player.GetComponent<PlayerMovement>().jumpForce = 5;
            player.GetComponent<PlayerMovement>().walkSpeed = 6;
            player.GetComponent<PlayerMovement>().sprintSpeed = 7;
        }
        //Current weapon Rapier
        if (currentWeaponIndex == 1)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 7;
            player.GetComponent<PlayerMovement>().jumpForce = 7;
            player.GetComponent<PlayerMovement>().walkSpeed = 7;
            player.GetComponent<PlayerMovement>().sprintSpeed = 9;
        }
        //Current weapon gun
        if (currentWeaponIndex == 2)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 9;
            player.GetComponent<PlayerMovement>().jumpForce = 7;
            player.GetComponent<PlayerMovement>().isDoubleJump = true;
            player.GetComponent<PlayerMovement>().walkSpeed = 9;
            player.GetComponent<PlayerMovement>().sprintSpeed = 10;
        }
        StartCoroutine(EquipWeapon(currentWeaponIndex));
    }

    private IEnumerator EquipWeapon(int index)
    {
        //Disable the current weapon
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }
        //Enable the new weapon
        currentWeapon = weapons[index];
        currentWeapon.SetActive(true);
        yield return new WaitForSeconds(weaponSwitchTime);
    }
}
