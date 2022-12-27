using UnityEngine;
using EricGames.Core.Equipment;
using EricGames.Core.StateMachine;
using EricGames.Core.Mechanics;
using EricGames.Core.Components;

namespace EricGames.Core.Characters
{
    [RequireComponent(typeof(AnimatorTriggerHandler))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class Character : MonoBehaviour
    {
        public enum TriggerType
        {
            ATTACK,
            JUMP,
            DODGE,
        }

        public enum State
        {
            NONE,
            MOVE,
            ATTACK,
            HITTED,
            JUMP,
            FALL,
            DODGE,
            BLOCK,
        }
        public enum Team
        {
            ALLY,
            ENEMY,
        }

        public Character target;

        #region Serialize Field

        private const float INPUT_DIR_CHANGED_THRES = 0.12f;
        [SerializeField] private LayerMask groundMask; // 
        [SerializeField] private bool facingRight = true;
        [SerializeField] private float jumpForce = 7.0f; // decide how height when jumping
        [SerializeField] private float blockingDuration = 0.2f;
        [SerializeField] private Body[] bodies = null;
        [SerializeField] private Weapon[] weapon; // weapon's gameobject

        #region Animator Tag State

        [SerializeField] private string moveStateTag = "Move";
        [SerializeField] private string jumpStateTag = "Jump";
        [SerializeField] private string fallStateTag = "Fall";
        [SerializeField] private string blockStateTag = "Block";
        [SerializeField] private string dodgeStateTag = "Dodge";
        [SerializeField] private string attackStateTag = "Attack";

        #endregion

        #region  Animator Parameter State

        [SerializeField] private string jumpParameter = "Jump";
        [SerializeField] private string fallParameter = "Fall";
        [SerializeField] private string blockParameter = "Block";
        [SerializeField] private string dodgeParameter = "Dodge";
        [SerializeField] private string attackParameter = "Attack";

        #endregion

        #endregion

        #region Private Variables

        private int moveStateTagHash = 0;
        private int jumpStateTagHash = 0;
        private int fallStateTagHash = 0;
        private int dodgeStateTagHash = 0;
        private int blockStateTagHash = 0;
        private int attackStateTagHash = 0;

        private int jumpParameterHash = 0;
        private int fallParameterHash = 0;
        private int dodgeParameterHash = 0;
        private int blockParameterHash = 0;
        private int attackParameterHash = 0;

        private int hp;
        private int mp;

        protected Damage damage;

        protected AnimatorTriggerHandler animatorTriggerHandler;
        protected Animator animator;
        protected Rigidbody2D rb;

        private StateMachine<State, TriggerType> stateMachine;

        [SerializeField]
        private StateMachineGraph stateMachineGraph;


        private float blockingTime = 0.0f;

        #endregion

        private enum LandingState
        {
            GROUNDED,
            JUMPING,
            FALLING,
        }

        private LandingState landingState;

        protected Vector3 forward
        {
            get
            {
                return transform.right * (flip ? -1 : 1);
            }
        }

        public Team team;
        private bool flip;
        bool canDoNext = false;
        bool doubleJump = true;
        public bool blocking = false;
        //public bool awaking = false;
        public bool unstopable = false;

        protected virtual void Awake()
        {
            animatorTriggerHandler = GetComponent<AnimatorTriggerHandler>();
            animator = GetComponent<Animator>();
            bodies = GetComponentsInChildren<Body>();
            rb = GetComponent<Rigidbody2D>();
        }

        private float speedX => Mathf.Abs(targMoveInput.x);
        private float speedY => rb.velocity.y;

        public virtual void Start()
        {
            flip = !facingRight;
            damage = new Damage(this);

            moveStateTagHash = Animator.StringToHash(moveStateTag);
            jumpStateTagHash = Animator.StringToHash(jumpStateTag);
            fallStateTagHash = Animator.StringToHash(fallStateTag);
            dodgeStateTagHash = Animator.StringToHash(dodgeStateTag);
            blockStateTagHash = Animator.StringToHash(blockStateTag);
            attackStateTagHash = Animator.StringToHash(attackStateTag);

            jumpParameterHash = Animator.StringToHash(jumpParameter);
            fallParameterHash = Animator.StringToHash(fallParameter);
            dodgeParameterHash = Animator.StringToHash(dodgeParameter);
            blockParameterHash = Animator.StringToHash(blockParameter);
            attackParameterHash = Animator.StringToHash(attackParameter);

            stateMachine = new StateMachine<State, TriggerType>(State.MOVE);

            InitStateMove();
            InitStateJump();
            InitStateFall();
            InitStateAttack();
            InitStateBlock();
            InitStateDodge();
        }

        void FixedUpdate()
        {
            CheckIsGrounded();
        }

        void Update()
        {
            animator.SetBool("isGrounded", landingState == LandingState.GROUNDED);
            animator.SetBool("unstopable", unstopable);
            animator.SetBool("Can Jump", doubleJump);
            animator.SetFloat("speedX", speedX, 0.02f, Time.deltaTime); // now horizontal input value
            animator.SetFloat("speedY", speedY); // curent vertical speed

            stateMachine?.Tick(Time.deltaTime);
        }

        private void CheckIsGrounded()
        {
            // check ground
            RaycastHit2D hit = Physics2D.Raycast(
                new Vector2(transform.position.x, transform.position.y) + Vector2.up * 0.5f,
                Vector2.down, 0.7f,
                groundMask);

            if (hit.collider != null)
            {
                landingState = LandingState.GROUNDED;
            }
            else if (speedY < 0)
            {
                landingState = LandingState.FALLING;
            }
            else
            {
                landingState = LandingState.JUMPING;
            }

            // Vector3 newPos = transform.position;

            // speedY -= 9.8f * Time.deltaTime;
            // newPos.y += speedY * Time.deltaTime;

            // if (isGrounded)
            // {
            //     if (newPos.y < hit.point.y)
            //     {
            //         speedY = 0.0f;
            //         newPos.y = hit.point.y;
            //     }
            // }

            // newPos.z = 0.0f;

            // transform.position = newPos;
        }

        #region State HITTED

        public delegate Damage OnHittedEventHandler(Damage damage);

        [SerializeField] public OnHittedEventHandler OnHitted;

        public void HandleHitted(Damage damage)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(0);
            var currentTransition = animator.GetAnimatorTransitionInfo(0);

            if (blocking) // blocking
            {
                //triggerHandler.SetTrigger("Hitted when Blocking", 0.1f);

                if (Vector3.Dot(damage.attackFrom, transform.right) < 0.0f)

                    if (blockingTime < blockingDuration) // perfect blocking
                    {
                        damage.enable = false; // disable
                    }
                    else // normal blocking
                    {
                        // todo 
                    }
            }
            else // non-blocking
            {
                //triggerHandler.SetTrigger("Hitted", 0.1f);

                if (animator.IsInTransition(0)) // in transition
                {
                }
                else // in state
                {
                }
            }

            damage = OnHitted.Invoke(damage);

            if (damage.enable)
            {
                damage.enable = false;
            }
        }

        #endregion

        public Vector2 targMoveInput;

        [SerializeField] private GameObject obstacle;

        public bool untouchable = false; // can't touch, will close body collider


        // check current rotation is same as input
        protected void CheckRotation(float movingX)
        {
            if ((movingX > INPUT_DIR_CHANGED_THRES && facingRight == false) || // looking right but wanna look left
                (movingX < -INPUT_DIR_CHANGED_THRES && facingRight == true)) // looking left but wanna look right
            {
                transform.Rotate(Vector3.up * 180.0f);
                facingRight = !facingRight;
            }
        }
    }
}