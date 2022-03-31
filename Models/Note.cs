using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace api.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public string? Redacteur { get; set; }
        public DateTime Date { get; set; }
        public string? Objet { get; set; }
        public string? Contexte { get; set; }
        public string? Problematique { get; set; }
        public string? Analyse { get; set; }
        public string? Recommandation { get; set; }
        public string? Instruction { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
    }

    public class NoteCreate
    {
        public string? Redacteur { get; set; }
        public DateTime Date { get; set; }
        public string? Objet { get; set; }
        public string? Contexte { get; set; }
        public string? Problematique { get; set; }
        public string? Analyse { get; set; }
        public string? Recommandation { get; set; }
        public string? Instruction { get; set; }
    }

    public class NoteUpdate : NoteCreate
    {
    }

    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<NoteCreate, Note>();
            CreateMap<NoteUpdate, Note>();
            CreateMap<Note, NoteUpdate>();
        }
    }
}
