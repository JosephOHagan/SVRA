using System.Collections;
using UnityEngine;

/*  
 *  --- NOTE ---
 *  This assumes the firing object is held in the user's hand (gun, flare)
 *  A more appropriate name might be held fire projectile, etc.
 *  An additional class which could be implemented would fire fixed projectile (cannon, firework)
 *  
 *  --- TODO ---
 *  Correct the fireType selection within the simple settings header
 *  Currently it defaults to single fire and cannot be set via the
 *  vairable on the front end GUI
 */

public class SVRA_FireProjectile : SVRA_InteractiveObjectClass {
    
    public enum FireTypes { SingleShot, Automatic };

    [Header("Simple Settings")]
    [Tooltip("The button on the controller to fire the projectile")]
    public Valve.VR.EVRButtonId fireButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    [Tooltip("The transform point from which the projectile will fire")]
    public Transform projectileFirePoint;
    [Tooltip("The prefab of the projectile object to fire")]
    public GameObject projectilePrefab;
    [Tooltip("The speed at which the projectile is fired")]
    public float projectileSpeed = 400;
    [Tooltip("The delay between projectiles firing in automatic fire mode")]
    public float automaticFireDelay = 0.2f;

    [Tooltip("The delay between projectiles firing in automatic fire mode")]
    public FireTypes fireType;


    [Header("Advanced Settings")]
    [Tooltip("The default fire mode of the shooter")]
    public FireTypes defaultFireType = FireTypes.SingleShot;
    [Tooltip("TODO")]
    public Material[] shooterTypeMaterials;
    [Tooltip("TODO")]
    public MeshRenderer[] shooterMeshRenderers;
    [Tooltip("The audio source to play upon firing the projectile")]
    public AudioSource projectileAudioSource;

    protected bool automaticFireMode = false;
    protected float restTimer = 0;

    public void Awake()
    {
        // Do something appropriate here
        SVRA_ClickButton.OnButtonPress += ToggleGunType;
    }

    public void Update()
    {
        //If shooter is set to automatic and the trigger is down
        if (automaticFireMode)
        {
            //If it is time to shoot
            if (restTimer > automaticFireDelay)
            {
                //Reset timer, and shoot
                restTimer = 0;
                FireProjectile();
            }
            else
            {
                //Add time to the reset timer
                //Delta time is how much time has passed between this and the last frame.
                restTimer += Time.deltaTime;
            }
        }
    }

    public override void ButtonPressDown(Valve.VR.EVRButtonId button, SVRA_ControllerSetup controller)
    {
        /* --- TODO ---
         * Currently there is no check for whether the held shooting object is actually in your hand before you fire
         * You would have to write in the ability to check if the current object is being held
         */

        //If button is desired "fire" button
        if (button == fireButton)
        {
            //Shoot
            FireProjectile();

            //Haptic pulse
            controller.device.TriggerHapticPulse(2000);

            //Trigger audio
            gunAudioSource.Play();

            //Turn on autofire, if gun is set to automatic
            if (defaultGunType == GunTypes.Automatic)
                autoFire = true;
        }
    }

    public override void ButtonPressUp(EVRButtonId button, VRControllerInput controller)
    {
        //If button is desired "fire" button
        if (button == fireButton)
        {
            //Set autofire to false
            autoFire = false;

            //Reset autofire timer
            restTimer = 0;
        }
    }

    protected void ShootBullet()
    {
        //Create bullet and set it to muzzle's position and rotation
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileExitPoint.position;
        projectile.transform.rotation = projectileExitPoint.rotation;

        //Add force to bullet
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce(transform.forward * projectileSpeed);
    }

    public void FireProjectile()
    {
        // Swap fire type and set to matching material
        // TODO : Rewrite / Improve Code

        if (defaultFireType == FireTypes.SingleShot)
            defaultFireType = FireTypes.SingleShot;
        else
            defaultFireType = FireTypes.Automatic;

        if ((int)defaultFireType < shooterTypeMaterials.Length)
        {
            for (int i = 0; i < shooterMeshRenderers.Length; i++)
                shooterMeshRenderers[i].material = shooterTypeMaterials[(int)defaultFireType];
        }
    }
}

}
