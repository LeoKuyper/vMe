﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using vMe.Models;
using vMe.Services;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace vMe.Views
{
    public partial class RobotPage : ContentPage
    {
        private static Timer timer;
        private FluidKeeper fluid = new FluidKeeper();
        private EnergyKeeper energy = new EnergyKeeper();

        //Setup variable
        double widthActive = 0;
        double heightActive = 0;

        public RobotPage()
        {
            InitializeComponent();
            Setup();
            Update();
            Startup();
            Console.WriteLine("RobotState Start");
        }

        private void Setup()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = mainDisplayInfo.Width / mainDisplayInfo.Density;
            var height = mainDisplayInfo.Height / mainDisplayInfo.Density;

            heightActive = (int)Math.Round(height);
            var h = height * 0.55;
            double heightSet = h;

            Console.WriteLine(heightSet);

            robotSprite.HeightRequest = heightSet;
        }

        public void Update()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                int battery = energy.RobotEnergy;
                int fluidCount = fluid.FluidCount;
                bool lowPower = false;
                bool lowFluid = false;


                if (battery <= 50)
                {
                    lowPower = true;
                    robotSprite.Source = "lowPower_robot";

                }


                if (fluidCount <= 50)
                {
                    lowFluid = true;
                    robotSprite.Source = "lowWater_robot";
                }



                if (lowFluid && lowPower)
                {
                    robotSprite.Source = "sad_robot";
                }else if (!lowFluid && !lowPower)
                {
                    robotSprite.Source = "happy_robot";
                }

            });

            
        }

        public void Startup()
        {
            HoverRobot();
            StartTime();
            FastStartTime();
        }        

        public async void HoverRobot()
        {
            await robotSprite.TranslateTo(0, 10, 500);
            await robotSprite.TranslateTo(0, 0, 1000);
            await robotSprite.TranslateTo(0, -10, 500);
            await robotSprite.TranslateTo(0, 0, 1000);
        }
        private void FastStartTime()
        {
            timer = new Timer();

            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += updateFast;
            timer.Start();

        }
        private void updateFast(object sender, ElapsedEventArgs e)
        {
            Update();
        }


        private void StartTime()
        {
            timer = new Timer();

            timer.Interval = 5000;
            timer.Enabled = true;
            timer.Elapsed += updateTimedData;
            timer.Start();

        }
        private void updateTimedData(object sender, ElapsedEventArgs e)
        {
            HoverRobot();
        }

    }
}
