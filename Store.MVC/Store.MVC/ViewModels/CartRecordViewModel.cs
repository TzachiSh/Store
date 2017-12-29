using Store.MVC.Validations;
using Store.MVC.ViewModels.Base;

namespace Store.MVC.ViewModels
{
    public class CartRecordViewModel : CartViewModelBase
    {
        [MustNotBeGreaterThan(nameof(UnitsInStock))]
        public int Quantity { get; set; }
    }
}
