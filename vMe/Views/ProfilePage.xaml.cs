﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using vMe.Services;
using System.Timers;

//Profile Page
namespace vMe.Views
{
    public partial class ProfilePage : ContentPage
    {
        private FluidKeeper fluidK = new FluidKeeper();
        private StepKeeper stepK = new StepKeeper();
        private EnergyKeeper energyK = new EnergyKeeper();
        private RobotState state = new RobotState();

        private static Timer timer;

        public ProfilePage()
        {
            InitializeComponent();
            Setup();
            FastStartTime();
            Update();
        }

        //Updates the whole ProfilePage
        public void Update()
        {
            var demo = false;

            int battery = energyK.RobotEnergy;
            int fluidCount = fluidK.FluidCount;
            int stepCount = stepK.RobotCounts;

            if (demo)
            {
                battery = 100;
                fluidCount = 100;
                stepCount = 10000;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
            fluid.Text = "Fluid: " + fluidCount.ToString() + "%";
            step.Text = "Steps: " + stepCount.ToString() + " out of 10000";
            energy.Text = "Energy: " + battery.ToString() + "%" ;

                robotSprite.Source = state.RobotSprite(state.ActivityState(battery, "power"), state.ActivityState(fluidCount, "fluid"), state.ActivityState(stepCount, "step"));

                if (state.ActivityState(fluidCount, "fluid") || state.ActivityState(battery, "power") || state.ActivityState(stepCount, "step"))
                {
                    RobotState.Text = "I am not happy";
                }
                else
                {
                    RobotState.Text = "I am feeling good";
                }
            });


        }

        //Updates the Ui
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

        //Sets the elements according to the display
        private void Setup()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = mainDisplayInfo.Width / mainDisplayInfo.Density;
            var height = mainDisplayInfo.Height / mainDisplayInfo.Density;

            var heightActive = (int)Math.Round(height);
            double heightSet1 = height * 0.55;
            double heightSet2 = height * 0.25;

            Console.WriteLine(heightSet1);

            //robotSprite.HeightRequest = heightSet1;
            //details.HeightRequest = heightSet2;
        }


    }
}
