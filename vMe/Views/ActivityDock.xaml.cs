﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using vMe.Views;
using vMe.Services;
using System.Timers;
using Xamarin.Essentials;

namespace vMe.Views
{
    [DesignTimeVisible(false)]
    public partial class ActivityDock : ContentView
    {
        //Update
        private FluidKeeper fluid = new FluidKeeper();
        private StepKeeper steps  = new StepKeeper();
        private EnergyKeeper energy = new EnergyKeeper();
        private RobotState state = new RobotState();

        private ProfilePage profile = new ProfilePage();

        private static Timer timer;




        //Setup variable
        double widthActive = 0;
        double heightActive = 0;
        double width3 = 0;


        //Menu config
        bool waterTappedState = false;
        bool runningTappedState = false;
        bool batteryTappedState = false;

        public ActivityDock()
        {            
            InitializeComponent();
            Setup();
            fluid.getFluid();
            energy.getEnergy();
            UiUpdate();
        }

        public void Setup()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = mainDisplayInfo.Width / mainDisplayInfo.Density;
            var height = mainDisplayInfo.Height / mainDisplayInfo.Density;

            dockContainer.WidthRequest = width;
            widthActive = (int)Math.Round(width);
            width3 = width / 3;
            width3 -= 15;
            waterLevel.WidthRequest = width3;
            runningLevel.WidthRequest = width3;
            batteryLevel.WidthRequest = width3;

            width3 -= 15;
            waterDropPic.WidthRequest = width3;
            runningManPic.WidthRequest = width3;
            batteryPic.WidthRequest = width3;

            heightActive = (int)Math.Round(height);
            var h = height * 0.25;
            Console.WriteLine(heightActive);
            Console.WriteLine(h);

            dockContainer.HeightRequest = h;
            waterLevel.HeightRequest = h;
            runningLevel.HeightRequest = h;
            batteryLevel.HeightRequest = h;
            FastStartTime();
        }

        public void UiUpdate()
        {
            
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                //Battery/Energy Ui Update
                int battery = energy.RobotEnergy;
                string sBatteryCount = battery.ToString();
                
                if (battery >= 5)
                {
                    batteryLabel.Text = "Battery is at "+sBatteryCount+"% you need to recharge now!";
                }
                else
                {                   
                    batteryLabel.Text = "Battery is at " + sBatteryCount + "%";
                }

                batteryPic.Source = "battery"+state.IconState(battery, "null");

                batteryPic.Margin = new Thickness(12, 40, 0, 20);
                batteryPic.WidthRequest = -1;

                //Steps Ui Update
                int stepCount = steps.RobotCounts;
                string stepCounts = stepCount.ToString();
                Console.WriteLine("Steps taken " + stepCounts);
                                
                if (stepCount >= 10000)
                {                    
                    runningLabel.Text = "Well done! You have taken " + stepCounts + " out of 10000 steps today";
                }
                else
                {
                    runningLabel.Text = "You have taken " + stepCounts + " out of 10000 steps today";
                }
                runningManPic.Source = "runningMan" + state.IconState(stepCount, "step");

                //Fluid Ui Update
                int fluidCount = fluid.FluidCount;
                string sFluidCount = fluidCount.ToString();
                waterLabel.Text = "You have drink " + sFluidCount + "% of your daily intake";
                if (fluidCount >= 5)
                {
                    waterDropPic.Source = "waterDrop";
                }
                if (fluidCount >= 10)
                {
                    waterLabel.Text = "You have drink " + sFluidCount + "% of your daily intake. I feel a bit thirsty at this stage.";
                }
                if (fluidCount >= 50)
                {
                    waterLabel.Text = "You have drink " + sFluidCount + "% of your daily intake";
                }
                if (fluidCount >= 80)
                {
                    waterLabel.Text = "You have drink " + sFluidCount + "% of your daily intake. Almost there!";
                }
                if (fluidCount >= 100)
                {
                    waterLabel.Text = "You have drink " + sFluidCount + "% of your daily intake. You have reached your goal!";
                }
                if (fluidCount >= 110)
                {
                    
                    waterLabel.Text = "You have drink " + sFluidCount + "% of your daily intake. I think that may be enough for now.";
                }

                waterDropPic.Source = "water" + state.IconState(fluidCount, "null");

                waterDropPic.Margin = new Thickness(12, 40, 0, 20);
                waterDropPic.WidthRequest = -1;



            });
        }       

               
        void waterTapped(System.Object sender, System.EventArgs e)
        {
            UiUpdate();
            Console.WriteLine("Tapped");
            if (waterTappedState)
            {
                Console.WriteLine("true");
                waterLevel.WidthRequest = width3;
                waterTappedState = false;

                waterLevel.IsVisible = true;
                runningLevel.IsVisible = true;
                batteryLevel.IsVisible = true;

                waterLevel.LowerChild(waterDropPic);
                waterLabel.IsVisible = false;
                waterButton.IsVisible = false;
            }
            else if (!waterTappedState)
            {
                Console.WriteLine("false");
                waterTappedState = true;

                waterLevel.IsVisible = true;
                runningLevel.IsVisible = false;
                batteryLevel.IsVisible = false;
                waterLevel.WidthRequest = widthActive - 20;

                waterLevel.RaiseChild(waterDropPic);
                waterLabel.IsVisible = true;
                waterButton.IsVisible = true;



            }
        }

        void runningTapped(System.Object sender, System.EventArgs e)
        {
            UiUpdate();
            Console.WriteLine("Tapped");
            if (runningTappedState)
            {
                Console.WriteLine("true");
                runningLevel.WidthRequest = width3;
                runningTappedState = false;

                waterLevel.IsVisible = true;
                runningLevel.IsVisible = true;
                batteryLevel.IsVisible = true;

                runningLevel.LowerChild(waterDropPic);
                runningLabel.IsVisible = false;
            }
            else if (!runningTappedState)
            {
                Console.WriteLine("false");
                runningTappedState = true;

                waterLevel.IsVisible = false;
                runningLevel.IsVisible = true;
                batteryLevel.IsVisible = false;
                runningLevel.WidthRequest = widthActive - 20;

                runningLevel.RaiseChild(waterDropPic);
                runningLabel.IsVisible = true;

            }
        }

        void batteryTapped(System.Object sender, System.EventArgs e)
        {
            UiUpdate();
            Console.WriteLine("Tapped");
            if (batteryTappedState)
            {
                Console.WriteLine("true");
                batteryLevel.WidthRequest = width3;
                batteryTappedState = false;

                waterLevel.IsVisible = true;
                runningLevel.IsVisible = true;

                batteryLevel.LowerChild(waterDropPic);
                batteryLabel.IsVisible = false;

            }
            else if (!batteryTappedState)
            {
                Console.WriteLine("false");
                batteryTappedState = true;

                waterLevel.IsVisible = false;
                runningLevel.IsVisible = false;
                batteryLevel.IsVisible = true;
                batteryLevel.WidthRequest = widthActive - 20;

                batteryLevel.RaiseChild(waterDropPic);
                batteryLabel.IsVisible = true;
            }
        }

        //Action Buttons

        void drinkWaterTapped(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine(fluid.FluidCount);
            fluid.FluidCount += 10;
            Console.WriteLine(fluid.FluidCount);
            UiUpdate();
            
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
            UiUpdate();

        }
    }
}
