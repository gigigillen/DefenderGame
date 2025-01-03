using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexEffect : MonoBehaviour {

    [SerializeField] private float duration = 5f;
    [SerializeField] private int damagePerTick = 1;
    [SerializeField] private float tickRate = 1f;
    [SerializeField] private float radius = 3f;

    [SerializeField] private ParticleSystem vortexParticles;

    private float nextTickTime;
    private bool isActive;


    public void StartVortex() {
        isActive = true;
        nextTickTime = Time.time + tickRate;


        if (vortexParticles != null) {
            vortexParticles.Play();
        }
    }

    private void Update() {
        if (!isActive) return;

        if (Time.time >= nextTickTime) {
            ApplyAreaDamage();
            nextTickTime = Time.time + tickRate;
        }

        duration -= Time.deltaTime;
        if (duration <= 0) {
            StartCleanup();
        }
    }

    private void ApplyAreaDamage() {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in hitColliders) {
            if (col.CompareTag("Enemy") || col.CompareTag("Wizard")) {
                Health health = col.GetComponent<Health>();
                if (health != null) {
                    health.Damage(damagePerTick);
                }
            }
        }
    }

    private void StartCleanup() {
        isActive = false;

        if (vortexParticles != null) {
            var main = vortexParticles.main;
            main.loop = false; 

            float destroyDelay = main.duration;
            Destroy(gameObject, destroyDelay);
        }
        else {

            Destroy(gameObject);
        }
    }
}

