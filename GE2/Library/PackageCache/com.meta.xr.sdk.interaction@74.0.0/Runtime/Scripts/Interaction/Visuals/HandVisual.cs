/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Oculus.Interaction.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction
{
    /// <summary>
    /// Renders the hand.
    /// </summary>
    public class HandVisual : MonoBehaviour, IHandVisual
    {
        /// <summary>
        /// The hand to render.
        /// </summary>
        [SerializeField, Interface(typeof(IHand))]
        private UnityEngine.Object _hand;
        public IHand Hand { get; private set; }

        [SerializeField]
        private bool _updateRootPose = true;

        [SerializeField]
        private bool _updateRootScale = true;

        [SerializeField]
        private bool _updateVisibility = true;

        /// <summary>
        /// Determines the appearance of the hand.
        /// </summary>
#if ISDK_OPENXR_HAND
        [HideInInspector]
#endif
        [SerializeField]
        private SkinnedMeshRenderer _skinnedMeshRenderer;

#if ISDK_OPENXR_HAND
        [HideInInspector]
#endif
        [SerializeField, Optional]
        private Transform _root = null;

#if ISDK_OPENXR_HAND
        [HideInInspector]
#endif
        [SerializeField, Optional]
        private MaterialPropertyBlockEditor _handMaterialPropertyBlockEditor;

        [HideInInspector]
        [SerializeField]
        private List<Transform> _jointTransforms = new List<Transform>();

#if !ISDK_OPENXR_HAND
        [HideInInspector]
#endif
        [SerializeField]
        private SkinnedMeshRenderer _openXRSkinnedMeshRenderer;

#if !ISDK_OPENXR_HAND
        [HideInInspector]
#endif
        [SerializeField, Optional]
        private Transform _openXRRoot = null;

#if !ISDK_OPENXR_HAND
        [HideInInspector]
#endif
        [SerializeField, Optional]
        private MaterialPropertyBlockEditor _openXRHandMaterialPropertyBlockEditor;

        [HideInInspector]
        [SerializeField]
        private List<Transform> _openXRJointTransforms = new List<Transform>();

        public event Action WhenHandVisualUpdated = delegate { };

        public bool IsVisible => SkinnedMeshRenderer != null && SkinnedMeshRenderer.enabled;

        private int _wristScalePropertyId;

        public IList<Transform> Joints
        {
#if ISDK_OPENXR_HAND
            get => _openXRJointTransforms;
#else
            get => _jointTransforms;
#endif
        }

        public Transform Root
        {
#if ISDK_OPENXR_HAND
            get => _openXRRoot;
            private set => _openXRRoot = value;
#else
            get => _root;
            private set => _root = value;
#endif
        }

        private SkinnedMeshRenderer SkinnedMeshRenderer
        {
#if ISDK_OPENXR_HAND
            get => _openXRSkinnedMeshRenderer;
            set => _openXRSkinnedMeshRenderer = value;
#else
            get => _skinnedMeshRenderer;
            set => _skinnedMeshRenderer = value;
#endif
        }

        private MaterialPropertyBlockEditor HandMaterialPropertyBlockEditor
        {
#if ISDK_OPENXR_HAND
            get => _openXRHandMaterialPropertyBlockEditor;
            set => _openXRHandMaterialPropertyBlockEditor = value;
#else
            get => _handMaterialPropertyBlockEditor;
            set => _handMaterialPropertyBlockEditor = value;
#endif
        }

        private bool _forceOffVisibility;
        public bool ForceOffVisibility
        {
            get
            {
                return _forceOffVisibility;
            }
            set
            {
                _forceOffVisibility = value;
                if (_started)
                {
                    UpdateVisibility();
                }
            }
        }

        private bool _started = false;

        protected virtual void Awake()
        {
            Hand = _hand as IHand;
            if (Root == null && Joints.Count > 0 && Joints[0] != null)
            {
                Root = Joints[0].parent;
            }
#if ISDK_OPENXR_HAND
            if (_root != null)
            {
                _root.gameObject.SetActive(false);
            }
            if (_openXRRoot != null)
            {
                _openXRRoot.gameObject.SetActive(true);
            }

#else
            if (_root != null)
            {
                _root.gameObject.SetActive(true);
            }
            if (_openXRRoot != null)
            {
                _openXRRoot.gameObject.SetActive(false);
            }
#endif
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(Hand, nameof(Hand));
            this.AssertField(SkinnedMeshRenderer, nameof(SkinnedMeshRenderer));
            if (HandMaterialPropertyBlockEditor != null)
            {
                _wristScalePropertyId = Shader.PropertyToID("_WristScale");
            }
            UpdateVisibility();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                Hand.WhenHandUpdated += UpdateSkeleton;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started && _hand != null)
            {
                Hand.WhenHandUpdated -= UpdateSkeleton;
            }
        }

        private void UpdateVisibility()
        {
            if (!_updateVisibility)
            {
                return;
            }

            if (!Hand.IsTrackedDataValid)
            {
                if (IsVisible || ForceOffVisibility)
                {
                    SkinnedMeshRenderer.enabled = false;
                }
            }
            else
            {
                if (!IsVisible && !ForceOffVisibility)
                {
                    SkinnedMeshRenderer.enabled = true;
                }
                else if (IsVisible && ForceOffVisibility)
                {
                    SkinnedMeshRenderer.enabled = false;
                }
            }
        }

        public void UpdateSkeleton()
        {
            UpdateVisibility();
            if (!Hand.IsTrackedDataValid)
            {
                WhenHandVisualUpdated.Invoke();
                return;
            }

            if (_updateRootPose)
            {
                if (Root != null && Hand.GetRootPose(out Pose handRootPose))
                {
                    Root.position = handRootPose.position;
                    Root.rotation = handRootPose.rotation;
                }
            }

            if (_updateRootScale)
            {
                if (Root != null)
                {
                    float parentScale = Root.parent != null ? Root.parent.lossyScale.x : 1f;
                    Root.localScale = Hand.Scale / parentScale * Vector3.one;
                }
            }

            if (!Hand.GetJointPosesLocal(out ReadOnlyHandJointPoses localJoints))
            {
                return;
            }
            for (var i = 0; i < Constants.NUM_HAND_JOINTS; ++i)
            {
                if (Joints[i] == null)
                {
                    continue;
                }
                Joints[i].SetPose(localJoints[i], Space.Self);
            }

            if (HandMaterialPropertyBlockEditor != null)
            {
                HandMaterialPropertyBlockEditor.MaterialPropertyBlock.SetFloat(_wristScalePropertyId, Hand.Scale);
                HandMaterialPropertyBlockEditor.UpdateMaterialPropertyBlock();
            }
            WhenHandVisualUpdated.Invoke();
        }

        public Transform GetTransformByHandJointId(HandJointId handJointId)
        {
            return Joints[(int)handJointId];
        }

        public Pose GetJointPose(HandJointId jointId, Space space)
        {
            return GetTransformByHandJointId(jointId).GetPose(space);
        }

        #region Inject

        public void InjectAllHandSkeletonVisual(IHand hand, SkinnedMeshRenderer skinnedMeshRenderer)
        {
            InjectHand(hand);
            InjectSkinnedMeshRenderer(skinnedMeshRenderer);
        }

        public void InjectHand(IHand hand)
        {
            _hand = hand as UnityEngine.Object;
            Hand = hand;
        }

        public void InjectSkinnedMeshRenderer(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            SkinnedMeshRenderer = skinnedMeshRenderer;
        }

        public void InjectOptionalUpdateRootPose(bool updateRootPose)
        {
            _updateRootPose = updateRootPose;
        }

        public void InjectOptionalUpdateRootScale(bool updateRootScale)
        {
            _updateRootScale = updateRootScale;
        }

        public void InjectOptionalRoot(Transform root)
        {
            Root = root;
        }

        public void InjectOptionalMaterialPropertyBlockEditor(MaterialPropertyBlockEditor editor)
        {
            HandMaterialPropertyBlockEditor = editor;
        }

        #endregion
    }
}
