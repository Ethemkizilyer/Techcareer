using System.ComponentModel.DataAnnotations;

namespace Techcareer.Models{

    public class Product{

        [Display(Name="Bootcamp Id")]
        public int ProductId {get;set;}
        [Required(ErrorMessage ="Bootcamp Adı zorunludur")]
        [Display(Name="Bootcamp Adı")]
        
        public string? Name {get;set;}
        [Required(ErrorMessage ="Bootcamp Açıklaması zorunludur")]
        [Display(Name="Bootcamp Açıklaması")]
        [StringLength(125,ErrorMessage ="{0} açıklama {2} ve  {1} arasında kadar harf içermelidir.",MinimumLength =50)]
        public string Description {get;set;} =string.Empty;
         [Required(ErrorMessage ="Bootcamp Saati zorunludur")]
         [Range(15,50)]
        [Display(Name="Bootcamp Saati")]
        public decimal? Clock {get;set;} 
        
        [Display(Name="Bootcamp Resmi")]
        public string? Image {get;set;} =string.Empty;
        public bool IsActive {get;set;}
        [Display(Name="Bootcamp Categorisi")]
        [Required(ErrorMessage ="Bootcamp Categorisi zorunludur")]
        public int? CategoryId {get;set;}
    }
}