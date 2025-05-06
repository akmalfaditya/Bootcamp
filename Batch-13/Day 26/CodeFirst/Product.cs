using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst;

public class Product
{
	[Key]
	public int Id { get; set; }
	
	[Column(TypeName = "nvarchar")]
	public string ProductName { get; set; }
	[Column("Description"), DefaultValue("undefined")]
	public string Penjelasan { get; set; }
	public int CategoryId { get; set; }
	public Category Category { get; set; }
	[Column(TypeName = "money")]
	[DefaultValue(100)]
	public int UnitPrice { get; set; }
	[Column(TypeName = "datetime")]
	public DateTime CreateTime { get; set; }
	[DefaultValue("MyValues")]
	public string Notes { get; set; }
}
