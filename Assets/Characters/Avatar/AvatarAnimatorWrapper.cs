using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Characters
{
    public enum AvatarAnimation
    {
        Idle=2081823275,
        Jumping=1137391654,
        Victory=-1090111034,
        Helping=-354287068,
    }
    public interface IAvatarAnimatorWrapper
    {
        float GetStateSpeed(AvatarAnimation state);
        float GetClipLength(AvatarAnimation state);
        void Play(AvatarAnimation animation, float normalizedTime);
        void Play(AvatarAnimation animation);
        float BaseLayerLayerWeight{set;}
    }
    public class AvatarAnimatorWrapper : IAvatarAnimatorWrapper
    {
        private Animator _animator;
        private Dictionary<AvatarAnimation,float> _stateSpeedValues = new Dictionary<AvatarAnimation,float>();
        private Dictionary<AvatarAnimation,float> _clipLengthValuesValues = new Dictionary<AvatarAnimation,float>();
        public void Play(AvatarAnimation animation, float normalizedTime)
        {
            _animator.Play((int)animation, -1, normalizedTime);
        }
        public void Play(AvatarAnimation animation)
        {
            _animator.Play((int)animation);
        }
        public float BaseLayerLayerWeight{set => _animator.SetLayerWeight(0,value);}
        public AvatarAnimatorWrapper(Animator animator)
        {
            _animator = animator;
            _stateSpeedValues.Add(AvatarAnimation.Idle,1);
            _clipLengthValuesValues.Add(AvatarAnimation.Idle,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Idle"));
            _stateSpeedValues.Add(AvatarAnimation.Jumping,1);
            _clipLengthValuesValues.Add(AvatarAnimation.Jumping,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Jumping"));
            _stateSpeedValues.Add(AvatarAnimation.Victory,1);
            _clipLengthValuesValues.Add(AvatarAnimation.Victory,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Victory"));
            _stateSpeedValues.Add(AvatarAnimation.Helping,1);
            _clipLengthValuesValues.Add(AvatarAnimation.Helping,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Helping"));
        }
        public float GetStateSpeed(AvatarAnimation state)
        {
            if(_stateSpeedValues.TryGetValue(state, out float res)) return res;
            return -1f;
        }
        public float GetClipLength(AvatarAnimation state)
        {
            if(_clipLengthValuesValues.TryGetValue(state, out float res)) return res;
            return -1f;
        }
    }
}
