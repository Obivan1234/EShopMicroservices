namespace Shopping.Web.Pages;

public class ProductListModel
    (ICatalogService catalogService, IBasketService basketService, ILogger<ProductListModel> logger)
    : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string SelectedCategory { get; set; } = default!;
    public IEnumerable<string> CategoryList { get; set; } = new List<string>();
    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();

    public async Task OnGetAsync(string categiryName)
    {
        var response = await catalogService.GetProducts();

        CategoryList = response.Products.SelectMany(p => p.Category).Distinct();

        if (!string.IsNullOrEmpty(categiryName))
        {
            ProductList = response.Products.Where(p => p.Category.Contains(categiryName));
            SelectedCategory = categiryName;
        }
        else
        {
            ProductList = response.Products;
        }
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Add to cart button clicked");

        var productResponse = await catalogService.GetProduct(productId);

        var basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
