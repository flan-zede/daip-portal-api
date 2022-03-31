using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace api.Models
{
    public class Crm
    {
        [Key]
        public int Id { get; set; }
        public string? Objet { get; set; }
        public string? Introduction { get; set; }
        public string? Deroulement { get; set; }
        public string? Analyse { get; set; }
        public string? Diligence { get; set; }
        public string? Instruction { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
    }

    public class CrmCreate
    {
        public string? Objet { get; set; }
        public string? Introduction { get; set; }
        public string? Deroulement { get; set; }
        public string? Analyse { get; set; }
        public string? Diligence { get; set; }
        public string? Instruction { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
    }

    public class CrmUpdate : CrmCreate
    {
    }

    public class CrmProfile : Profile
    {
        public CrmProfile()
        {
            CreateMap<CrmCreate, Crm>();
            CreateMap<CrmUpdate, Crm>();
            CreateMap<Crm, CrmUpdate>();
        }
    }
}
