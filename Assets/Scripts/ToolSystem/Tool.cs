﻿using UnityEngine;

namespace ToolSystem
{
    [CreateAssetMenu]
    public class Tool : ScriptableObject
    {
        public enum ToolAction
        {
            Tap,
            Continuous
        }
        
        [Tooltip("Radius for the tool. This is in world radius not HexCell radius")]
        public float radius;
        [Tooltip("When action is set to Continuous, damage is dealt every frame.")]
        public ToolAction action;
        [Tooltip("Damage is per tap when action is set to Tap, and per second when action is set to Continuous.")]
        public float damage;
        [Tooltip("Whether the tool is unlocked or not")]
        public bool unlocked;
        // TODO: When a starting tool is selected, set all tools to false 
        [Tooltip("Start cleaning with this tool.")]
        public bool startingTool;
        public AnimationCurve damageFalloff;
        [Tooltip("Will only damage the artefact if not damaging rock.")]
        public bool artefactSafety;
        [Tooltip("Will only damage mines if not damaging rock.")]
        public bool mineSafety;
    }
}