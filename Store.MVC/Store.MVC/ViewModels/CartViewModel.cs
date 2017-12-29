using Store.Models.Entities;
using System.Collections.Generic;

namespace Store.MVC.ViewModels
{
    public class CartViewModel
    {
        public Customer Customer { get; set; }
        public IList<CartRecordViewModel> CartRecords { get; set; } 
    }
}