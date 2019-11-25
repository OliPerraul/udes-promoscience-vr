using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public enum TransitionCameraAnimation
    {
        Transition_In=1306404397,
        Transition_Out=1833136083,
        Idle=2081823275,
    }
    public interface ITransitionCameraAnimatorWrapper
    {
        float GetStateSpeed(TransitionCameraAnimation state);
        float GetClipLength(TransitionCameraAnimation state);
        void Play(TransitionCameraAnimation animation, float normalizedTime);
        void Play(TransitionCameraAnimation animation);
        float BaseLayerLayerWeight{set;}
    }
    public class TransitionCameraAnimatorWrapper : ITransitionCameraAnimatorWrapper
    {
        private Animator _animator;
        private Dictionary<TransitionCameraAnimation,float> _stateSpeedValues = new Dictionary<TransitionCameraAnimation,float>();
        private Dictionary<TransitionCameraAnimation,float> _clipLengthValuesValues = new Dictionary<TransitionCameraAnimation,float>();
        public void Play(TransitionCameraAnimation animation, float normalizedTime)
        {
            _animator.Play((int)animation, -1, normalizedTime);
        }
        public void Play(TransitionCameraAnimation animation)
        {
            _animator.Play((int)animation);
        }
        public float BaseLayerLayerWeight{set => _animator.SetLayerWeight(0,value);}
        public TransitionCameraAnimatorWrapper(Animator animator)
        {
            _animator = animator;
            _stateSpeedValues.Add(TransitionCameraAnimation.Transition_In,1);
            _clipLengthValuesValues.Add(TransitionCameraAnimation.Transition_In,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Transition.In"));
            _stateSpeedValues.Add(TransitionCameraAnimation.Transition_Out,1);
            _clipLengthValuesValues.Add(TransitionCameraAnimation.Transition_Out,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Transition.Out"));
            _stateSpeedValues.Add(TransitionCameraAnimation.Idle,1);
            _clipLengthValuesValues.Add(TransitionCameraAnimation.Idle,Cirrus.Animations.Utils.GetClipLength(_animator.runtimeAnimatorController,"Idle"));
        }
        public float GetStateSpeed(TransitionCameraAnimation state)
        {
            if(_stateSpeedValues.TryGetValue(state, out float res)) return res;
            return -1f;
        }
        public float GetClipLength(TransitionCameraAnimation state)
        {
            if(_clipLengthValuesValues.TryGetValue(state, out float res)) return res;
            return -1f;
        }
    }
}
