using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInputNamespace
{
    public class Joystick : MonoBehaviour, ISimpleInputDraggable
    {
        public enum MovementAxes { XandY, X, Y };

        public SimpleInput.AxisInput xAxis = new SimpleInput.AxisInput("Horizontal");
        public SimpleInput.AxisInput yAxis = new SimpleInput.AxisInput("Vertical");

        private RectTransform joystickTR;
        private Graphic background;
        private Graphic goGraphic;
        private float startAlpha;
        public MovementAxes movementAxes = MovementAxes.XandY;
        public float valueMultiplier = 1f;

#pragma warning disable 0649
        [SerializeField]
        private Image thumb;
        private RectTransform thumbTR;

        [SerializeField]
        private float movementAreaRadius = 75f;

        [SerializeField]
        private bool isDynamicJoystick = false;

        [SerializeField]
        private RectTransform dynamicJoystickMovementArea;

        [SerializeField]
        private bool canFollowPointer = false;
#pragma warning restore 0649

        public bool joystickHeld = false;
        private Vector2 pointerInitialPos;

        private float _1OverMovementAreaRadius;
        private float movementAreaRadiusSqr;

        private Vector2 joystickInitialPos;

        private float opacity = 1f;
        private float thumb_opacity = 1f;
        private float thumb_initial_opacity = 1f;

        private Vector2 m_value = Vector2.zero;
        public Vector2 Value { get { return m_value; } }


        public enum JoystickButtonType { movement, ultiAttack, basicAttack }
        public JoystickButtonType joystickButtonType;

        public virtual void Awake()
        {
            InitJoystickStatsOnAwake(thumb);
        }

        public void InitJoystickStatsOnAwake(Image newThumb, bool isThumbRaycastable = false)
        {
            thumb = newThumb;

            joystickTR = (RectTransform)transform;
            thumbTR = thumb.rectTransform;
            goGraphic = GetComponent<Graphic>();
            Graphic bgGraphic = GetComponent<Graphic>();
            if (bgGraphic)
            {
                background = bgGraphic;
                background.raycastTarget = false;
                startAlpha = background.color.a;
                thumb_initial_opacity = thumb.color.a;
            }

            if (isDynamicJoystick)
            {
                //opacity = 0f;
                thumb.raycastTarget = false;

                //OnUpdate();
            }
            else

            {
                if (isThumbRaycastable)
                    thumb.raycastTarget = true;
            }

            _1OverMovementAreaRadius = 1f / movementAreaRadius;
            movementAreaRadiusSqr = movementAreaRadius * movementAreaRadius;

            joystickInitialPos = joystickTR.anchoredPosition;
            thumbTR.localPosition = Vector3.zero;


        }

        public void InitJoystickStatsOnStart()
        {
            SimpleInputDragListener eventReceiver;
            if (!isDynamicJoystick)
            {
                if (thumbTR.gameObject.TryGetComponent(out SimpleInputDragListener simpleInputDragListener))
                {

                    eventReceiver = simpleInputDragListener;

                }
                else
                {
                    eventReceiver = thumbTR.gameObject.AddComponent<SimpleInputDragListener>();

                }

            }
            else
            {
                if (!dynamicJoystickMovementArea)
                {
                    dynamicJoystickMovementArea = new GameObject("Dynamic Joystick Movement Area", typeof(RectTransform)).GetComponent<RectTransform>();
                    dynamicJoystickMovementArea.SetParent(thumb.canvas.transform, false);
                    dynamicJoystickMovementArea.SetAsFirstSibling();
                    dynamicJoystickMovementArea.anchorMin = Vector2.zero;
                    dynamicJoystickMovementArea.anchorMax = Vector2.one;
                    dynamicJoystickMovementArea.sizeDelta = Vector2.zero;
                    dynamicJoystickMovementArea.anchoredPosition = Vector2.zero;
                }

                eventReceiver = dynamicJoystickMovementArea.gameObject.AddComponent<SimpleInputDragListener>();
                eventReceiver.ActivateRaycast();
            }

            eventReceiver.Listener = this;

        }

        public void InitJoystickStats(Image newThumb, bool isActive)
        {
            InitJoystickStatsOnAwake(newThumb, isActive);
            InitJoystickStatsOnStart();


        }

        public void Deactivate()
        {


            goGraphic.raycastTarget = false;

        }
        public void Activate()
        {
            goGraphic.raycastTarget = true;

        }

        private void Start()
        {
            InitJoystickStatsOnStart();
        }

        private void OnEnable()
        {
            xAxis.StartTracking();
            yAxis.StartTracking();

            SimpleInput.OnUpdate += OnUpdate;
        }

        private void OnDisable()
        {
            xAxis.StopTracking();
            yAxis.StopTracking();

            SimpleInput.OnUpdate -= OnUpdate;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            joystickHeld = true;

            if (FindObjectOfType<InputHandler>() != null)
            {
                FindObjectOfType<InputHandler>().PlayerController.DetectJoystickButton(joystickButtonType);

            }





            if (isDynamicJoystick)
            {
                pointerInitialPos = Vector2.zero;

                Vector3 joystickPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(dynamicJoystickMovementArea, eventData.position, eventData.pressEventCamera, out joystickPos);
                joystickTR.position = joystickPos;
            }
            else
                RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTR, eventData.position, eventData.pressEventCamera, out pointerInitialPos);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 pointerPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTR, eventData.position, eventData.pressEventCamera, out pointerPos);

            Vector2 direction = pointerPos - pointerInitialPos;
            if (movementAxes == MovementAxes.X)
                direction.y = 0f;
            else if (movementAxes == MovementAxes.Y)
                direction.x = 0f;

            if (direction.sqrMagnitude > movementAreaRadiusSqr)
            {
                Vector2 directionNormalized = direction.normalized * movementAreaRadius;
                if (canFollowPointer)
                    joystickTR.localPosition += (Vector3)(direction - directionNormalized);

                direction = directionNormalized;
            }

            m_value = direction * _1OverMovementAreaRadius * valueMultiplier;

            thumbTR.localPosition = direction;

            xAxis.value = m_value.x;
            yAxis.value = m_value.y;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (FindObjectOfType<InputHandler>() != null)
            {
                FindObjectOfType<InputHandler>().AttackButtonUp(this);

            }
            joystickHeld = false;
            m_value = Vector2.zero;

            thumbTR.localPosition = Vector3.zero;
            //if( /*!isDynamicJoystick &&*/ /*canFollowPointer*/ )
            joystickTR.anchoredPosition = joystickInitialPos;

            xAxis.value = 0f;
            yAxis.value = 0f;
        }

        private void OnUpdate()
        {
            if (!isDynamicJoystick)
                return;

            if (joystickHeld)
            {

                //opacity = Mathf.Min( 1f, opacity + Time.unscaledDeltaTime * 4f );
                opacity = startAlpha;
                thumb_opacity = thumb_initial_opacity * 2;

            }
            else
            {

                //opacity = Mathf.Max(0f, opacity - Time.unscaledDeltaTime * 4f);
                thumb_opacity = thumb_initial_opacity;

            }

            Color c = thumb.color;
            c.a = thumb_opacity;
            thumb.color = c;

            //if( background )
            //{
            //	c = background.color;
            //	c.a = opacity;
            //	background.color = c;
            //}
        }
    }
}