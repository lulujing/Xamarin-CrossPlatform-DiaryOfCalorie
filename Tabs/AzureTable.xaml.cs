using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Tabs
{
    public partial class AzureTable : ContentPage
    {
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        public AzureTable()
        {
            InitializeComponent();

        }
        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<FoodCalorie> notHotDogInformation = await AzureManager.AzureManagerInstance.GetHotDogInformation();
           
            List<FoodCalorieM> result = new List<FoodCalorieM>();
            foreach(var item in notHotDogInformation)
            {
                FoodCalorieM a = new FoodCalorieM();
                a.updatedAt = item.updatedAt.ToString("yyyy/MM/dd");
                a.Tag = item.Tag;
                a.quantity = item.quantity;
                a.CaloriePerG = item.CaloriePerG;
                result.Add(a);

            }

            HotDogList.ItemsSource = result;
        }
    }
}
