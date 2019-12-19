using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public partial class Player : Entity
{
    private float gardTime = 0;
    public GameObject gameOver;
    public float energy;
    public Weapon weapon;
    public ParticleSystem dashEffect;
    public Transform hand;
    public BoxCollider2D jumpArea;
    public SpriteRenderer spriteRenderer;
    private State<Player> playerState;
    public Rigidbody2D rigidbody;

    private Animator animator;
    private float attackTimer = 0;
    private PostProcessVolume volume;
    private int flag = 0;
    private void Awake()
    {
        energy = 100;
        volume = FindObjectOfType<PostProcessVolume>();
        playerState = new IdleState();
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (health == 0)
            return;
        gardTime -= Time.deltaTime;
        Color color = spriteRenderer.color;
        color.a = (gardTime > 0) ? 0.5f : 1;
        spriteRenderer.color = color;

        State<Player> nowState = playerState.InputHandle(this);
        playerState.action(this);

        if (!nowState.Equals(playerState))
        {
            playerState = nowState;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
            foreach (var collider in colliders)
            {
                var item = collider.GetComponent<WeaponItems>();
                if (item != null)
                {
                    SetWeapon(item.GetWeapon());
                    Destroy(item.gameObject);
                }
            }
        }

        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouse.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            hand.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            hand.transform.localScale = new Vector3(-1, -1, 1);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        var dis = mouse - transform.position;

        float angle = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
        hand.transform.eulerAngles = new Vector3(0, 0, angle);
        attackTimer += Time.deltaTime;
        if (weapon != null)
        {
            if (Input.GetMouseButtonDown(0) && weapon.IsAttack(attackTimer))
            {
                if (weapon is IMouseDown)
                {
                    attackTimer = 0;

                    var down = weapon as IMouseDown;
                    down.MouseDown(this);
                }
            }
            if (Input.GetMouseButton(0) && weapon.IsAttack(attackTimer))
            {
                if (weapon is IMouseStay)
                {
                    attackTimer = 0;

                    var stay = weapon as IMouseStay;
                    stay.MouseStay(this);
                }
            }
            if (Input.GetMouseButtonUp(0) && weapon.IsAttack(attackTimer))
            {
                if (weapon is IMouseUp)
                {
                    attackTimer = 0;

                    var up = weapon as IMouseUp;
                    up.MouseUp(this);
                }
                Debug.Log(1);
            }
        }
    }
    public override void Dead()
    {
        base.Dead();
        Time.timeScale = 0 ;
        gameOver.SetActive(true);
    }
    public override void Hurt(int damage)
    {
        if (gardTime > 0)
            return;
        gardTime = 1.25f;
        base.Hurt(damage);
        ColorGrading colorGrading;
        volume.profile.TryGetSettings(out colorGrading);
        colorGrading.enabled.value = true;
        colorGrading.saturation.value = -100*(1-((float)health / 100));
        CameraManager.CameraShake();
        switch(flag)
        {
            case 0:
                if(health<=75)
                {
                    DamageTextManager.Instance.ShowHealthShow(0);
                    flag = 1;
                }
                break;
            case 1:
                if (health <= 50)
                {
                    DamageTextManager.Instance.ShowHealthShow(1);
                    flag = 2;
                }
                break;
            case 2:
                if (health <= 25)
                {
                    DamageTextManager.Instance.ShowHealthShow(2);
                    flag = 3;
                }
                break;

        }
        SoundManager.Instance.PlayEffect($"Hit_{Random.Range(0, 2)}");
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

    }
    public void SetWeapon(Weapon weapon)
    {
        if(this.weapon!=null)
            Destroy(this.weapon.gameObject);

        this.weapon = Instantiate(weapon, hand.position, Quaternion.identity, hand);
        this.weapon.transform.localEulerAngles = new Vector3(0, 0, 0);

    }
}
public partial class Player : Entity
{
    public class IdleState : State<Player>
    {
        public override State<Player> InputHandle(Player t)
        {    
            if (Input.GetKeyDown(KeyCode.Space)&&t.energy>=10)
            {
                return new JumpState();
            }
            float h = Input.GetAxis("Horizontal");
            if (h != 0.0f&&t.energy>=5.0f) 
            {
                Debug.Log(1);
                return new MoveState();
            }
            if (Input.GetMouseButtonDown(1) && t.energy >= 20)
            {
                return new DashState();
            }
            return this;
        }
        public override void Enter(Player t)
        {
            base.Enter(t);
            t.animator.SetBool("Move", false);
        }
        public override void Update(Player t)
        {
            base.Update(t);
            t.energy += Time.deltaTime * 25;
            t.energy= Mathf.Clamp(t.energy, 0, 100);
        }
    }
    public class MoveState : State<Player>
    {
        Coroutine coroutine;
        public override State<Player> InputHandle(Player t)
        {
            if (Input.GetKeyDown(KeyCode.Space) && t.energy >= 10)
            {
                action = Exit;
                return new JumpState();
            }
            float h = Input.GetAxis("Horizontal");
            if (t.energy <= 0|| h == 0.0f)
            {
                action = Exit;
                return new IdleState();
            }
            if(Input.GetMouseButtonDown(1) && t.energy >= 5.0f)
            {
                action = Exit;
                return new DashState();
            }
            return this;
        }
        public override void Enter(Player t)
        {
            base.Enter(t);
            coroutine= t.StartCoroutine(MoveSound(t));
            t.animator.SetBool("Move", true);
        }
        public IEnumerator MoveSound(Player p)
        {
            while(true)
            {
                yield return new WaitForSeconds(0.2f);
                if(p.rigidbody.velocity.y<0.1f)
                SoundManager.Instance.PlayEffect("Move_0");
                yield return new WaitForSeconds(0.2f);
                if (p.rigidbody.velocity.y < 0.1f)

                    SoundManager.Instance.PlayEffect("Move_1");
            }
        }
        public override void Update(Player t)
        {
            base.Update(t);
            float h = Input.GetAxis("Horizontal");
           float v=(h* t.movement);
            t.energy -= Time.deltaTime * 2;
            t.rigidbody.velocity = new Vector2(v, t.rigidbody.velocity.y);
            Debug.Log(12);
        }
        public override void Exit(Player t)
        {
            base.Exit(t);
            if(coroutine!=null)
            t.StopCoroutine(coroutine);
        }
    }

    public class JumpState : State<Player>
    {
        private const float jumpforce = 4;
        public override State<Player> InputHandle(Player t)
        {
            if (Input.GetMouseButtonDown(1)&&t.energy>=20)
            {
                return new DashState();
            }
            List<Collider2D> colliders = new List<Collider2D>();
            if(t.jumpArea.OverlapCollider(new ContactFilter2D(),colliders)>0)
            {

                foreach (var col in colliders)
                {
                    if (col.gameObject == t.gameObject)
                    {
                        continue;
                    }
                    else
                        return new IdleState();
                }
            }
            return this;
        }

        public override void Enter(Player t)
        {
            base.Enter(t);
            t.energy -= 10;
            Vector2 v = t.rigidbody.velocity;
            v.y = jumpforce;
            t.rigidbody.velocity = v;
            float h = Input.GetAxis("Horizontal");
            float a = (h * t.movement);
            t.rigidbody.velocity = new Vector2(a, t.rigidbody.velocity.y);
        }
    }
    public class DashState : State<Player>
    {
        private const float jumpforce = 4;
        private bool isDash =true;
        public override State<Player> InputHandle(Player t)
        {
            if(!isDash)
            {
                return new IdleState();
            }
            return this;
        }
        IEnumerator Dash(Player player)
        {
            player.dashEffect.Play();
            SoundManager.Instance.PlayEffect("Dash");
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dis = (mouse - (Vector2)player.transform.position).normalized;
            player.rigidbody.gravityScale = 0;
            float t = 0;
            while (t<0.1f)
            {   
                t += Time.deltaTime;
                player.rigidbody.MovePosition((Vector2)player.transform.position + dis*0.25f);
                yield return null;
            }

            player.dashEffect.Stop();
            player.rigidbody.velocity = Vector2.zero;
            player.rigidbody.gravityScale = 1;
            isDash = false;
        }
        public override void Enter(Player t)
        {
            base.Enter(t);
            t.energy -= 20;
            t.StartCoroutine(Dash(t));
        }
    }
}