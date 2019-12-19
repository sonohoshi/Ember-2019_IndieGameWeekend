using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Snowman : Enemy
{
    private State<Snowman> SnowmanState;
    private Rigidbody2D rigidbody;
    private float turnTimer = 0;
    private float direction = 1;
    public CircleCollider2D findArea;

    private void Awake()
    {
        SnowmanState = new IdleState();
        rigidbody = GetComponent<Rigidbody2D>();
        dropHeart = Random.Range(0.0f, 10.0f);
    }
    
    protected override void Update()
    { 
        base.Update();
        State<Snowman> nowState = SnowmanState.InputHandle(this);
        SnowmanState.action(this);

        if (!nowState.Equals(SnowmanState))
        {
            SnowmanState = nowState;
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

public partial class Snowman : Enemy
{
    public class IdleState : State<Snowman>
    {
        public override State<Snowman> InputHandle(Snowman t)
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

        public override void Update(Snowman t)
        {
            base.Update(t);
            float h = 1.2f;
            Vector2 velocity = t.rigidbody.velocity;
            velocity.x = h * t.movement * t.direction;
            t.rigidbody.velocity = velocity;
        }
    }
    
    public class ChaseState : State<Snowman>
    {
        
        public override State<Snowman> InputHandle(Snowman t)
        {
            if(t.isDead)
                return new DeadState();
            
            List<Collider2D> colliders = new List<Collider2D>();
            t.findArea.OverlapCollider(new ContactFilter2D(),colliders);
            
            return this;
        }

        public override void Update(Snowman t)
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
            velocity.y = t.rigidbody.velocity.y;
            t.rigidbody.velocity = velocity;
        }
    }
    
    public class DeadState : State<Snowman>
    {
        public override State<Snowman> InputHandle(Snowman t)
        {
            base.Update(t);
            return this;
        }

        public override void Enter(Snowman t)
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
