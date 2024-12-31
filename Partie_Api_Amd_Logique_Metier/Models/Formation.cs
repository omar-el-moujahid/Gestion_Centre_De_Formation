using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class Formation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titre { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int FormateurId { get; set; }
        public Formateur Formateur { get; set; }


        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The price must be a positive value.")]
        [DataType(DataType.Currency)]
        public decimal Prix { get; set; }

        [Required]
        [StringLength(100)]
        public string EstimationDeDuree { get; set; } // Example: "20 heures"
        [JsonIgnore]
        public ICollection<Inscription> Inscriptions { get; set; }
        [JsonIgnore] 
        public ICollection<Evaluation> Evaluations { get; set; }
        [JsonIgnore]
        public ICollection<Certificate> Certificates { get; set; }
        [JsonIgnore] 
        public ICollection<Payment> Payment { get; set; }
        [JsonIgnore] 
        public ICollection<Media> Media { get; set; }
    }

  
}