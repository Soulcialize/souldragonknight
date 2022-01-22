using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyController : ActorController
{
    public enum Type { KNIGHT, DRAGON }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;

    [Space(10)]

    [SerializeField] private Type type;
    [SerializeField] private LayerMask playerLayerMask;

    private bool isDying = false;

    protected override void Start()
    {
        base.Start();
        GetComponentInParent<Rigidbody2D>().velocity = new Vector2(-1f * speed, 0f);
        if (PhotonNetwork.NickName == "Knight" && type == Type.KNIGHT
            || PhotonNetwork.NickName == "Dragon" && type == Type.DRAGON)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (isDying)
        {
            return;
        }

        Collider2D collider = Physics2D.OverlapCircle(transform.position, ((CircleCollider2D)collider2d).radius, playerLayerMask);
        if (collider != null)
        {
            ActorController playerHit = GetActorControllerFromActorCollider(collider);
            if (!(playerHit.Combat.CombatStateMachine.CurrState is DeadState))
            {
                playerHit.Die();
            }
        }
    }

    public override void Die()
    {
        if (isDying)
        {
            return;
        }

        isDying = true;
        GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        spriteRenderer.enabled = true;
        StartCoroutine(FadeOutAndDestroy(3f));
    }

    private IEnumerator FadeOutAndDestroy(float duration)
    {
        Color color = spriteRenderer.material.color;
        float alpha = color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, 0f, t));
            spriteRenderer.material.color = newColor;
            yield return null;
        }

        Destroy(transform.parent.gameObject);
    }
}
