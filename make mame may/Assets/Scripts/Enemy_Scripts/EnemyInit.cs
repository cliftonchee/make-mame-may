namespace Enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyInit : MonoBehaviour
    {
        [SerializeField] public int maxHealth = 100;
        int currentHealth;

        public Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            // Play hurt animation
            animator.SetTrigger("IsHurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Enemy died!");

            // Die animation
            animator.SetBool("IsDead", true);

            // Disable enemy
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }
    }
}
