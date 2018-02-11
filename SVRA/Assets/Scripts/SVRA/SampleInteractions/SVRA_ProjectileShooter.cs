using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ProjectileShooter : MonoBehaviour {

    [System.Serializable]
    public class ProjectileFiringCharacteristics
    {
        [Tooltip("Fire in the direction of the particular axis")]
        public bool direction;

        [Tooltip("Apply a firing force in the direction of the particlar axis")]
        public bool directionForce;

        [Tooltip("Distance to offset the firing position by on the particlar axis")]
        public float offset;        
    }

    [System.Serializable]
    public class ProjectileCharacteristics
    {        
        public float projectileSize = 0.1f;
        public float projectileSpeed = 500f;
        public float projectileCooldown = 0.2f;

        public ProjectileFiringCharacteristics greenAxis;
        public ProjectileFiringCharacteristics blueAxis;                
        public ProjectileFiringCharacteristics redAxis;

    }

    private bool projectileFiring = false;
    private float cooldown = 0f;
    private SVRA_ControllerManager controller;

    public ProjectileCharacteristics projectileCharacteristics;

    [Tooltip("Prefab or GameObject of projectile")]
    public GameObject projectile;

    [Tooltip("Trigger vibration on firing")]
    public bool vibration = true;

    
    
    void Update()
    {
        if (!projectileFiring)
        {
            return;
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            if (vibration)
            {
                controller.Vibration(50, 0.1f);
            }
            
            FireProjectile();

            cooldown = projectileCharacteristics.projectileCooldown;
        }
    }

    void SVRAInteractionStart(SVRA_GrabPoint grabPoint)
    {
        if (grabPoint.HoldingSomething())
        {
            projectileFiring = true;
            controller = grabPoint.controller;
        }
    }

    void SVRAGrabStop()
    {
        StopFiring();
    }

    void SVRAInteractionStop()
    {
        StopFiring();
    }

    void StopFiring()
    {
        projectileFiring = false;
        controller = null;
        cooldown = 0f;
    }

    void FireProjectile()
    {
        Vector3 location = transform.position + (transform.up * projectileCharacteristics.greenAxis.offset * BoolToInt(projectileCharacteristics.greenAxis.direction)) +
            (transform.forward * projectileCharacteristics.blueAxis.offset * BoolToInt(projectileCharacteristics.blueAxis.direction)) +
            (transform.right * projectileCharacteristics.redAxis.offset * BoolToInt(projectileCharacteristics.redAxis.direction));

        GameObject instance = (GameObject)Instantiate(projectile, location, Quaternion.identity);

        instance.transform.localScale = Vector3.one * projectileCharacteristics.projectileSize;

        instance.GetComponent<Rigidbody>().AddForce(transform.up * projectileCharacteristics.projectileSpeed * BoolToInt(projectileCharacteristics.greenAxis.directionForce));
        instance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileCharacteristics.projectileSpeed * BoolToInt(projectileCharacteristics.blueAxis.directionForce));        
        instance.GetComponent<Rigidbody>().AddForce(transform.right * projectileCharacteristics.projectileSpeed * BoolToInt(projectileCharacteristics.redAxis.directionForce));
    }

    private int BoolToInt(bool input)
    {
        return input ? 1 : 0;
    }
}
