using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabs
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<FoodCalorie> foodCalorieTable;
       

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://lulu2017.azurewebsites.net");
            this.foodCalorieTable = this.client.GetTable<FoodCalorie>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }
        public async Task<List<FoodCalorie>> GetHotDogInformation()
        {
            return await this.foodCalorieTable.ToListAsync();
        }
        public async Task PostHotDogInformation(FoodCalorie notHotDogModel)
        {
            await this.foodCalorieTable.InsertAsync(notHotDogModel);
        }
    }
}
