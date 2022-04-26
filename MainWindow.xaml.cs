using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using ConPDS.Net.Data;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ConPDS.Net
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                System.Drawing.Image img = System.Drawing.Image.FromFile(op.FileName);

                RecogFromFileToJson(img);
                RecogFromBufferToJSON(img);
            }
        }
        private void RecogFromFileToJson(System.Drawing.Image img)
        {

            try
            {
                string jsonData = ConPDS.Net.Proxy.RecogFromFileToJson(img);
                dynamic data = null;
                if (!string.IsNullOrEmpty(jsonData))
                {
                     data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);
                }
                String recogStructure = data.result.ToString();
                if (recogStructure == null)
                {
                    textStructValue.Text = "<Container Code not Recognized>";
                }
                else
                {
                    textStructValue.Text = recogStructure;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error:");
            }
        }


        private void RecogFromBufferToJSON(System.Drawing.Image img)
        {
            try
            {
                byte[] buffer = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
                string jsonData = ConPDS.Net.Proxy.RecogFromBufferToJson(buffer);
                dynamic data = null;

                if (!string.IsNullOrEmpty(jsonData))
                {
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);
                }

                String recogStructure = data.result.ToString();
                if (recogStructure == null)
                {
                    textJsonValue.Text = "<Container Code not Recognized>";
                }
                else
                {
                    textJsonValue.Text = recogStructure;
                    jsonBodyValue.Text = jsonData;
                }

            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error:");
            }
        }
     
    }
}
