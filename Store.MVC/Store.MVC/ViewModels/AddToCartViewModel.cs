using Store.MVC.Validations;
using Store.MVC.ViewModels.Base;

namespace Store.MVC.ViewModels
{
    public class AddToCartViewModel :CartViewModelBase
    {
        [MustNotBeGreaterThan(nameof(UnitsInStock)), MustBeGreaterThanZero]
        public int Quantity { get; set; }
    }
}