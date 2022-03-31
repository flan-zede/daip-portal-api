using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;
using api.Wrappers;
using api.Data;

namespace webapi.Controllers
{
    [Route("crr")]
    [ApiController]
    public class CrrController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public CrrController(DataContext _context, IMapper _mapper)
        {
            mapper = _mapper;
            context = _context;
        }

        [HttpGet]
        public ActionResult<List<Crr>> FindAll([FromQuery] QueryParam query)
        {
            var res = from s in context.Crrs select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Objet.Contains(s));
            res = res.OrderBy(p => p.Objet);
            return Ok(mapper.Map<List<Crr>>(res.ToList()));
        }

        [HttpGet("page")]
        public ActionResult<List<Crr>> Paginate([FromQuery] QueryParam query)
        {
            var res = from s in context.Crrs select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Objet.Contains(s));
            var length = res.Count();
            res = res.Skip((query.Index - 1) * query.Size).Take(query.Size).OrderBy(p => p.Objet);
            return Ok(ResponseService.Format(length, query, mapper.Map<List<Crr>>(res.ToList())));
        }

        [HttpGet("count")]
        public ActionResult<int> Count()
        {
            return Ok(context.Crrs.Count());
        }

        [HttpGet("{id}")]
        public ActionResult<Crr> FindOne(int id)
        {
            var res = context.Crrs.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            return Ok(mapper.Map<Crr>(res));
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Crr> Create(CrrCreate data)
        {
            var item = mapper.Map<Crr>(data);
            if (item == null) throw new ArgumentNullException(nameof(item));
            item.Creation = DateTime.UtcNow.Date;
            item.Modification = DateTime.UtcNow.Date;
            context.Crrs.Add(item);
            context.SaveChanges();
            return Ok(mapper.Map<Crr>(item));
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<Crr> Update(int id, CrrUpdate data)
        {
            var res = context.Crrs.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            mapper.Map(data, res);
            res.Modification = DateTime.UtcNow.Date;
            context.Crrs.Update(res);
            context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var res = context.Crrs.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            context.Crrs.Remove(res);
            context.SaveChanges();
            return NoContent();
        }
    }

}
