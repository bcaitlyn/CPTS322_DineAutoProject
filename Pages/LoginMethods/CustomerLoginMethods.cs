using DineAuto.Pages.Cart;
using Newtonsoft.Json;

namespace DineAuto.Pages.LoginMethods
{
    /// <summary>
    /// Created by Kaden
    /// Each one of these login methods inherits from our Base class login methods.
    /// The purpose of these classes are to load the correct type of users into the dictionary. 
    /// </summary>
    public class CustomerLoginMethods : LoginMethods
    {
        public override Dictionary<string, string> LoadUsers()
        {
            this.FilePath = "Tables/customers.json";
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();

        }
        
    }
}
