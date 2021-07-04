namespace Itau.Processo.Core.Data.Model
{
    public class AppSettings
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public Petstore PetStore { get; set; }
    }

    public class Petstore
    {
        public API API { get; set; }
        public Web Web { get; set; }
    }

    public class API
    {
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Exercicio1a Exercicio1A { get; set; }
        public Userregressiontest UserRegressionTest { get; set; }
    }

    public class Exercicio1a
    {
        public User User { get; set; }
        public Pet Pet { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailUpdate { get; set; }
    }

    public class Pet
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string TagName { get; set; }
    }

    public class Userregressiontest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastNameUpdate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailUpdate { get; set; }
    }

    public class Web
    {
        public string Url { get; set; }
        public string[] ListaProdutos { get; set; }
    }
}
