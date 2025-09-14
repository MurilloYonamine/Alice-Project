using System.Collections;
using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable {
    private Rigidbody2D _rigidBody2D;
    private bool canTurnInvencible = false;

    [SerializeField] private float _health;
    [SerializeField] private float _invencibleTimeElapsed;
    [SerializeField] private bool _targetable;
    [SerializeField] private bool _invencible;
    [SerializeField] private bool _disableSimulation;
    [SerializeField] public Collider2D _physicsCollider;
    [SerializeField] private LayerMask _groundLayer;

    public float Health {
        get => _health;
        set {
            _health = value;
            if (_health <= 0) {
                Targetable = false;
            }
        }
    }
    public bool Targetable {
        get => _targetable; set {
            if (_disableSimulation) {
                _rigidBody2D.simulated = false;
            }
            _physicsCollider.enabled = value;
        }
    }
    public bool Invencible {
        get => _invencible; set {
            _invencible = value;

            if (_invencible == true) {
                _invencibleTimeElapsed = 0f;
            }
        }
    }

    private void Start() {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void OnHit(float damage, Vector2 knockback) {
        if (!Invencible) {
            Health -= damage;

            //_rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);

            Invencible = true;
            _physicsCollider.enabled = !Invencible;
            StartCoroutine(InvencibleTime());
        }
    }

    private IEnumerator InvencibleTime() {
        _rigidBody2D.gravityScale = -35f;

        float timer = 1f;
        StartCoroutine(HitTwinkle(timer));
        yield return new WaitForSeconds(timer);

        _rigidBody2D.gravityScale = 1f;
        Invencible = false;
        _physicsCollider.enabled = !Invencible;
    }

    private IEnumerator HitTwinkle(float timer) {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime <= timer) {
            spriteRenderer.color = Color.Lerp(originalColor, Color.white, Mathf.PingPong(Time.time * 5f, 1f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    public void OnHit(float damage) {
        if (!Invencible) {
            Health -= damage;

            Invencible = true;
        }
    }

    public void OnObjectDestroyed() {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        // Comentado: não cancela mais a invencibilidade ao tocar o chão
        // if (((1 << collision2D.gameObject.layer) & _groundLayer) != 0) {
        //     Invencible = false;
        // }
    }
}