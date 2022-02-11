﻿using Managers;
using ToolSystem;
using UI.Cleaning;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cleaning
{
    [RequireComponent(typeof(Button))]
    public class SelectToolButton : DialogueComponent<CleaningDialogue>
    {
        [SerializeField] private Tool tool;
        [SerializeField] private Color activatedColor;
        
        private ToolManager toolManager;
        
        private Image image;
        private Button dialogueButton;

        protected override void OnComponentAwake()
        {
            TryGetComponent(out dialogueButton);
            
            if (!tool || !tool.unlocked) dialogueButton.interactable = false;
        }

        protected override void OnComponentStart()
        {
            base.OnComponentStart();
            
            toolManager = M.GetOrThrow<ToolManager>();
            image = GetComponent<Image>();

            // Selects tool if its the starting tool. If multiple, whatever gets first then others will be set false.
            if (!tool || !tool.startingTool) return;
            
            if (toolManager.CurrentTool == null)
                SelectTool();
            else
                tool.startingTool = false;
        }

        protected override void Subscribe()
        {
            dialogueButton.onClick.AddListener(OnSubmit);
        }

        protected override void Unsubscribe()
        {
            dialogueButton.onClick.RemoveListener(OnSubmit);
        }

        private void OnSubmit()
        {
            if (toolManager.CurrentTool == tool) return;
            
            SelectTool();
        }

        private void SelectTool()
        {
            toolManager.SelectTool(tool);
            image.color = activatedColor;
            Dialogue.DeselectToolButton(this);
        }

        public void DeselectButton() => image.color = Color.white;
    }
}