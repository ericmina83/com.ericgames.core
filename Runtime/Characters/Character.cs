using UnityEngine;
using EricGames.Runtime.StateMachine;
using EricGames.Runtime.Components;

#nullable enable

namespace EricGames.Runtime.Characters
{
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
            HIT,
            DODGE,
            BLOCK,
        }

        public enum Team
        {
            ALLY,
            ENEMY,
        }

        [SerializeField] private float blockingDuration = 0.2f;

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
        [SerializeField] private string hitParameter = "Hit";

        #endregion

        #region Private Variables

        private int moveStateTagHash = 0;
        private int jumpStateTagHash = 0;
        private int fallStateTagHash = 0;
        private int dodgeStateTagHash = 0;
        private int blockStateTagHash = 0;
        private int attackStateTagHash = 0;

        private int hitParameterHash = 0;
        private int jumpParameterHash = 0;
        private int fallParameterHash = 0;
        private int dodgeParameterHash = 0;
        private int blockParameterHash = 0;
        private int attackParameterHash = 0;

        private int hp;
        private int mp;

        protected Animator animator;

        protected AnimatorTriggerHandler animatorTriggerHandler;
        private readonly StateMachine<State> stateMachine = new(State.MOVEMENT);
        private readonly TriggerHandler<TriggerType> triggerHandler = new();

        private float blockingTime = 0.0f;

        #endregion

        protected enum LandingState
        {
            NONE,
            GROUNDED,
            JUMPING,
            FALLING,
        }

        protected LandingState landingState = LandingState.NONE;
        public Team team;
        //public bool awaking = false;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            animatorTriggerHandler = new AnimatorTriggerHandler(animator);

            moveStateTagHash = Animator.StringToHash(moveStateTag);
            jumpStateTagHash = Animator.StringToHash(jumpStateTag);
            fallStateTagHash = Animator.StringToHash(fallStateTag);
            dodgeStateTagHash = Animator.StringToHash(dodgeStateTag);
            blockStateTagHash = Animator.StringToHash(blockStateTag);
            attackStateTagHash = Animator.StringToHash(attackStateTag);

            hitParameterHash = Animator.StringToHash(hitParameter);
            jumpParameterHash = Animator.StringToHash(jumpParameter);
            fallParameterHash = Animator.StringToHash(fallParameter);
            dodgeParameterHash = Animator.StringToHash(dodgeParameter);
            blockParameterHash = Animator.StringToHash(blockParameter);
            attackParameterHash = Animator.StringToHash(attackParameter);

            OnAwake();
        }

        protected abstract void OnAwake();

        private void Start()
        {
            InitStateMovement();
            InitStateAttack();
            InitStateBlock();
            InitStateDodge();

            OnStart();
        }

        protected abstract void OnStart();

        private void Update()
        {
            if (SpeedY > 0.1f)
            {
                landingState = LandingState.JUMPING;
            }
            else if (CheckIsGrounded())
            {
                landingState = LandingState.GROUNDED;
            }
            else
            {
                landingState = LandingState.FALLING;
            }

            animator.SetFloat("InputX", moveInput.x, 0.02f, Time.deltaTime); // now horizontal input value
            animator.SetFloat("InputY", moveInput.y, 0.02f, Time.deltaTime); // current vertical speed

            animator.SetBool("Is Grounded", landingState == LandingState.GROUNDED);
            animator.SetFloat("SpeedY", SpeedY);
            animator.SetBool(blockParameterHash, blocking);

            OnUpdate();

            var deltaTime = Time.deltaTime;
            triggerHandler.Tick(deltaTime);
            stateMachine.Tick(deltaTime);
            animatorTriggerHandler.Tick(deltaTime);
        }

        protected abstract void OnUpdate();

        protected abstract bool CheckIsGrounded();


        public Vector3 lookInput;

        public Vector2 moveInput;

        [SerializeField] private GameObject? obstacle;

        public bool untouchable = false; // can't touch, will close body collider
    }
}