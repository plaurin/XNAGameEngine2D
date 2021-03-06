﻿using System;
using System.Collections.Generic;
using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public interface IInputMapper
    {
        IDigitalButtonMapper GetDigitalButton(string name);

        VisualButton GetVisualButton(string name);

        InputEvent GetEvent(string name);

        IKeyboardMapper CreateKeyboardTracking();

        IMouseMapper CreateMouseTracking(ICamera camera);

        ITouchMapper CreateTouchTracking(ICamera camera);
    }

    public class InputConfiguration : IInputMapper
    {
        private readonly Dictionary<string, DigitalButton> digitalButtons;
        private readonly Dictionary<string, VisualButton> visualButtons;
        private readonly Dictionary<string, InputEvent> inputEvents;
        private readonly List<KeyboardTracking> keyboardTrackings;
        private readonly List<MouseTracking> mouseTrackings;
        private readonly List<TouchTracking> touchTrackings;

        public InputConfiguration()
        {
            this.digitalButtons = new Dictionary<string, DigitalButton>();
            this.visualButtons = new Dictionary<string, VisualButton>();
            this.inputEvents = new Dictionary<string, InputEvent>();
            this.keyboardTrackings = new List<KeyboardTracking>();
            this.mouseTrackings = new List<MouseTracking>();
            this.touchTrackings = new List<TouchTracking>();
        }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            var keyState = inputContext.KeyboardGetState();
            var mouseState = inputContext.MouseGetState();
            var touchState = inputContext.TouchGetState();

            foreach (var keyboardTracking in this.keyboardTrackings)
            {
                keyboardTracking.Update(keyState, gameTime);
            }

            foreach (var mouseTracking in this.mouseTrackings)
            {
                mouseTracking.Update(mouseState, gameTime);
            }

            foreach (var digitalButton in this.digitalButtons.Values)
            {
                digitalButton.Update(keyState, mouseState, gameTime);
            }

            foreach (var visualButton in this.visualButtons.Values)
            {
                visualButton.Update(touchState, mouseState, gameTime);
            }

            foreach (var touchTracking in this.touchTrackings)
            {
                touchTracking.Update(touchState, gameTime);
            }

            foreach (var inputEvent in this.inputEvents.Values)
            {
                inputEvent.Update(touchState, gameTime);
            }
        }

        public DigitalButton AddDigitalButton(string name)
        {
            var digitalButton = new DigitalButton();
            this.digitalButtons.Add(name, digitalButton);

            return digitalButton;
        }

        public IDigitalButtonMapper GetDigitalButton(string name)
        {
            return this.digitalButtons[name];
        }

        public VisualButton AddVisualButton(string name, Rectangle rectangle)
        {
            var visualButton = new VisualButton(rectangle);
            this.visualButtons.Add(name, visualButton);

            return visualButton;
        }

        public VisualButton GetVisualButton(string name)
        {
            return this.visualButtons[name];
        }

        public InputEvent AddEvent(string name)
        {
            var inputEvent = new InputEvent();
            this.inputEvents.Add(name, inputEvent);

            return inputEvent;
        }

        public InputEvent GetEvent(string name)
        {
            return this.inputEvents[name];
        }

        public IKeyboardMapper CreateKeyboardTracking()
        {
            var keyboardTracking = new KeyboardTracking();
            this.keyboardTrackings.Add(keyboardTracking);

            return keyboardTracking;
        }

        public IMouseMapper CreateMouseTracking(ICamera camera)
        {
            var mouseTracking = new MouseTracking(camera);
            this.mouseTrackings.Add(mouseTracking);

            return mouseTracking;
        }

        public ITouchMapper CreateTouchTracking(ICamera camera)
        {
            var touchTracking = new TouchTracking(camera);
            this.touchTrackings.Add(touchTracking);

            return touchTracking;
        }
    }
}
