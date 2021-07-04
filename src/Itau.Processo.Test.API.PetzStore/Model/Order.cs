using System;

namespace Itau.Processo.Test.API.PetzStore.Model
{
    public class Order
    {
        public int id { get; set; }
        public int petId { get; set; }
        public int quantity { get; set; }
        public DateTime shipDate { get; set; }
        public string status { get; set; }
        public bool complete { get; set; }
        public int userId { get; set; }

    }
}
