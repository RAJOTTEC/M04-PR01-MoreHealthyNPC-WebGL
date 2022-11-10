using System;
using UnityEngine;

public class StandardHealth : MonoBehaviour, IHealth
{
    [SerializeField] private bool dirRight = true;
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private int startingHealth = 100;

    private int currentHealth;

    private float timeRemaining = 1;

    public event Action<float> OnHPPctChanged = delegate { };
    public event Action OnDied = delegate { };

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public float CurrentHpPct
    {
        get { return (float)currentHealth / (float)startingHealth; }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

        currentHealth -= amount;

        OnHPPctChanged(CurrentHpPct);

        if (CurrentHpPct <= 0)
            Die();
    }

    private void Die()
    {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }

    private void Update()
    {
        if (dirRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
        }
        if (transform.position.x >= 4.0f)
        {
            dirRight = false;
        }
        if (transform.position.x <= -4)
        {
            dirRight = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(startingHealth / 10);
            timeRemaining = 1;
        }

        else
        {
            timeRemaining -= Time.deltaTime;

            if(timeRemaining <= 0)
            {
                currentHealth = startingHealth;
                OnHPPctChanged(CurrentHpPct);
            }
        }
    }
}
