using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;
using api.Wrappers;
using api.Data;

namespace webapi.Controllers
{
    [Route("crm")]
    [ApiController]
    public class CrmController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public CrmController(DataContext _context, IMapper _mapper)
        {
            mapper = _mapper;
            context = _context;
        }

        [HttpGet]
        public ActionResult<List<Crm>> FindAll([FromQuery] QueryParam query)
        {
            var res = from s in context.Crms select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Objet.Contains(s));
            res = res.OrderBy(p => p.Objet);
            return Ok(mapper.Map<List<Crm>>(res.ToList()));
        }

        [HttpGet("page")]
        public ActionResult<List<Crm>> Paginate([FromQuery] QueryParam query)
        {
            var res = from s in context.Crms select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Objet.Contains(s));
            var length = res.Count();
            res = res.Skip((query.Index - 1) * query.Size).Take(query.Size).OrderBy(p => p.Objet);
            return Ok(ResponseService.Format(length, query, mapper.Map<List<Crm>>(res.ToList())));
        }

        [HttpGet("count")]
        public ActionResult<int> Count()
        {
            return Ok(context.Crms.Count());
        }

        [HttpGet("{id}")]
        public ActionResult<Crm> FindOne(int id)
        {
            var res = context.Crms.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            return Ok(mapper.Map<Crm>(res));
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Crm> Create(CrmCreate data)
        {
            var item = mapper.Map<Crm>(data);
            if (item == null) throw new ArgumentNullException(nameof(item));
            item.Creation = DateTime.UtcNow.Date;
            item.Modification = DateTime.UtcNow.Date;
            context.Crms.Add(item);
            context.SaveChanges();
            return Ok(mapper.Map<Crm>(item));
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<Crm> Update(int id, CrmUpdate data)
        {
            var res = context.Crms.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            mapper.Map(data, res);
            res.Modification = DateTime.UtcNow.Date;
            context.Crms.Update(res);
            context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var res = context.Crms.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            context.Crms.Remove(res);
            context.SaveChanges();
            return NoContent();
        }
    }

}
