using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIFORDATA.Models
{
    public class Participant
    {
        // Liste des formations auxquelles l'étudiant est inscrit
        public ICollection<Inscription> Inscriptions { get; set; } 
        // Informations sur les paiements effectués par l'étudiant
        public ICollection<Payment> Payments { get; set; }
        // Certifications obtenues par l'étudiant
        public ICollection<Certificate> Certificates { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }

    }
}