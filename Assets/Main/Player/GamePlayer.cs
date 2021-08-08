using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Prefab;

public class GamePlayer : GrabberBehaviour, INetworkHandler
{
    [SerializeField] PlayerIKAnchor m_PlayerIKAnchor = default;
    [SerializeField] PlayerHand m_LeftHand = default, m_RightHand = default;
    PoseController m_PoseController;
    HandController m_LeftHandController, m_RightHandController;

    public NetworkBehaviour FindNetworkBehaviour(ushort behaviourId)
    => GetNetworkBehaviour(behaviourId);

    CancellationTokenSource m_OVRTraceCTS;

    public IGrabber GetGrabber(int index) => m_Grabbers[index];
    private void Awake()
    {
        m_LeftHand.Init();
        m_RightHand.Init();
    }
    private void Start()
    {
        m_LeftHandController = new HandController(true, m_LeftHand, IsOwner);
        m_RightHandController = new HandController(false, m_RightHand, IsOwner);
        m_PoseController = new PoseController(GetComponent<Animator>(), m_PlayerIKAnchor, m_LeftHand, m_RightHand);
        if (IsOwner)
            IKTrace(m_OVRTraceCTS.Token).Forget();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_OVRTraceCTS = new CancellationTokenSource();
        m_LeftHand.OnSpawn();
        m_RightHand.OnSpawn();

    }
    override public void OnPool()
    {
        m_OVRTraceCTS?.Cancel();
        m_OVRTraceCTS = null;
        m_LeftHand.OnPool();
        m_RightHand.OnPool();
        m_LeftHandController = null;
        m_RightHandController = null;
        m_PoseController = null;
        base.OnPool();
    }
    private void OnAnimatorIK()
    {
        m_PoseController.ApplyIK();
    }
    async UniTaskVoid IKTrace(CancellationToken token)
    {
        var rootAnchor = m_PlayerIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.Root);
        var lookAnchor = m_PlayerIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.LookTarget);
        var eyeAnchor = m_PlayerIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.Eye);
        var leftHandAnchor = m_PlayerIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.LeftHand);
        var rightHandAnchor = m_PlayerIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.RightHand);

        var ovrIKAnchor = OVRRigHandler.PlayerIKAnchor;
        var ovrRoot = ovrIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.Root);
        var ovrLeft = ovrIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.LeftHand);
        var ovrRight = ovrIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.RightHand);
        var ovrLook = ovrIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.LookTarget);
        var ovrEye = ovrIKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.Eye);

        await UniTaskAsyncEnumerable.EveryUpdate().ForEachAsync(_ =>
        {
            var rot = ovrRoot.rotation.eulerAngles;
            rootAnchor.SetPositionAndRotation(ovrRoot.position, Quaternion.Euler(new Vector3(0, rot.y, rot.z)));
            // playerRoot.SetPositionAndRotation(ovrRoot.position, ovrRoot.rotation);
            leftHandAnchor.SetPositionAndRotation(ovrLeft.position, ovrLeft.rotation);
            rightHandAnchor.SetPositionAndRotation(ovrRight.position, ovrRight.rotation);
            lookAnchor.SetPositionAndRotation(ovrLook.position, ovrLook.rotation);
            eyeAnchor.SetPositionAndRotation(ovrEye.position, ovrEye.rotation);
        }, token);

    }
    class PoseController
    {
        bool IsActive = true;
        Animator m_Animator;
        HandShape m_LeftHandShape = default, m_RightHandShape = default;
        Transform m_LeftThumbTransform, m_RightThumbTransform, m_LeftTarget, m_RightTarget, m_LookTarget;
        HumanPoseHandler m_HumanPoseHandler;
        HandController m_LeftHandController, m_RightHandController;
        HumanPose m_HumanPose;

        public PoseController(Animator animator, PlayerIKAnchor iKAnchor, PlayerHand leftHand, PlayerHand rightHand)
        {
            m_Animator = animator;
            var thumb = FingerName.Thumb;
            m_LeftThumbTransform = leftHand.HandTransform.GetFingerTransform(thumb, 0);
            m_RightThumbTransform = rightHand.HandTransform.GetFingerTransform(thumb, 0);

            m_LeftTarget = iKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.LeftHand);
            m_RightTarget = iKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.RightHand);
            m_LookTarget = iKAnchor.GetIKAnchor(PlayerIKAnchor.AnchorBone.LookTarget);

            m_HumanPoseHandler = new HumanPoseHandler(animator.avatar, animator.transform);

            leftHand.HandShapeAsObservable.Subscribe(shape => m_LeftHandShape = shape).AddTo(leftHand);
            rightHand.HandShapeAsObservable.Subscribe(shape => m_RightHandShape = shape).AddTo(rightHand);
        }
        void OverwriteHandPose(HandShape hand, ref int id)
        {
            OverwriteFingerPose(hand.Thumb, ref id);
            OverwriteFingerPose(hand.Index, ref id);
            OverwriteFingerPose(hand.Middle, ref id);
            OverwriteFingerPose(hand.Ring, ref id);
            OverwriteFingerPose(hand.Little, ref id);
        }
        public void OverwriteThumbRotate()
        {
            m_LeftThumbTransform.Rotate(Vector3.left, m_LeftHandShape.ThumbRotation * 90f, Space.Self);
            m_RightThumbTransform.Rotate(Vector3.left, m_RightHandShape.ThumbRotation * 90f, Space.Self);
        }
        void OverwriteFingerPose(Finger finger, ref int id)
        {
            m_HumanPose.muscles[id++] = finger.Streached1;
            m_HumanPose.muscles[id++] = finger.Spread;
            m_HumanPose.muscles[id++] = finger.Streached2;
            m_HumanPose.muscles[id++] = finger.Streached3;
        }

        private void LateUpdate()
        {
            if (IsActive)
            {
                m_HumanPoseHandler.GetHumanPose(ref m_HumanPose);
                int id = 55;
                OverwriteHandPose(m_LeftHandShape, ref id);
                OverwriteHandPose(m_RightHandShape, ref id);
                m_HumanPoseHandler.SetHumanPose(ref m_HumanPose);
                OverwriteThumbRotate();
            }
        }
        public void ApplyIK()
        {
            m_Animator.SetLookAtWeight(1, 0.2f, 0.9f, 0.7f);
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftTarget.position);
            m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_RightTarget.position);
            m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftTarget.rotation);
            m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_RightTarget.rotation);
            m_Animator.SetLookAtPosition(m_LookTarget.position);
        }
    }
    class HandController : IControllable
    {
        PlayerHand m_PlayerHand;
        ReactiveProperty<HandState> m_HandStateRP = new ReactiveProperty<HandState>();
        public HandState HandState { set { m_HandStateRP.Value = value; } get { return m_HandStateRP.Value; } }
        public IObservable<HandShape> HandShapeAsObservable { private set; get; }
        List<IDisposable> m_Subscriptions;

        public HandController(bool isLeft, PlayerHand playerHand, bool isMine)
        {
            var laser = playerHand.LaserTargetFinder;

            m_PlayerHand = playerHand;
            var wispHand = m_PlayerHand.WispHand;
            var grabber = playerHand.HandGrabber;

            HandShapeAsObservable = grabber.TargetAsObservable.Select(target =>
            {
                if (target != null)
                    return target.HandShapeAsObservable;
                else
                    return m_PlayerHand.HandShapeAsObservable;
            }).Switch().Publish().RefCount();

            CancellationTokenSource traceCTS = default;
            m_Subscriptions = new List<IDisposable>(new[]{

                //LaserFocus
                Observable.CombineLatest(laser.TargetAsObservable, grabber.TargetAsObservable, (target, grabbable) => (target, grabbable)).Subscribe(tuple =>
                {
                    var target = tuple.target;
                    if (target is ILaserReceivable laserReceivable)
                        laserReceivable.SendFocusInfo(laser, tuple.grabbable);
                    else
                        laser.SetSpriteAndText(SpriteManager.LaserIcon.Default);

                }),

                //ChangeState
                m_HandStateRP.Subscribe(state =>
                {
                    m_PlayerHand.EnableLaser(state == HandState.Laser);
                    if (state == HandState.Wisp)
                    {
                        traceCTS = new CancellationTokenSource();
                        wispHand.EnableTrace(m_PlayerHand.transform,traceCTS.Token);
                        grabber.transform.SetParent(wispHand.transform,false);
                        grabber.PlayerHandEnabled=false;
                    }
                    else
                    {
                        traceCTS?.Cancel();
                        grabber.transform.SetParent(playerHand.transform,false);
                        grabber.PlayerHandEnabled=true;
                    }
                    if(grabber.Target is BaseItem item)
                        item.SetOffsetPosition(grabber);
                }),

                //WispSpriteMove
                // grabber.TargetAsObservable.Subscribe(target=>{
                //     if(HandState==HandState.Wisp)
                //         wispHand.SetSpritePosition(target?.transform.localPosition??default);
                // }),
            });

            if (isMine)
            {
                var inputType = isLeft ? InputManager.InputType.LeftController : InputManager.InputType.RightController;
                var handInput = InputManager.CreateHandInput(inputType);

                //WispMode
                handInput.MainClick.AddListener(isDouble =>
                {
                    if (isDouble)
                        HandState = HandState != HandState.Wisp ? HandState.Wisp : HandState.Default;
                });
                //LaseMode
                handInput.SubClick.AddListener(isDouble =>
                {
                    if (isDouble)
                        HandState = HandState != HandState.Laser ? HandState.Laser : HandState.Default;
                });
                //Grab
                handInput.HandPress.AddListener(pressed =>
                {
                    if (pressed)
                    {
                        if (HandState != HandState.Laser)
                            grabber.SendGrabAction();
                        else
                            m_PlayerHand.Interact(grabber.Target as IInteractor, false);
                    }
                    else
                        grabber.Release();
                });
                handInput.IndexClick.AddListener(isDouble =>
                {
                    if (isDouble)
                    {
                        if (HandState == HandState.Laser)
                        {
                            m_PlayerHand.Interact(grabber.Target as IInteractor, isDouble);
                        }
                    }
                });

                InputManager.HandInput controlInput = default;
                //InputConnection
                m_Subscriptions.Add(grabber.TargetAsObservable.Subscribe(target =>
                {
                    controlInput?.Destroy();
                    var controllable = target as IControllable ?? this;
                    controlInput = InputManager.CreateHandInput(inputType);
                    controllable.Connect(controlInput);
                }));
            }
        }
        public void Connect(InputManager.HandInput input)
        {
            input.IndexTrigger.AddListener(value => m_PlayerHand.Animator.SetFloat("IndexValue", value));
            input.HandTrigger.AddListener(value => m_PlayerHand.Animator.SetFloat("HandValue", value));
        }
    }
    public enum HandState
    {
        Default,
        Wisp,
        Laser,
    }

}

[Serializable]
public struct PlayerIKAnchor
{
    [SerializeField] Transform m_Root, m_LookAnchor, m_EyeAnchor, m_LeftHandAnchor, m_RightHandAnchor;
    public PlayerIKAnchor(Transform root, Transform lookAnchor, Transform eyeAnchor, Transform leftHandAnchor, Transform rightHandAnchor)
    {
        m_Root = root;
        m_LookAnchor = lookAnchor;
        m_EyeAnchor = eyeAnchor;
        m_LeftHandAnchor = leftHandAnchor;
        m_RightHandAnchor = rightHandAnchor;
    }
    public Transform GetIKAnchor(AnchorBone ikBone)
    {
        switch (ikBone)
        {
            case AnchorBone.Root:
                return m_Root;
            case AnchorBone.LookTarget:
                return m_LookAnchor;
            case AnchorBone.Eye:
                return m_EyeAnchor;
            case AnchorBone.LeftHand:
                return m_LeftHandAnchor;
            case AnchorBone.RightHand:
                return m_RightHandAnchor;
        }
        return default;
    }
    public Transform[] ToArray() => new Transform[] { m_Root, m_LookAnchor, m_LeftHandAnchor, m_RightHandAnchor };

    public enum AnchorBone
    {
        Root,
        LookTarget,
        Eye,
        LeftHand,
        RightHand,
    }
}

[Serializable]
public struct Finger
{
    [Range(-2f, 2f)] public float Spread, Streached1, Streached2, Streached3;
    public Finger(float spread, float streached1, float streached2, float streached3)
    {
        Spread = spread;
        Streached1 = streached1;
        Streached2 = streached2;
        Streached3 = streached3;
    }
    public override string ToString()
    => $"({Spread}f,{Streached1}f,{Streached2}f,{Streached3}f)";
    public static Finger Lerp(Finger finger1, Finger finger2, float lerp)
    => new Finger(Mathf.Lerp(finger1.Spread, finger2.Spread, lerp), Mathf.Lerp(finger1.Streached1, finger2.Streached1, lerp), Mathf.Lerp(finger1.Streached2, finger2.Streached2, lerp), Mathf.Lerp(finger1.Streached3, finger2.Streached3, lerp));
}
[Serializable]
public struct HandShape
{
    [Range(-1f, 1f)] public float ThumbRotation;
    public Finger Thumb, Index, Middle, Ring, Little;
    public HandShape(Finger thumb, Finger index, Finger middle, Finger ring, Finger little, float thumbRot = 0f)
    {
        Thumb = thumb;
        Index = index;
        Middle = middle;
        Ring = ring;
        Little = little;
        ThumbRotation = thumbRot;
    }
    public static HandShape Lerp(HandShape hand1, HandShape hand2, float lerp)
    => new HandShape(Finger.Lerp(hand1.Thumb, hand2.Thumb, lerp), Finger.Lerp(hand1.Index, hand2.Index, lerp), Finger.Lerp(hand1.Middle, hand2.Middle, lerp), Finger.Lerp(hand1.Ring, hand2.Ring, lerp), Finger.Lerp(hand1.Little, hand2.Little, lerp));
}
[Serializable]
public class HandShapeReactiveProperty : ReactiveProperty<HandShape>
{
    public HandShapeReactiveProperty() { }
    public HandShapeReactiveProperty(HandShape handShape) : base(handShape) { }
}
[Serializable]
public struct HandleAnchor
{
    public Vector3 Position;
    public Quaternion Rotation;
}