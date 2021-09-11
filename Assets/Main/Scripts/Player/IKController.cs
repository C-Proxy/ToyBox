using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace MecanimIKPlus
{
    public class IKController : MonoBehaviour
    {
        [SerializeField] PlayerHand m_LeftHand = default, m_RightHand = default;
        [SerializeField]
        Transform
        m_LookTarget = default,
        m_EyeAnchor = default,
        m_BodyTarget = default,
        m_LeftFootTarget = default,
        m_RightFootTarget = default;
        [SerializeField]
        float
        m_LookAtWeight = 1.0f,
        m_HandPosWeight = 1.0f,
        m_HandRotWeight = 1.0f,
        m_FootPosWeight = 1.0f,
        m_FootRotWeight = 1.0f,
        m_ElbowWeight = 1.0f,
        m_KneeWeight = 1.0f;
        [SerializeField] bool m_SwitchHeadAxisXZ = true;
        Animator m_Animator;
        HumanPoseHandler m_HumanPoseHandler;
        HumanPose m_HumanPose;
        Transform m_HeadAnchor, m_NeckAnchor, m_LeftElbowAnchor, m_RightElbowAnchor, m_LeftKneeAnchor, m_RightKneeAnchor, m_LeftHandTarget, m_RightHandTarget, m_LeftThumbAnchor, m_RightThumbAnchor;
        HandShape m_LeftHandShape, m_RightHandShape;

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_HeadAnchor = m_Animator.GetBoneTransform(HumanBodyBones.Head);
            m_NeckAnchor = m_Animator.GetBoneTransform(HumanBodyBones.Neck);
            m_LeftElbowAnchor = m_Animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            m_RightElbowAnchor = m_Animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            m_LeftKneeAnchor = m_Animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            m_RightKneeAnchor = m_Animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            m_LeftThumbAnchor = m_Animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
            m_RightThumbAnchor = m_Animator.GetBoneTransform(HumanBodyBones.RightThumbProximal);
            m_LeftHandTarget = m_LeftHand.HandFollower.transform;
            m_RightHandTarget = m_RightHand.HandFollower.transform;

            m_HumanPoseHandler = new HumanPoseHandler(m_Animator.avatar, m_Animator.transform);

            var token = gameObject.GetCancellationTokenOnDestroy();
            HandShapeAsync(m_LeftHand, token);
            HandShapeAsync(m_RightHand, token);
        }

        void OnAnimatorIK()
        {
            m_Animator.SetLookAtWeight(m_LookAtWeight, 0.0f, 1.0f, 1.0f, 0.0f);
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, m_HandPosWeight);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, m_HandRotWeight);
            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, m_HandPosWeight);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, m_HandRotWeight);
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, m_FootPosWeight);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, m_FootRotWeight);
            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, m_FootPosWeight);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, m_FootRotWeight);
            m_Animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, m_ElbowWeight);
            m_Animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, m_ElbowWeight);

            if (m_LookTarget != null)
            {
                m_Animator.SetLookAtPosition(m_LookTarget.position);
            }
            if (m_BodyTarget != null)
            {
                m_Animator.bodyPosition = m_BodyTarget.position;
                m_Animator.bodyRotation = m_BodyTarget.rotation;
            }
            if (m_LeftHandTarget != null)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHandTarget.position);
                m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftHandTarget.rotation);
                m_Animator.SetIKHintPosition(AvatarIKHint.LeftElbow, m_LeftElbowAnchor.position);
            }
            if (m_RightHandTarget != null)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_RightHandTarget.position);
                m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_RightHandTarget.rotation);
                m_Animator.SetIKHintPosition(AvatarIKHint.RightElbow, m_RightElbowAnchor.position);
            }
            if (m_LeftFootTarget != null)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.LeftFoot, m_LeftFootTarget.position);
                m_Animator.SetIKRotation(AvatarIKGoal.LeftFoot, m_LeftFootTarget.rotation);
            }
            if (m_RightFootTarget != null)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.RightFoot, m_RightFootTarget.position);
                m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, m_RightFootTarget.rotation);
            }
        }
        void LateUpdate()
        {
            m_HumanPoseHandler.GetHumanPose(ref m_HumanPose);
            int id = 55;
            OverwriteHandPose(m_LeftHandShape, ref id);
            OverwriteHandPose(m_RightHandShape, ref id);
            m_HumanPoseHandler.SetHumanPose(ref m_HumanPose);
            OverwriteThumbRotate();
            ApplyHeadIK();
        }
        void HandShapeAsync(PlayerHand hand, CancellationToken token)
        {
            UniTask.Run(async () =>
            {
                await foreach (var handShape in hand.HandShapeAsyncEnumerable)
                {
                    if (hand.IsLeft)
                        m_LeftHandShape = handShape;
                    else
                        m_RightHandShape = handShape;
                }
            }, cancellationToken: token).Forget();
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
            m_LeftThumbAnchor.Rotate(Vector3.left, m_LeftHandShape.ThumbRotation * 90f, Space.Self);
            m_RightThumbAnchor.Rotate(Vector3.left, m_RightHandShape.ThumbRotation * 90f, Space.Self);
        }
        void OverwriteFingerPose(Finger finger, ref int id)
        {
            m_HumanPose.muscles[id++] = finger.Streached1;
            m_HumanPose.muscles[id++] = finger.Spread;
            m_HumanPose.muscles[id++] = finger.Streached2;
            m_HumanPose.muscles[id++] = finger.Streached3;
        }
        void ApplyHeadIK()
        {
            var headAng = m_HeadAnchor.eulerAngles;
            var neckAng = m_NeckAnchor.eulerAngles;
            float ang = Mathf.DeltaAngle(360.0f, m_EyeAnchor.eulerAngles.z);
            if (m_SwitchHeadAxisXZ)
            {
                headAng.x = ang;
                neckAng.x = ang * 0.5f;
            }
            else
            {
                headAng.z = ang;
                neckAng.z = ang * 0.5f;
            }
            m_HeadAnchor.eulerAngles = headAng;
            m_NeckAnchor.eulerAngles = neckAng;
        }
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