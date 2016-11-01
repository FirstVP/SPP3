using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace WPhotoEditor
{

    public class MainShaderEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MainShaderEffect), 0);
        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(MainShaderEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register("Contrast", typeof(double), typeof(MainShaderEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(MainShaderEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(MainShaderEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(MainShaderEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
        public MainShaderEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("_Effect.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(BrightnessProperty);
            this.UpdateShaderValue(ContrastProperty);
            this.UpdateShaderValue(RedProperty);
            this.UpdateShaderValue(GreenProperty);
            this.UpdateShaderValue(BlueProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        public double Brightness
        {
            get
            {
                return ((double)(this.GetValue(BrightnessProperty)));
            }
            set
            {
                this.SetValue(BrightnessProperty, value);
            }
        }
        public double Contrast
        {
            get
            {
                return ((double)(this.GetValue(ContrastProperty)));
            }
            set
            {
                this.SetValue(ContrastProperty, value);
            }
        }
        public double Red
        {
            get
            {
                return ((double)(this.GetValue(RedProperty)));
            }
            set
            {
                this.SetValue(RedProperty, value);
            }
        }
        public double Green
        {
            get
            {
                return ((double)(this.GetValue(GreenProperty)));
            }
            set
            {
                this.SetValue(GreenProperty, value);
            }
        }
        public double Blue
        {
            get
            {
                return ((double)(this.GetValue(BlueProperty)));
            }
            set
            {
                this.SetValue(BlueProperty, value);
            }
        }
    }
}
