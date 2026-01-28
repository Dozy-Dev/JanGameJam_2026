using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Simple color-based highlight for an interactable.
    /// Attach to the same GameObject as an InteractableBase or IInteractable.
    /// </summary>
    [DisallowMultipleComponent]
    public class InteractionHighlighter : MonoBehaviour
    {
        [Header("Highlight Settings")]
        [SerializeField] private Renderer[] targetRenderers;
        [SerializeField] private float intensityMultiplier = 1.5f;

        private readonly List<MaterialPropertyBlock> _blocks = new List<MaterialPropertyBlock>();
        private readonly List<Color[]> _originalColors = new List<Color[]>();
        private bool _initialized;

        private static readonly int ColorPropId_Color = Shader.PropertyToID("_Color");
        private static readonly int ColorPropId_BaseColor = Shader.PropertyToID("_BaseColor");


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            if (targetRenderers == null || targetRenderers.Length == 0)
            {
                targetRenderers = GetComponentsInChildren<Renderer>();
            }

            foreach (var rend in targetRenderers)
            {
                if (rend == null) continue;

                int matCount = rend.sharedMaterials.Length;
                var colors = new Color[matCount];
                var block = new MaterialPropertyBlock();

                for (int i = 0; i < matCount; i++)
                {
                    var mat = rend.sharedMaterials[i];
                    if (mat != null && mat.HasProperty(ColorPropId_Color))
                    {
                        colors[i] = mat.color;
                    }
                    else
                    {
                        colors[i] = Color.white;
                    }
                }

                _originalColors.Add(colors);
                _blocks.Add(block);
            }
        }

        public void SetHighlighted(bool highlighted)
        {
            Initialize();

            for (int r = 0; r < targetRenderers.Length; r++)
            {
                var rend = targetRenderers[r];
                if (rend == null) continue;

                var origColors = _originalColors[r];
                var block = _blocks[r];

                rend.GetPropertyBlock(block);

                for (int m = 0; m < origColors.Length; m++)
                {
                    Color c = origColors[m];
                    if (highlighted)
                        c *= intensityMultiplier;

                    // Try BaseColor first (URP/HDRP)
                    if (rend.sharedMaterials[m].HasProperty(ColorPropId_BaseColor))
                        block.SetColor(ColorPropId_BaseColor, c);

                    // Fallback to Color (Standard / legacy)
                    if (rend.sharedMaterials[m].HasProperty(ColorPropId_Color))
                        block.SetColor(ColorPropId_Color, c);
                }

                rend.SetPropertyBlock(block);
            }
        }

    }
}
