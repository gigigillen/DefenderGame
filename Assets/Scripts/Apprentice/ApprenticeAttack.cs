using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeAttack : MonoBehaviour { 

    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 1f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start() {

        attackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update() {

        if (attacking) {
            timer += Time.deltaTime;

            if (timer >= timeToAttack) {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    public void Attack() {

        attacking = true;
        attackArea.SetActive(attacking);
    }
}
