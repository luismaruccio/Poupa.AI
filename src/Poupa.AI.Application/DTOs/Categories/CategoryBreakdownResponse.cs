namespace Poupa.AI.Application.DTOs.Categories
{
    public class CategoryBreakdownResponse(IEnumerable<CategoryResponse> incomes, IEnumerable<CategoryResponse> expenses)
    {
        public List<CategoryResponse> Incomes { get; set; } = incomes.ToList();
        public List<CategoryResponse> Expenses { get; set; } = expenses.ToList();
    }
}
