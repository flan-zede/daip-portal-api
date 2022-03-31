using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;
using api.Wrappers;
using api.Data;

namespace webapi.Controllers
{
    [Route("note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public NoteController(DataContext _context, IMapper _mapper)
        {
            mapper = _mapper;
            context = _context;
        }

        [HttpGet]
        public ActionResult<List<Note>> FindAll([FromQuery] QueryParam query)
        {
            var res = from s in context.Notes select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Objet.Contains(s));
            res = res.OrderBy(p => p.Objet);
            return Ok(mapper.Map<List<Note>>(res.ToList()));
        }

        [HttpGet("page")]
        public ActionResult<List<Note>> Paginate([FromQuery] QueryParam query)
        {
            var res = from s in context.Notes select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Objet.Contains(s));
            var length = res.Count();
            res = res.Skip((query.Index - 1) * query.Size).Take(query.Size).OrderBy(p => p.Objet);
            return Ok(ResponseService.Format(length, query, mapper.Map<List<Note>>(res.ToList())));
        }

        [HttpGet("count")]
        public ActionResult<int> Count()
        {
            return Ok(context.Notes.Count());
        }

        [HttpGet("{id}")]
        public ActionResult<Note> FindOne(int id)
        {
            var res = context.Notes.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            return Ok(mapper.Map<Note>(res));
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Note> Create(NoteCreate data)
        {
            var item = mapper.Map<Note>(data);
            if (item == null) throw new ArgumentNullException(nameof(item));
            item.Creation = DateTime.UtcNow.Date;
            item.Modification = DateTime.UtcNow.Date;
            context.Notes.Add(item);
            context.SaveChanges();
            return Ok(mapper.Map<Note>(item));
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<Note> Update(int id, NoteUpdate data)
        {
            var res = context.Notes.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            mapper.Map(data, res);
            res.Modification = DateTime.UtcNow.Date;
            context.Notes.Update(res);
            context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var res = context.Notes.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            context.Notes.Remove(res);
            context.SaveChanges();
            return NoContent();
        }
    }

}
