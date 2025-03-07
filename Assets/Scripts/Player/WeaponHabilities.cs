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

    [Header("Camera")]
    public CinemachineCamera camera3rdperson;
    public CinemachineCamera cameraAiming;

    private void Start()
    {
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
    }

    private void Update()
    {
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
        if (currentWeaponIndex == 0)
        {
            GreatSwordHabilities();
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
        //GreatSword habilities
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
            Debug.Log("Aiming");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //Camera 3rd person
            camera3rdperson.Priority = 1;
            cameraAiming.Priority = 0;
            Debug.Log("Not Aiming");
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
