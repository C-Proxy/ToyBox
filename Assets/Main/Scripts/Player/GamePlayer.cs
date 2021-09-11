using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Prefab;

public class GamePlayer : GrabberBehaviour, INetworkHandler
{
    [SerializeField] PlayerIKAnchor m_PlayerIKAnchor = default;
    [SerializeField] Animator m_PlayerAnimator = default;
    [SerializeField] PlayerHand m_LeftHand = default, m_RightHand = default;
    // PoseController m_PoseController;
    HandController m_LeftHandController, m_RightHandController;

    CanvasCube m_CanvasCube;

    [ServerRpc]
    void ToggleCanvasServerRpc()
    {
        if (m_CanvasCube)
        {
            if (m_CanvasCube.IsLerping) return;

            UniTask.Run(async () =>
            {
                await UniTask.SwitchToMainThread();
                await m_CanvasCube.CloseAsync();
                m_CanvasCube = null;
            }, false, m_AliveCTS.Token);
        }
        else
        {
            var eyeAnchor = m_PlayerIKAnchor.EyeAnchor;
            m_CanvasCube = PrefabGenerator.SpawnPrefabOnServer(NetworkPrefabName.CanvasCube, eyeAnchor.position + eyeAnchor.forward, Quaternion.LookRotation(eyeAnchor.forward)).GetComponent<CanvasCube>();
        }

    }

    public NetworkBehaviour FindNetworkBehaviour(ushort behaviourId)
    => GetNetworkBehaviour(behaviourId);

    public IGrabber GetGrabber(int index) => m_Grabbers[index];
    private void Start()
    {
        m_LeftHandController = new HandController(m_LeftHand);
        m_RightHandController = new HandController(m_RightHand);
        // m_PoseController = new PoseController(m_PlayerAnimator, m_PlayerIKAnchor, m_LeftHand, m_RightHand);
        if (IsOwner)
        {
            OVRRigTraceAsync(m_AliveCTS.Token).Forget();
            m_LeftHandController.SetControl(InputManager.CreateHandInput(InputManager.InputType.LeftController), ToggleCanvasServerRpc);
            m_RightHandController.SetControl(InputManager.CreateHandInput(InputManager.InputType.RightController), ToggleCanvasServerRpc);
        }
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_LeftHand.OnSpawn();
        m_RightHand.OnSpawn();

    }
    override public void OnPool()
    {
        m_LeftHand.OnPool();
        m_RightHand.OnPool();
        m_LeftHandController = null;
        m_RightHandController = null;
        // m_PoseController = null;
        base.OnPool();
    }
    // private void OnAnimatorIK()
    // {
    //     m_PoseController.ApplyIK();
    // }
    async UniTaskVoid OVRRigTraceAsync(CancellationToken token)
    {
        var rootAnchor = m_PlayerIKAnchor.Root;
        var lookAnchor = m_PlayerIKAnchor.LookAnchor;
        var eyeAnchor = m_PlayerIKAnchor.EyeAnchor;
        var leftHandAnchor = m_PlayerIKAnchor.LeftHand;
        var rightHandAnchor = m_PlayerIKAnchor.RightHand;

        var ovrIKAnchor = OVRRigHandler.PlayerIKAnchor;
        var ovrRoot = ovrIKAnchor.Root;
        var ovrLook = ovrIKAnchor.LookAnchor;
        var ovrEye = ovrIKAnchor.EyeAnchor;
        var ovrLeft = ovrIKAnchor.LeftHand;
        var ovrRight = ovrIKAnchor.RightHand;
        try
        {
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
        catch { }
    }
    // class PoseController
    // {
    //     bool IsActive = true;
    //     Animator m_Animator;
    //     HandShape m_LeftHandShape = default, m_RightHandShape = default;
    //     Transform m_LeftThumbTransform, m_RightThumbTransform, m_BodyTarget, m_LeftTarget, m_RightTarget, m_LookTarget, m_LeftElbow, m_RightElbow;
    //     HumanPoseHandler m_HumanPoseHandler;
    //     HandController m_LeftHandController, m_RightHandController;
    //     HumanPose m_HumanPose;

    //     public PoseController(Animator animator, PlayerIKAnchor iKAnchor, PlayerHand leftHand, PlayerHand rightHand)
    //     {
    //         m_Animator = animator;
    //         var thumb = FingerName.Thumb;
    //         m_LeftThumbTransform = leftHand.HandTransform.GetFingerTransform(thumb, 0);
    //         m_RightThumbTransform = rightHand.HandTransform.GetFingerTransform(thumb, 0);

    //         m_BodyTarget = iKAnchor.BodyAnchor;
    //         m_LeftTarget = leftHand.HandFollower.transform;
    //         m_RightTarget = rightHand.HandFollower.transform;
    //         m_LookTarget = iKAnchor.LookAnchor;
    //         m_LeftElbow = iKAnchor.LeftElbow;
    //         m_RightElbow = iKAnchor.RightElbow;

    //         m_HumanPoseHandler = new HumanPoseHandler(animator.avatar, animator.transform);

    //         leftHand.HandShapeAsObservable.Subscribe(shape => m_LeftHandShape = shape).AddTo(leftHand);
    //         rightHand.HandShapeAsObservable.Subscribe(shape => m_RightHandShape = shape).AddTo(rightHand);
    //     }


    //     // private void LateUpdate()
    //     // {
    //     //     if (!IsActive) return;
    //     //     m_HumanPoseHandler.GetHumanPose(ref m_HumanPose);
    //     //     int id = 55;
    //     //     OverwriteHandPose(m_LeftHandShape, ref id);
    //     //     OverwriteHandPose(m_RightHandShape, ref id);
    //     //     m_HumanPoseHandler.SetHumanPose(ref m_HumanPose);
    //     //     OverwriteThumbRotate();
    //     // }
    //     // public void ApplyIK()
    //     // {
    //     //     if (!IsActive) return;
    //     //     m_Animator.SetLookAtWeight(1, 0.2f, 0.9f, 0.7f);
    //     //     m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    //     //     m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //     //     m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    //     //     m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    //     //     m_Animator.bodyPosition = m_BodyTarget.position;
    //     //     m_Animator.bodyRotation = m_BodyTarget.rotation;
    //     //     m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftTarget.position);
    //     //     m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_RightTarget.position);
    //     //     m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftTarget.rotation);
    //     //     m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_RightTarget.rotation);
    //     //     m_Animator.SetLookAtPosition(m_LookTarget.position);
    //     //     m_Animator.SetIKHintPosition(AvatarIKHint.LeftElbow, m_LeftElbow.position);
    //     //     m_Animator.SetIKHintPosition(AvatarIKHint.RightElbow, m_RightElbow.position);

    //     // }
    // }
    class HandController : IControllable
    {
        PlayerHand m_PlayerHand;
        ReactiveProperty<HandState> m_HandStateRP = new ReactiveProperty<HandState>();
        public HandState HandState { set { m_HandStateRP.Value = value; } get { return m_HandStateRP.Value; } }
        public IObservable<HandShape> HandShapeAsObservable { private set; get; }
        List<IDisposable> m_Subscriptions;

        public HandController(PlayerHand playerHand)
        {
            var laser = playerHand.LaserTargetFinder;

            m_PlayerHand = playerHand;
            var wispHand = m_PlayerHand.WispHand;
            var grabber = playerHand.HandGrabber;

            // HandShapeAsObservable = grabber.TargetAsObservable.Select(target =>
            // {
            //     if (target != null)
            //         return target.HandShapeAsObservable;
            //     else
            //         return m_PlayerHand.HandShapeAsObservable;
            // }).Switch().Publish().RefCount();

            CancellationTokenSource traceCTS = default;
            m_Subscriptions = new List<IDisposable>(new[]{

                //LaserFocus
                Observable.CombineLatest(laser.TargetAsObservable, grabber.TargetAsObservable, (target, grabbable) => (target, grabbable)).Subscribe(tuple =>
                {
                    var target = tuple.target;
                    if (target is IEventReceivable<FocusEvent> focusTarget)
                        focusTarget.SendEvent(new FocusEvent(laser,tuple.grabbable));
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
                        grabber.transform.SetParent(playerHand.HandFollower.transform,false);
                        grabber.PlayerHandEnabled=true;
                    }
                    if(grabber.Target is BaseItem item)
                        item.SetOffsetPosition(grabber);
                }),

            });


        }
        public void SetControl(InputManager.HandInput input, UnityAction canvasAction)
        {
            var grabber = m_PlayerHand.HandGrabber;

            //WispMode
            input.MainClick.AddListener(isDouble =>
            {
                if (isDouble)
                    HandState = HandState != HandState.Wisp ? HandState.Wisp : HandState.Default;
            });
            //LaserMode
            input.SubClick.AddListener(isDouble =>
            {
                if (isDouble)
                    HandState = HandState != HandState.Laser ? HandState.Laser : HandState.Default;
            });
            //Grab
            input.HandPress.AddListener(pressed =>
            {
                if (pressed)
                {
                    if (HandState != HandState.Laser)
                        grabber.SendGrabEvent();
                    else
                        m_PlayerHand.Interact();
                }
                else
                    grabber.Release();
            });
            input.IndexPress.AddListener(pressed =>
            {
                if (pressed)
                {
                    if (HandState == HandState.Laser)
                    {
                        m_PlayerHand.Interact();
                    }
                }
            });
            var cameraAnchor = Camera.main.transform;
            input.StartPress.AddListener(pressed =>
            {
                if (pressed)
                    canvasAction();
            });
            var inputType = m_PlayerHand.IsLeft ? InputManager.InputType.LeftController : InputManager.InputType.RightController;
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
    [SerializeField] Transform m_Root, m_LookAnchor, m_EyeAnchor, m_BodyAnchor, m_LeftHand, m_RightHand, m_LeftElbow, m_RightElbow, m_LeftKnee, m_RightKnee;
    public Transform Root => m_Root;
    public Transform LookAnchor => m_LookAnchor;
    public Transform EyeAnchor => m_EyeAnchor;
    public Transform BodyAnchor => m_BodyAnchor;
    public Transform LeftHand => m_LeftHand;
    public Transform RightHand => m_RightHand;
    public Transform LeftElbow => m_LeftElbow;
    public Transform RightElbow => m_RightElbow;
    public Transform LeftKnee => m_LeftKnee;
    public Transform RightKnee => m_RightKnee;
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