﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GameFramework;
using GameFramework.Screens;
using GameNavigator.Navigator;
using GameNavigator.ObjectInspector;
using GameNavigator.Properties;

namespace GameNavigator
{
    public class GameNavigatorService
    {
        private NavigatorViewModel navigatorViewModel;
        private Window navigatorWindow;

        private Window inspectorWindow;

        public bool IsNavigatorOpen
        {
            get { return this.navigatorWindow == null || this.navigatorWindow.IsVisible; }
        }

        public void Launch(IScreen gameScreen, GameResourceManager gameResourceManager, Action<int, int> moveGameWindow)
        {
            RestoreGameWindow(moveGameWindow);

            // Create a thread
            var newWindowThread = new Thread(() =>
            {
                // Navigator window
                this.navigatorViewModel = new NavigatorViewModel(gameScreen, gameResourceManager);
                var navigator = new NavigatorView { DataContext = navigatorViewModel };
                this.navigatorWindow = new Window { Content = navigator };

                this.RestoreNavigatorWindow();

                this.navigatorWindow.Closing += (sender, args) => { args.Cancel = true; this.navigatorWindow.Hide(); };
                
                this.navigatorWindow.SizeChanged += (sender, args) => PersistNavigatorWindow();
                this.navigatorWindow.LocationChanged += (sender, args) => PersistNavigatorWindow();

                //this.navigatorWindow.Show();

                // Object Inspector window
                var objectInspector = new ObjectInspectorView(navigatorViewModel);
                this.inspectorWindow = new Window { Content = objectInspector, Title = "Object Inspector" };

                this.RestoreInspectorWindow();

                this.inspectorWindow.Closing += (sender, args) => { args.Cancel = true; this.inspectorWindow.Hide(); };

                this.inspectorWindow.SizeChanged += (sender, args) => PersistInspectorWindow();
                this.inspectorWindow.LocationChanged += (sender, args) => PersistInspectorWindow();

                // Start the Dispatcher Processing
                Dispatcher.Run();
            });

            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();
        }

        public void Show()
        {
            if (this.navigatorWindow != null)
            {
                this.navigatorWindow.Dispatcher.Invoke(() => this.navigatorWindow.Show());
            }

            if (this.inspectorWindow != null)
            {
                this.inspectorWindow.Dispatcher.Invoke(() => this.inspectorWindow.Show());
            }
        }

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            if (this.navigatorWindow != null)
            {
                NavigatorMessage result = null;

                this.navigatorWindow.Dispatcher.Invoke(() =>
                {
                    result = this.navigatorViewModel.Update(gameTiming);
                    this.navigatorWindow.Title = "Navigator" + (result.ShouldPlay ? string.Empty : " - Pause");
                });

                return result;
            }

            return new NavigatorMessage();
        }

        public void PersistGameWindowPosition(int x, int y)
        {
            Settings.Default.GameWindowPosition = x + "," + y;
            Settings.Default.Save();
        }

        private static void RestoreGameWindow(Action<int, int> moveGameWindow)
        {
            try
            {
                var position = Settings.Default.GameWindowPosition;
                var values = position.Split(',');

                moveGameWindow.Invoke(int.Parse(values[0].Trim()), int.Parse(values[1].Trim()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void RestoreNavigatorWindow()
        {
            try
            {
                var position = Settings.Default.NavigatorPosition;
                var values = position.Split(',');

                this.navigatorWindow.Left = int.Parse(values[0].Trim());
                this.navigatorWindow.Top = int.Parse(values[1].Trim());
                this.navigatorWindow.Width = int.Parse(values[2].Trim());
                this.navigatorWindow.Height = int.Parse(values[3].Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PersistNavigatorWindow()
        {
            Settings.Default.NavigatorPosition = string.Join(",", new[]
            {
                this.navigatorWindow.Left,
                this.navigatorWindow.Top,
                this.navigatorWindow.Width,
                this.navigatorWindow.Height
            });

            Settings.Default.Save();
        }

        private void RestoreInspectorWindow()
        {
            try
            {
                var position = Settings.Default.InspectorPosition;
                var values = position.Split(',');

                this.inspectorWindow.Left = int.Parse(values[0].Trim());
                this.inspectorWindow.Top = int.Parse(values[1].Trim());
                this.inspectorWindow.Width = int.Parse(values[2].Trim());
                this.inspectorWindow.Height = int.Parse(values[3].Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PersistInspectorWindow()
        {
            Settings.Default.InspectorPosition = string.Join(",", new[]
            {
                this.inspectorWindow.Left,
                this.inspectorWindow.Top,
                this.inspectorWindow.Width,
                this.inspectorWindow.Height
            });

            Settings.Default.Save();
        }
    }
}