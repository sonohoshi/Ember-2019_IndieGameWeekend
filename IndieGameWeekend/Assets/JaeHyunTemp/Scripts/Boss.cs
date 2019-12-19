using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//public class BossPattern : Entity
//{

//    int m_seed;
//    [SerializeField]
//    Animator animator;

//    [SerializeField]
//    GameObject IcePillar;
//    [SerializeField]
//    Vector2 temp;


//    [SerializeField]
//    int m_WaitTime;

//    [SerializeField]
//    float interval;


//    public cN_WayBullet m_cN_WayBullet;
//    public float m_time;

//    WaitForSeconds m_waitForSeconds;
//    private void Start()
//    {
//        m_seed = Random.Range(0, 3);
//        animator.SetBool("IsAttack",false);
//        animator.SetBool("IsDie", false);
//        animator.SetBool("IsIdle", true);
//        StartCoroutine(Turn(m_seed));
//    }

//    IEnumerator Turn(int seed)
//    {
//        //int t = 0;
//        //while(t < 1)
//        //{
//        //    yield return null;

//        //}

//        switch(seed)
//        {
//            case 0://아이스 뿅뿅뿅
//                Debug.Log("아이스");
//                animator.SetBool("IsAttack", true);
//                animator.SetBool("IsIdle", false);
//                StartCoroutine(MakePillar(m_WaitTime, interval));
//                break;

//            case 1://nway
//                Debug.Log("nWay");
//                animator.SetBool("IsAttack", true);
//                animator.SetBool("IsIdle", false);
//                StartCoroutine(Shoot());
//                break;

//            case 2://이동
//                animator.SetBool("IsIdle", true);
//                animator.SetBool("IsAttack", false);

//                break;

//            default:
//                animator.SetBool("IsIdle", true);

//                break;
//        }
//        m_seed = Random.Range(0, 3);
//        yield return new WaitForSeconds(2f);
//        StartCoroutine(Turn(m_seed));

//    }

//    IEnumerator MakePillar(float waitTime, float interval)//몇초만에 다시 나올지, 간격
//    {

//        temp = new Vector2(transform.position.x - (interval * 2), transform.position.y);
//        for (int i = 0; i < 5; i++)
//        {
//            temp.x += interval;
//            Instantiate(IcePillar, temp, Quaternion.identity);
//            Destroy(GameObject.Find("Ice(Clone)"), 1f);
//            yield return new WaitForSeconds(waitTime);
//        }
//    }

//    IEnumerator Shoot()
//    {
//        Debug.Log("왜 안들어와");
//        m_waitForSeconds = new WaitForSeconds(m_time);
//        while (true)
//        {
//            yield return null;
//            for (int i = 0; i < m_cN_WayBullet.bulletsSpeedVector.Length; i++)
//            {
//                var temp_Obj = BulletPool.Instance.PopBullet();
//                var bullet = temp_Obj.GetComponent<Bullet>();
//                bullet.speedVector = m_cN_WayBullet.bulletsSpeedVector[i];
//                bullet.axis = transform;
//            }

//            yield return m_waitForSeconds;
//        }
//    }

//}

public partial class Boss : Entity
{

    [Header("UI")]
    public GameObject bossPanel;
    public Image bossHealth;

    [Header("Component")]
    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool isDamage;
    private State<Boss> bossState;

    [Header("Pattern_0")]
    public int pattern_0_damage;
    public ParticleSystem pattern_0_effect;
    public Collider2D pattern_0Box;

    [Header("Pattern_1")]
    public int pattern_1_damage;
    public GameObject iceBreak;
    public Transform pattern_1_start;
    public Transform pattern_1_end;
    private void Awake()
    {
        health = 100;
        bossState = new IdleState();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        State<Boss> nowState = bossState.InputHandle(this);
        bossState.action(this);

        if (!nowState.Equals(bossState))
        {
            bossState = nowState;
        }
        bossHealth.fillAmount = health / 100.0f;
    }
    public void GroundAttack()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        pattern_0Box.OverlapCollider(new ContactFilter2D(), colliders);
        foreach(var collider in colliders)
        {
            if(collider.CompareTag("Player"))
            {
                collider.GetComponent<Entity>().Hurt(pattern_0_damage);
            }
        }
        CameraManager.CameraShake(0.3f,0.2f);
        pattern_0_effect.Play();
    }
    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        isDamage = true;
        StartCoroutine(HurtEffect());
    }
    public override void Dead()
    {
        base.Dead();
        SceneManager.LoadScene(2);
    }
    private IEnumerator HurtEffect()
    {
        GetComponent<SpriteRenderer>().material.SetFloat("_Value", 1);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material.SetFloat("_Value", 0);

    }

}


public partial class Boss : Entity
{
    public class IdleState : State<Boss>
    {
        public override State<Boss> InputHandle(Boss t)  
        {
            if (t.isDamage)
            {
                t.animator.SetTrigger("Walk");
                t.bossPanel.SetActive(true);
                return new CombatState();
            }
            return this;
        }
    }

    public class CombatState : State<Boss>
    {
        private float timer = 0;
        public override State<Boss> InputHandle(Boss t)
        {
            if (timer >= Random.Range(2.0f, 4.0f))
            {
                int random = Random.Range(0, 2  );

                switch(random)
                {
                    case 0:
                        return new Pattern_0_State();
                    case 1:
                        return new Pattern_1_State();
                    case 2:
                        return new Pattern_2_State();
                }
            }
            return this;
        }
        public override void Update(Boss t)
        {
            base.Update(t);
            timer += Time.deltaTime;

        }
    }
    public class Pattern_0_State : State<Boss>
    {
        private float timer = 0;
        public override State<Boss> InputHandle(Boss t)
        {
            if (timer >= 3)
                return new CombatState();
            return this;
        }
        public override void Enter(Boss t)
        {
            base.Enter(t);
            t.animator.SetTrigger("IsAttack");
        }
        public override void Update(Boss t)
        {
            base.Update(t);
            timer += Time.deltaTime;

        }
    }
    public class Pattern_1_State : State<Boss>
    {
        private float timer = 0;
        public override State<Boss> InputHandle(Boss t)
        {
            if (timer >= 3)
                return new CombatState();
            return this;
        }
        public override void Enter(Boss t)
        {
            base.Enter(t);
            t.animator.SetTrigger("IsIcePower"); 
            for (int i = 0; i < 5; i++)
            {
                var pos = new Vector2(Random.Range(t.pattern_1_start.position.x, t.pattern_1_end.position.x), t.pattern_1_start.position.y);
                Instantiate(t.iceBreak, pos, Quaternion.identity);
            }
        }
        public override void Update(Boss t)
        {
            base.Update(t);
            timer += Time.deltaTime;

        }
    }
    public class Pattern_2_State : State<Boss>
    {
        private float timer = 0;
        public override State<Boss> InputHandle(Boss t)
        {
            if (timer >= 3)
                return new CombatState();
            return this;
        }
        public override void Enter(Boss t)
        {
            base.Enter(t);
      
        }
        public override void Update(Boss t)
        {
            base.Update(t);
            timer += Time.deltaTime;

        }
    }
}