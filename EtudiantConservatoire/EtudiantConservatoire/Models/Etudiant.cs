using System.ComponentModel.DataAnnotations;

namespace EtudiantConservatoire.Models
{
    public class Etudiant
    {
        public int Id { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }


        [DataType(DataType.Text)]
        public string? Genre { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateNaissance { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }


        [DataType(DataType.PostalCode)]
        public int CodePostal { get; set; }

        public string Adresse { get; set; }

        public string Ville { get; set; }
    }
}
