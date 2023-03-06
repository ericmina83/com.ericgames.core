using UnityEngine;
using EricGames.Core.Equipment;
using EricGames.Core.StateMachine;
using EricGames.Core.Mechanics;
using EricGames.Core.Components;

namespace EricGames.Core.Characters
{
    [RequireComponent(typeof(AnimatorTriggerHandler))]
    [RequireComponent(typeof(Animator))]
    public abstract partial class Character : MonoBehaviour
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
            MOVEMENT,
            ATTACK,
            HITTED,
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

        private StateMachine<State> stateMachine;

        [SerializeField]
        protected float moveSpeed = 1.0f;

        private float blockingTime = 0.0f;

        #endregion

        protected enum LandingState
        {
            GROUNDED,
            JUMPING,
            FALLING,
        }

        protected LandingState landingState;
        public Team team;
        public bool blocking = false;
        //public bool awaking = false;
        public bool unstopable = false;

        // private float speedX => Mathf.Abs(moveInput.x);
        abstract protected float speedY { get; }
        TriggerHandler<TriggerType> triggerHandler = new TriggerHandler<TriggerType>();

        protected virtual void Awake()
        {
            animatorTriggerHandler = GetComponent<AnimatorTriggerHandler>();
            animator = GetComponent<Animator>();
            bodies = GetComponentsInChildren<Body>();

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
        }


        public virtual void Start()
        {
            damage = new Damage(this);

            stateMachine = new StateMachine<State>(State.MOVEMENT);

            InitStateMovement();
            InitStateAttack();
            InitStateBlock();
            InitStateDodge();
        }

        void FixedUpdate()
        {
            if (CheckIsGrounded())
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

            animator.SetFloat("InputX", moveInput.x, 0.02f, Time.deltaTime); // now horizontal input value
            animator.SetFloat("InputY", moveInput.y, 0.02f, Time.deltaTime); // curent vertical speed

            animator.SetFloat("SpeedY", speedY);
            animator.SetBool(blockParameterHash, blocking);
        }

        void Update()
        {
            OnUpdate();

            var deltaTime = Time.deltaTime;
            triggerHandler?.Tick(deltaTime);
            stateMachine?.Tick(deltaTime);
        }

        protected abstract void OnUpdate();

        protected abstract bool CheckIsGrounded();

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

        public Vector3 lookInput;

        public Vector2 moveInput;

        private bool canRotate = false;

        [SerializeField] private GameObject obstacle;

        public bool untouchable = false; // can't touch, will close body collider


        private void ApplyRotation()
        {
            var magnitude = moveInput.magnitude;

            if (!Mathf.Approximately(magnitude, 0.0f))
            {
                transform.rotation = CheckRotation(moveInput);
            }
        }

        // check current rotation is same as input
        public abstract Quaternion CheckRotation(Vector2 input);
    }
}