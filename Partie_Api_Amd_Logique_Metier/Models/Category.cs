﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le nom de la catégorie ne doit pas dépasser 100 caractères.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Formation> Formations { get; set; }
    }
}
