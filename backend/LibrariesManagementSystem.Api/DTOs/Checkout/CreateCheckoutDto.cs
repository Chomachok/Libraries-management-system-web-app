namespace LibrariesManagementSystem.Api.DTOs.Checkout;

public class CreateCheckoutDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int DurationDays { get; set; } = 14;
}