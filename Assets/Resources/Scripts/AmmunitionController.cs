using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionController : MonoBehaviour
{

    private Transform ammoTransform;
    public float ammoSpeed = 5f;
    public float lifeSpan = 5f;
    public enum ammoTypeEnum { Bullet, Rocket, Laser, LaserPink, Plasma, DestructionLaser, BossBullet, BossPlasma, BossLaser }
    public ammoTypeEnum ammoType;
    public float damage = 30f, fireCooldown = 0.75f;
    public bool isExplosive = false;
    public float explosionDamage = 25f;
    public bool isBossAmmo = false;

    public GameObject FuzzParticle, ImpactParticle;

    public string ownerTag = "Nothing";

    // Start is called before the first frame update
    void Start()
    {

        ammoTransform = GetComponent<Transform>();
        ownerTag = ExtensionMethods.getParent(gameObject).tag;
        gameObject.transform.parent = null;
        Instantiate(FuzzParticle, transform.position, transform.rotation);
        Destroy(gameObject, lifeSpan);

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Transform to Y because blender's axes 

        ammoTransform.position += ammoTransform.up * ammoSpeed;

    }

    private void OnTriggerEnter(Collider other)
    {

        // Friendly Fire Off
        if (!GameState.isFriendlyFire)
        {

            if (other.tag == ownerTag || (other.tag == "Base" && ownerTag == "Player") || (other.tag == "Player" && ownerTag == "Base"))
            {
                // Prevent exploiting
                if (other.tag == "Base") ammoHit();
                return;
            }
        }

        try
        {
            if(other.tag =="Player") ExtensionMethods.getParent(other.gameObject).GetComponent<EntityHandler>().getDamage(damage, ownerTag);
            else other.GetComponent<EntityHandler>().getDamage(damage, ownerTag);

        }
        catch (NullReferenceException e)
        {
            // Handle exception
        }

        if (isExplosive)
        {
            // implement explosion / Or Particles on hit?
        }

        // Boss Ammo Handle
        if (!isBossAmmo) ammoHit();
        else if (other.tag == "Base" || other.tag == "Player") ammoHit();
    }

    private void ammoHit()
    {
        GameObject scaleHit = Instantiate(ImpactParticle, transform.position, transform.rotation);
        scaleHit.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        Destroy(gameObject);
    }


}
