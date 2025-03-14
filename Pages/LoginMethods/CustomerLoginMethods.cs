using Newtonsoft.Json;

namespace DineAuto.Pages.LoginMethods
{
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
