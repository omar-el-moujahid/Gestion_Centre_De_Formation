using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class Participant:User
    {
        [JsonIgnore]
        // Liste des formations auxquelles l'étudiant est inscrit
        public ICollection<Inscription> Inscriptions { get; set; }
        [JsonIgnore]
        // Informations sur les paiements effectués par l'étudiant
        public ICollection<Payment> Payments { get; set; }
        [JsonIgnore]
        // Certifications obtenues par l'étudiant
        public ICollection<Certificate> Certificates { get; set; }
        [JsonIgnore]
        public ICollection<Evaluation> Evaluations { get; set; }

    }
}