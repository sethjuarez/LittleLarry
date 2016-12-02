using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using GIS = GHIElectronics.UWP.Shields;
using numl.Model;
using Windows.Gaming.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LittleLarry
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GIS.FEZHAT hat;
        private DispatcherTimer timer;
        private Gamepad gamepad;

        public MainPage()
        {
            InitializeComponent();
            gamepad = null;
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            Setup();
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            if (Gamepad.Gamepads.Count < 1)
                gamepad = null;
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            if (Gamepad.Gamepads.Count == 1)
                gamepad = Gamepad.Gamepads[0];
        }

        private async void Setup()
        {
            hat = await GIS.FEZHAT.CreateAsync();
            if (Gamepad.Gamepads.Count == 1)
                gamepad = Gamepad.Gamepads[0];


            hat.D2.Color = GIS.FEZHAT.Color.Black;
            hat.D3.Color = GIS.FEZHAT.Color.Black;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += OnTick;
            timer.Start();

        }

        private void OnTick(object sender, object e)
        {
            double x, y, z;

            hat.GetAcceleration(out x, out y, out z);

            LightTextBox.Text = hat.GetLightLevel().ToString("P2");
            TempTextBox.Text = hat.GetTemperature().ToString("N2");
            AccelTextBox.Text = $"({x:N2}, {y:N2}, {z:N2})";
            Button18TextBox.Text = hat.IsDIO18Pressed().ToString();
            Button22TextBox.Text = hat.IsDIO22Pressed().ToString();

            var ain1 = hat.ReadAnalog(GIS.FEZHAT.AnalogPin.Ain1);

            var ain2 = hat.ReadAnalog(GIS.FEZHAT.AnalogPin.Ain2);
            var ain3 = hat.ReadAnalog(GIS.FEZHAT.AnalogPin.Ain3);

            Ain1TextBox.Text = ain1.ToString("N2");
            byte a1 = (byte)(255 - (ain1 * 255));
            Ain1Border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, a1, a1, a1));
            Ain1TextBox.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, (byte)(255-a1), (byte)(255 - a1), (byte)(255 - a1)));
            Ain2TextBox.Text = ain2.ToString("N2");
            byte a2 = (byte)(255 - (ain2 * 255));
            Ain2Border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, a2, a2, a2));
            Ain2TextBox.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, (byte)(255 - a2), (byte)(255 - a2), (byte)(255 - a2)));
            Ain3TextBox.Text = ain3.ToString("N2");
            byte a3 = (byte)(255 - (ain3 * 255));
            Ain3Border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, a3, a3, a3));
            Ain3TextBox.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, (byte)(255 - a3), (byte)(255 - a3), (byte)(255 - a3)));

            if (gamepad != null)
            {
                var reading = gamepad.GetCurrentReading();
                CntrlLTextBox.Text = $"({reading.LeftThumbstickX:N2}, {reading.LeftThumbstickY:N2})";
                CntrlRTextBox.Text = $"({reading.RightThumbstickX:N2}, {reading.RightThumbstickY:N2})";

                var b = GetFlags(reading.Buttons)
                            .Where(btn => (GamepadButtons)btn != GamepadButtons.None)
                            .Select(btn => Enum.GetName(typeof(GamepadButtons), btn));
                ButtonsTextBox.Text = string.Join(", ", b.ToArray());

                hat.MotorA.Speed = Math.Abs(reading.LeftThumbstickY) < 0.2 ? 0.0 : reading.LeftThumbstickY;
                hat.MotorB.Speed = Math.Abs(reading.RightThumbstickY) < 0.2 ? 0.0 : reading.RightThumbstickY;
            }



            //if ((this.i++ % 5) == 0)
            //{
            //    this.LedsTextBox.Text = this.next.ToString();

            //    //this.hat.DIO24On = this.next;
            //    //this.hat.D2.Color = this.next ? GIS.FEZHAT.Color.White : GIS.FEZHAT.Color.Black;
            //    //this.hat.D3.Color =  this.next ? GIS.FEZHAT.Color.White : GIS.FEZHAT.Color.Black;

            //    //this.hat.WriteDigital(GIS.FEZHAT.DigitalPin.DIO16, this.next);
            //    //this.hat.WriteDigital(GIS.FEZHAT.DigitalPin.DIO26, this.next);

            //    this.hat.SetPwmDutyCycle(GIS.FEZHAT.PwmPin.Pwm5, this.next ? 1.0 : 0.0);
            //    this.hat.SetPwmDutyCycle(GIS.FEZHAT.PwmPin.Pwm6, this.next ? 1.0 : 0.0);
            //    this.hat.SetPwmDutyCycle(GIS.FEZHAT.PwmPin.Pwm7, this.next ? 1.0 : 0.0);
            //    this.hat.SetPwmDutyCycle(GIS.FEZHAT.PwmPin.Pwm11, this.next ? 1.0 : 0.0);
            //    this.hat.SetPwmDutyCycle(GIS.FEZHAT.PwmPin.Pwm12, this.next ? 1.0 : 0.0);

            //    

            //    this.next = !this.next;
            //}

            //if (this.hat.IsDIO18Pressed())
            //{
            //    this.hat.S1.Position += 5.0;
            //    this.hat.S2.Position += 5.0;

            //    if (this.hat.S1.Position >= 180.0)
            //    {
            //        this.hat.S1.Position = 0.0;
            //        this.hat.S2.Position = 0.0;
            //    }
            //}


            //if (hat.IsDIO22Pressed())
            //{
            //    hat.MotorA.Speed = 1.0;
            //    hat.MotorB.Speed = 1.0;
            //}
            //else
            //{
            //    hat.MotorA.Speed = 0.0;
            //    hat.MotorB.Speed = 0.0;
            //}
        }

        static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
    }
}
