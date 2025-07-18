using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs.Response
{
    public class CategoryDto
    {
       public int Id { get; set; }

       [Required]
       public string CategoryName { get; set; }

       [Required]
       public string slug { get; set; }

       public int? ParentId { get; set; }
    }
}
