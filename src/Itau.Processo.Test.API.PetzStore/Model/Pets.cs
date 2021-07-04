namespace Itau.Processo.Test.API.PetzStore.Model
{
    public class Pets
    {
        public int id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string[] photoUrls { get; set; }
        public Tags[] tags { get; set; }

    }
}
