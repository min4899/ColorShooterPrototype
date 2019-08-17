using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : MonoBehaviour
{

    public int health = 1;
    public int currentHp;
    public int colorState;
    public int points;
    public float damagedPeriod = 3f;
    public GameObject destructionVFX;
    public GameObject pointText;
    public Material whiteMaterial;
    public SpriteRenderer enemySprite;
    public WashingMachine script;

    private Material defaultMaterial;
    private byte interval;
    private byte red;
    private byte green;
    private byte blue;
    private bool exposed;

    void Start()
    {
        currentHp = health;
        if (enemySprite != null)
        {
            defaultMaterial = enemySprite.GetComponent<SpriteRenderer>().material;
        }
        enemySprite.GetComponent<SpriteRenderer>().color = Color.white;
        interval = (byte)(255 / health);
        red = 255;
        green= 255;
        blue = 255;
        exposed = true;
    }

    void GetDamage(int damage)
    {
        AudioManager.instance.PlaySound("Enemy_Hit");
        currentHp -= damage; 
        if (currentHp <= 0)
        {
            currentHp = 0;
            // activate 2nd phase
            StartCoroutine(Damaged());
        }
        else
        {
            StartCoroutine(TakeDamageEffect());
            if (colorState == 1) // red
            {
                blue -= interval;
                green -= interval;
            }
            else if (colorState == 2) // yellow
            {
                blue -= interval;
            }
            else if (colorState == 3) // green
            {
                red -= interval;
                blue -= interval;
            }
            enemySprite.GetComponent<SpriteRenderer>().color = new Color32(red, green, blue, 255);
        }
    }

    void Heal(int damage)
    {
        AudioManager.instance.PlaySound("Enemy_Hit");
        currentHp += damage;
        if (currentHp >= health) // back to neutral
        {
            currentHp = health;
            colorState = 0;
            enemySprite.GetComponent<SpriteRenderer>().color = Color.white;
            red = 255;
            green = 255;
            blue = 255;
        }
        else
        {
            //StartCoroutine(TakeDamageEffect());
            if (colorState == 1) // red
            {
                blue += interval;
                green += interval;
            }
            else if (colorState == 2) // yellow
            {
                blue += interval;
            }
            else if (colorState == 3) // green
            {
                red += interval;
                blue += interval;
            }
            enemySprite.GetComponent<SpriteRenderer>().color = new Color32(red, green, blue, 255);
        }
    }

    // Set 'WashingMachine' damaged variable true and disable collision for this object for few seconds.
    IEnumerator Damaged()
    {
        exposed = false;
        enemySprite.GetComponent<SpriteRenderer>().color = new Color32(150, 25, 160, 255);
        if (destructionVFX != null)
        {
            AudioManager.instance.PlaySound("Enemy_Explosion");
            Instantiate(destructionVFX, transform.position, Quaternion.identity);
        }
        SetScore();
        script.Damaged();
        yield return new WaitForSeconds(damagedPeriod);
        script.NotDamaged();
        currentHp = health;
        colorState = 0;
        enemySprite.GetComponent<SpriteRenderer>().color = Color.white;
        red = 255;
        green = 255;
        blue = 255;
        exposed = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (exposed)
        {
            if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") ||
                other.CompareTag("EnemyYellowShot") || other.CompareTag("EnemyGreenShot") || other.CompareTag("Boss"))
            {
                return;
            }

            // Reacting to player's shots
            // If its no color, set color
            if (colorState == 0)
            {
                if (other.CompareTag("PlayerRedShot"))
                {
                    colorState = 1;
                    GetDamage(Player.instance.shotDamage);
                }
                else if (other.CompareTag("PlayerYellowShot"))
                {
                    colorState = 2;
                    GetDamage(Player.instance.shotDamage);
                }
                else if (other.CompareTag("PlayerGreenShot"))
                {
                    colorState = 3;
                    GetDamage(Player.instance.shotDamage);
                }
            }
            // Same color, decrease health
            else if (other.CompareTag("PlayerRedShot") && colorState == 1)
            {
                GetDamage(Player.instance.shotDamage);
            }
            else if (other.CompareTag("PlayerYellowShot") && colorState == 2)
            {
                GetDamage(Player.instance.shotDamage);
            }
            else if (other.CompareTag("PlayerGreenShot") && colorState == 3)
            {
                GetDamage(Player.instance.shotDamage);
            }
            else // different color, increase health
            {
                Heal(Player.instance.shotDamage);
            }
        }
        if (other.CompareTag("Player"))
        {
            if (Player.instance.CheckShield()) // shield on
            {
                Player.instance.ChangeToNeutral();
            }
            else // shield off
            {
                Player.instance.GetDamage();
            }
        }
    }

    IEnumerator TakeDamageEffect()
    {
        if (defaultMaterial != null && whiteMaterial != null)
        {
            //gameObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
            enemySprite.material = whiteMaterial;
            yield return new WaitForSeconds(0.05f);
            //gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            enemySprite.material = defaultMaterial;
        }
    }

    void SetScore()
    {
        // Set overall score earned for killing this enemy
        int total = 0;
        if (!Player.instance.CheckShield()) // If player kills an enemy while shield is down, give me more points
        {
            total += 20;
        }

        // increment enemy killed count and overall score in gamecontroller
        if (GameController.instance != null)
        {
            if (colorState == GameController.instance.lastColorDestroyed) // killing same color enemies gives combo
            {
                GameController.instance.colorCombo++;
                //total += points + points * GameController.instance.colorCombo;
            }
            else // not the same color, reset counter
            {
                GameController.instance.lastColorDestroyed = colorState;
                GameController.instance.colorCombo = 1;
                //total += points;
            }
            total += points * GameController.instance.colorCombo;
            GameController.instance.score += total;
            GameController.instance.UpdateScore();
            GameController.instance.UpdateMultiplier();
        }
        else // gamecontroller not on (for testing in editor), just give standard amount of points
        {
            total += points;
        }

        // Activate this enemy's score animation
        if (pointText != null)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            GameObject canvas = GameObject.Find("Canvas");
            GameObject newScoreText = Instantiate(pointText, screenPosition, Quaternion.identity, canvas.transform);
            //newScoreText.transform.position = screenPosition;
            newScoreText.GetComponent<ScoreTextController>().SetText(total.ToString());
            //newScoreText.GetComponent<TextMeshProUGUI>().SetText("+" + total);
        }
    }
}
