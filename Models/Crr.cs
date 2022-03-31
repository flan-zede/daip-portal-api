using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace api.Models
{
    public class Crr
    {
        [Key]
        public int Id { get; set; }
        public string? Redacteur { get; set; }
        public string? Fonction { get; set; }
        public DateTime Date { get; set; }
        public string? Lieu { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public string? Presidence { get; set; }
        public string? Pj { get; set; }
        public string? Objet { get; set; }
        public string? Introduction { get; set; }
        public string? Information { get; set; }
        public string? EtatLieu { get; set; }
        public string? Question { get; set; }
        public string? Diligence { get; set; }
        public DateTime DebutProchain { get; set; }
        public DateTime FinProchain { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
    }

    public class CrrCreate
    {

        public string? Redacteur { get; set; }
        public string? Fonction { get; set; }
        public DateTime Date { get; set; }
        public string? Lieu { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public string? Presidence { get; set; }
        public string? Pj { get; set; }
        public string? Objet { get; set; }
        public string? Introduction { get; set; }
        public string? Information { get; set; }
        public string? EtatLieu { get; set; }
        public string? Question { get; set; }
        public string? Diligence { get; set; }
        public DateTime DebutProchain { get; set; }
        public DateTime FinProchain { get; set; }
    }

    public class CrrUpdate : CrrCreate
    {
    }

    public class CrrProfile : Profile
    {
        public CrrProfile()
        {
            CreateMap<CrrCreate, Crr>();
            CreateMap<CrrUpdate, Crr>();
            CreateMap<Crr, CrrUpdate>();
        }
    }
}
