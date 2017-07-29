using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Tabs.Model;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Tabs
{
    public partial class CustomVision : ContentPage
    {
        public CustomVision()
        {
            InitializeComponent();
        }

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            TagLabel.Text = "";

            await MakePredictionRequest(file);
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "cb3277985d2148ccb06c7a86262b7f70");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/2882c483-adfd-4da2-8cc3-24213a64afdc/image"; 

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);
                   
                    
                    double max = responseModel.Predictions.Max(m => m.Probability);
                    var lab = responseModel.Predictions.Max(m => m.Tag);
                   
                    var f = lab.Split('_');
                  
                    if (max >= 0.5)
                     {
                        TagLabel.Text = f[0];
                        TagLabel1.Text = "1";
                        TagLabel2.Text = f[1];
                     }
                    else
                    {
                        TagLabel.Text = "Undefine";
                        TagLabel1.Text = "1";
                    }

                   
                }

                //Get rid of file once we have finished using it
                file.Dispose();
            }
        }

        async void Save(object sender, System.EventArgs e)
        {
            FoodCalorie postinformation = new FoodCalorie();
            postinformation.Tag = TagLabel.Text;
            postinformation.quantity = Convert.ToInt32(TagLabel1.Text);
            postinformation.CaloriePerG= Convert.ToInt32(TagLabel2.Text);

            await AzureManager.AzureManagerInstance.PostHotDogInformation(postinformation);
            TagLabel.Text = "";
            TagLabel1.Text = "";
            TagLabel2.Text = "";
            image.Source = "";
        }
    }
}
