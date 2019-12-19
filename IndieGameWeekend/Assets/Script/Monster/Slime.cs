using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Slime : Enemy
{
    private State<Slime> slimeState;
    private Rigidbody2D rigidbody;
    private float turnTimer = 0;
    private float direction = 1;
    private float jumpTimer = 0;
    public CircleCollider2D findArea;

    private void Awake()
    {
        slimeState = new IdleState();
        rigidbody = GetComponent<Rigidbody2D>();
        dropHeart = Random.Range(0.0f, 10.0f);
    }
    
    protected override void Update()
    {
        jumpTimer += Time.deltaTime;
        base.Update();
        State<Slime> nowState = slimeState.InputHandle(this);
        slimeState.action(this);

        if (!nowState.Equals(slimeState))
        {
            slimeState = nowState;
        }

        turnTimer += Time.deltaTime;
        if (turnTimer >= 3f)
        {
            SetDir();
            turnTimer = 0f;
        }
    }

    void SetDir()
    {
        direction *= -1;
    }
}

public partial class Slime : Enemy
{
    public class IdleState : State<Slime>
    {
        public override State<Slime> InputHandle(Slime t)
        {
            if(t.isDead)
                return new DeadState();
            
            List<Collider2D> colliders = new List<Collider2D>();
            t.findArea.OverlapCollider(new ContactFilter2D(),colliders);

            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    t.target = col.transform;
                    return new ChaseState();
                }
            }
            return this;
        }

        public override void Update(Slime t)
        {
            base.Update(t);
            float h = 1.2f;
            Vector2 velocity = t.rigidbody.velocity;
            velocity.x = h * t.movement * t.direction;
            velocity.y = 5;
            if (t.jumpTimer >= 2f)
            {
                t.rigidbody.velocity = velocity;
                t.jumpTimer = 0f;
            }
            
        }
    }
    
    public class ChaseState : State<Slime>
    {
        
        public override State<Slime> InputHandle(Slime t)
        {
            if(t.isDead)
                return new DeadState();
            
            List<Collider2D> colliders = new List<Collider2D>();
            t.findArea.OverlapCollider(new ContactFilter2D(),colliders);
            
            return this;
        }

        public override void Update(Slime t)
        {
            base.Update(t);
            Vector2 velocity;
            velocity.x = (t.target.transform.position - t.transform.position).normalized.x * t.movement;
            if (velocity.x < 0)
            {
                t.transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                t.transform.localScale = new Vector3(-1,1,1);
            }
            velocity.y = 4;
            if (t.jumpTimer >= 2f)
            {
                t.rigidbody.velocity = velocity;
                t.jumpTimer = 0f;
            }
        }
    }
    
    public class DeadState : State<Slime>
    {
        public override State<Slime> InputHandle(Slime t)
        {
            base.Update(t);
            return this;
        }

        public override void Enter(Slime t)
        {
            base.Enter(t);
            Destroy(t.rigidbody);
            Destroy(t.GetComponent<Collider2D>());
            t.anim.SetBool("isDead", t.isDead);
            Destroy(t.gameObject, 1.5f);
            if (t.dropHeart >= 8.0f)
                Instantiate(t.heart, t.transform.position, Quaternion.identity);
        }
    }
}
