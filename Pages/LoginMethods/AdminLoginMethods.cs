using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace DineAuto.Pages.LoginMethods
{
    public class AdminLoginMethods : LoginMethods
    {

        public override Dictionary<string, string> LoadUsers()
        {
            this.FilePath = "Tables/admins.json";
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();

        }
    }
}

