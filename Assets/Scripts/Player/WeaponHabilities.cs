using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Entities.UniversalDelegates;
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
    public Transform emissionPoint;
    public GameObject bullet;

    [Header("Camera")]
    public CinemachineCamera camera3rdperson;
    public CinemachineCamera cameraAiming;
    public bool isAiming;
    public bool canAct = false; //decides if the player can make an action or not

    private void Start()
    {
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    public void TriggerAction()
    {
        if (canAct == false)
        {
            canAct = true; //allows the player to make an action
        }
        else if (canAct == true)
        {
            canAct = false; //disables the player to make an action
        }
    }

    private void Update()
    {
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentWeaponIndex == 0)
        {
            GreatSwordHabilities();

            //Block manager
            if (Input.GetMouseButtonUp(1) && canAct)
            {
                animator.SetBool("Block", false);
            }

            //Combat manager
            if (Input.GetMouseButtonDown(0) && canAct)
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

            //Block manager
            if (stateInfo.IsName("R_Deflect") && stateInfo.normalizedTime >= 0.9f)
            {
                animator.SetBool("Block", false);
            }

            //Combat manager
            if (Input.GetMouseButtonDown(0) && canAct)
            {
                if (stateInfo.IsName("R_Attack1") && stateInfo.normalizedTime < 0.9f)
                {
                    animator.SetInteger("Hit", 2);
                }
            }
            if (stateInfo.normalizedTime >= 0.9f && (stateInfo.IsName("R_Attack1") || stateInfo.IsName("R_Attack2") || stateInfo.IsName("R_AttackAir")))
            {
                animator.SetInteger("Hit", 0);
            }
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

    //This method is called when the player switches to the GreatSword
    public void GreatSwordHabilities()
    {
        //GreatSword habilities
        if (player.GetComponent<PlayerMovement>().grounded && stateInfo.IsName("GS_Walk") && animator.GetFloat("Speed") <= 0.1f && animator.GetFloat("Direction") <= 0.1f)
        {
            //makes the player able to block while holding right mouse click
            if (Input.GetMouseButton(1) && canAct)
            {
                animator.SetBool("Block", true);
            }
            //makes the player able to attack
            if (Input.GetMouseButtonDown(0) && canAct)
            {
                animator.SetInteger("Hit", 1);
            }
        }
    }

    //This method is called when the player switches to the Rapier
    public void RapierHabilities()
    {
        //Rapier habilities
        if (Input.GetMouseButtonDown(0) && canAct)
        {
            animator.SetInteger("Hit", 1);
        }
        if (Input.GetMouseButtonDown(1) && canAct)
        {
            animator.SetBool("Block", true);
        }
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

        //shooting
        if (Input.GetMouseButtonDown(0) && isAiming && canAct)
        {
            //tells the animator to add one to the hit integer
            animator.SetTrigger("Shoot");

            // Shoots a bullet from the emission point to the point in the middle of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Vector3 forceDirection;

            // Create a layer mask that excludes the player's layer
            int layerMask = ~LayerMask.GetMask("Player"); // Exclude the "Player" layer

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                forceDirection = (hit.point - emissionPoint.position).normalized;
            }
            else
            {
                forceDirection = ray.direction; // Use the ray's direction if no hit
            }

            GameObject bulletInstance = Instantiate(bullet, emissionPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();

            // Apply force to the bullet
            bulletRb.AddForce(forceDirection * bulletInstance.GetComponent<Bullet>().speed, ForceMode.Impulse);

            // Destroy the bullet after its lifetime
            Destroy(bulletInstance, bulletInstance.GetComponent<Bullet>().lifeTime);
        }
    }
    
}
