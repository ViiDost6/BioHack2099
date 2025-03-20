using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHabilities : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject[] weapons;
    public GameObject weaponManager;
    public int currentWeaponIndex;
    public GameObject currentWeapon;
    public GameObject playerObj;
    public GameObject player;
    public Animator animator;
    private AnimatorStateInfo stateInfo;
    public Canvas aimCanvas;

    [Header("Camera")]
    public CinemachineCamera camera3rdperson;
    public CinemachineCamera cameraAiming;
    public bool isAiming;

    private void Start()
    {
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    private void Update()
    {
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
        if (currentWeaponIndex == 0)
        {
            GreatSwordHabilities();

            //Block manager
            if (Input.GetMouseButtonUp(1))
            {
                animator.SetBool("Block", false);
            }

            //Combat manager
            if (Input.GetMouseButtonDown(0))
            {
                if (stateInfo.IsName("GS_Slash") && stateInfo.normalizedTime < 0.9f)
                {
                    animator.SetInteger("Hit", 2);
                }
                else if (stateInfo.IsName("GS_Slash2") && stateInfo.normalizedTime < 0.9f)
                {
                    animator.SetInteger("Hit", 3);
                }
            }
            if (stateInfo.normalizedTime >= 0.9f && (stateInfo.IsName("GS_Slash") || stateInfo.IsName("GS_Slash2") || stateInfo.IsName("GS_Slash3")))
            {
                animator.SetInteger("Hit", 0);
            }
        }
        else if (currentWeaponIndex == 1)
        {
            RapierHabilities();
        }
        else if (currentWeaponIndex == 2)
        {
            GunHabilities();
            if (cameraAiming.Priority == 1)
            {
                playerObj.transform.rotation = Quaternion.Euler(0, cameraAiming.transform.rotation.eulerAngles.y, 0);
            }
        }

        //if the camera is aiming, enables the aimCanvas
        if (cameraAiming.Priority == 1)
        {
            aimCanvas.enabled = true;
        }
        else
        {
            aimCanvas.enabled = false;
        }
    }

    //This method is called when the player switches weapons
    public void HabilitesManager(int weaponIndex)
    {
        if (currentWeaponIndex != weaponIndex)
        {
            currentWeaponIndex = weaponIndex;

            if (currentWeaponIndex == 0)
            {
                //Calls GreatSwordHabilities method
            }
            else if (currentWeaponIndex == 1)
            {
                //Calls RapierHabilities method
            }
            else if (currentWeaponIndex == 2)
            {
                //Calls Gun method
            }
        }
    }

    //This method is called when the player switches to the GreatSword
    public void GreatSwordHabilities()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //GreatSword habilities
        if (player.GetComponent<PlayerMovement>().grounded && stateInfo.IsName("GS_Walk") && animator.GetFloat("Speed") <= 0.1f && animator.GetFloat("Direction") <= 0.1f)
        {
            //makes the player able to block while holding right mouse click
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("Block", true);
                Debug.Log("Block");
            }
            //makes the player able to attack
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetInteger("Hit", 1);
                Debug.Log("Attack");
            }
        }
    }

    //This method is called when the player switches to the Rapier
    public void RapierHabilities()
    {
        //Rapier habilities
    }

    //This method is called when the player switches to the Gun
    public void GunHabilities()
    {
        //Gun habilities
        var gamepad = Gamepad.current;

        //aiming camera with right mouse click
        if (Input.GetMouseButtonDown(1))
        {
            //Camera aiming
            cameraAiming.Priority = 1;
            camera3rdperson.Priority = 0;
            animator.SetBool("Aim", true);
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //Camera 3rd person
            camera3rdperson.Priority = 1;
            cameraAiming.Priority = 0;
            animator.SetBool("Aim", false);
            isAiming = false;
        }

        if (gamepad != null)
        {
            if (gamepad.leftShoulder.isPressed)
            {
                
            }
            else
            {
                
            }
        }
        

    }
    
}
