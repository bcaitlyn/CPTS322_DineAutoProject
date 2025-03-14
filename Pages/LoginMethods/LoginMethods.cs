namespace DineAuto.Pages.LoginMethods
{
    /// <summary>
    /// This abstract class will serve as a way to organize our login types. This way we can keep classes clean across different types of logins.
    /// </summary>
    public abstract class LoginMethods
    {
        public string FilePath = string.Empty;

        /// <summary>
        /// Abstract class for loading users, the database we load depends on the user type, hence why the method is abstract
        /// </summary>
        public abstract Dictionary<string, string> LoadUsers();

    }
}
