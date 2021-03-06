﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the damage and defines whether the projectile belongs to the ‘Enemy’ or to the ‘Player’, whether the projectile is destroyed in the collision, or not and amount of damage.
/// </summary>

public class Projectile : MonoBehaviour {

    public float speed; // from mover script

    //[Tooltip("Damage which a projectile deals to another object. Integer")]
    //public int damage;

    [Tooltip("Whether the projectile belongs to the ‘Enemy’ or to the ‘Player’")]
    public bool enemyBullet = true;

    [Tooltip("Whether the projectile is destroyed in the collision, or not")]
    public bool destroyedByCollision = true;

    [Tooltip("If any changes are made to the projectile in mid air. (physics calc moved to Update)")]
    public bool speedChange = false;

    private Rigidbody2D rb; // from mover script
                            
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.up * speed * Time.deltaTime;
        rb.velocity = transform.up * speed;
    }

    void FixedUpdate() // test
    {
        if(speedChange)
        {
            rb.velocity = transform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //when a projectile collides with another object
    {
        if (enemyBullet && collision.CompareTag("Player")) // shots will only be destoryed if it hits player or enemy
        {
            //Player.instance.GetDamage(damage); 
            if (destroyedByCollision)
                Destruction();
        }
        //else if (!enemyBullet && collision.CompareTag("Enemy"))
        else if (!enemyBullet && (collision.CompareTag("Enemy") || collision.CompareTag("Boss")))
        {
            //collision.GetComponent<Enemy>().GetDamage(damage);
            if (destroyedByCollision)
                Destruction();
        }
    }

    void Destruction() 
    {
        Destroy(gameObject);
    }
}


