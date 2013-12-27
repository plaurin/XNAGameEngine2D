﻿using System;

using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    public abstract class ScreenBase
    {
        public abstract void Initialize(Camera camera);

        public abstract InputConfiguration GetInputConfiguration();

        public abstract void LoadContent(GameResourceManager resourceManager);

        public abstract void Update(IGameTiming gameTime);

        public abstract Scene GetScene();
    }
}