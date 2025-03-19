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
    public Animator animator;


    [Header("Keybinds")]
    //Keyboard keys for weapon switching
    public KeyCode lastWeaponKey = KeyCode.Q;
    public KeyCode nextWeaponKey = KeyCode.E;
    //indicates if the player can switch weapons now
    public bool canSwitch = true;
    
    private void Start()
    {
        EquipWeapon(currentWeaponIndex);
        weapons[1].SetActive(false);
        weapons[2].SetActive(false);
    }

    private void Update()
    {
        var gamepad = Gamepad.current;

        //the player cant switch weapons while aiming or jumping
        if (gameObject.GetComponent<WeaponHabilities>().isAiming || !player.GetComponent<PlayerMovement>().grounded)
        {
            canSwitch = false;
        }
        else
        {
            canSwitch = true;
        }

        //allows weapon switching
        if (canSwitch)
        {
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
            animator.SetBool("GreatSword", true);
            animator.SetBool("Rapier", false);
            animator.SetBool("Gun", false);
        }
        //Current weapon Rapier
        if (currentWeaponIndex == 1)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 7;
            player.GetComponent<PlayerMovement>().jumpForce = 7;
            player.GetComponent<PlayerMovement>().walkSpeed = 7;
            player.GetComponent<PlayerMovement>().sprintSpeed = 9;
            animator.SetBool("GreatSword", false);
            animator.SetBool("Rapier", true);
            animator.SetBool("Gun", false);
        }
        //Current weapon gun
        if (currentWeaponIndex == 2)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 9;
            player.GetComponent<PlayerMovement>().jumpForce = 7;
            player.GetComponent<PlayerMovement>().isDoubleJump = true;
            player.GetComponent<PlayerMovement>().walkSpeed = 9;
            player.GetComponent<PlayerMovement>().sprintSpeed = 10;
            animator.SetBool("GreatSword", false);
            animator.SetBool("Rapier", false);
            animator.SetBool("Gun", true);
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
