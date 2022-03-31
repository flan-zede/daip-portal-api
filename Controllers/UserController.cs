using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;
using api.Wrappers;
using api.Data;

namespace webapi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserController(DataContext _context, IMapper _mapper)
        {
            mapper = _mapper;
            context = _context;
        }

        [HttpGet]
        public ActionResult<List<UserResponse>> FindAll([FromQuery] QueryParam query)
        {
            var res = from s in context.Users select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Username.Contains(s) || p.Email.Contains(s));
            res = res.OrderBy(p => p.Username);
            return Ok(mapper.Map<List<UserResponse>>(res.ToList()));
        }

        [HttpGet("page")]
        public ActionResult<List<UserResponse>> Paginate([FromQuery] QueryParam query)
        {
            var res = from s in context.Users select s;
            query = QueryParamService.Control(query);
            foreach (var s in query.SearchKeys) res = res.Where(p => p.Username.Contains(s) || p.Email.Contains(s));
            var length = res.Count();
            res = res.Skip((query.Index - 1) * query.Size).Take(query.Size).OrderBy(p => p.Username);
            return Ok(ResponseService.Format(length, query, mapper.Map<List<UserResponse>>(res.ToList())));
        }

        [HttpGet("count")]
        public ActionResult<int> Count([FromQuery] string role)
        {
            return Ok(context.Users.Where(p => p.Role == role).Count());
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> FindOne(int id)
        {
            var userId = int.Parse(User.Identity.Name);
            if (id != userId && !User.IsInRole(Role.ADMIN)) return Forbid();
            var res = context.Users.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            return Ok(mapper.Map<UserResponse>(res));
        }

        [HttpPost]
        public ActionResult Create(UserCreate data)
        {
            var item = mapper.Map<User>(data);
            if (item == null) throw new ArgumentNullException(nameof(item));

            var res = context.Users.FirstOrDefault(p => p.Username == item.Username || p.Email == item.Email);
            if (res != null) return NotFound(new { message = "Le nom d'user ou l'adresse email déjà en cours d'utilisation par un autre profil" });

            item.Password = BCrypt.Net.BCrypt.HashPassword(item.Password);
            item.Creation = DateTime.UtcNow.Date;
            item.Modification = DateTime.UtcNow.Date;
            context.Users.Add(item);
            context.SaveChanges();

            res = context.Users.FirstOrDefault(p => p.Username == item.Username || p.Email == item.Email);
            var Jwt = JwtService.CreateJwt(res);
            var User = mapper.Map<UserResponse>(res);
            return Ok(new { User, Jwt });
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<UserResponse> Update(int id, UserUpdate data)
        {
            var res = context.Users.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            mapper.Map(data, res);
            res.Modification = DateTime.UtcNow.Date;
            context.Users.Update(res);
            context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public ActionResult PartialUpdate(int id, JsonPatchDocument<UserUpdate> data)
        {
            var res = context.Users.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            var item = mapper.Map<UserUpdate>(res);
            data.ApplyTo(item, ModelState);
            if (!TryValidateModel(item)) return ValidationProblem(ModelState);
            mapper.Map(item, res);
            res.Modification = DateTime.UtcNow.Date;
            context.Users.Update(res);
            context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var res = context.Users.FirstOrDefault(p => p.Id == id);
            if (res == null) return NotFound();
            context.Users.Remove(res);
            context.SaveChanges();
            return NoContent();
        }

        [HttpPost, Route("login")]
        public ActionResult Login(Login data)
        {
            var item = mapper.Map<Login>(data);
            if (item == null) throw new ArgumentNullException(nameof(item));
            var res = context.Users.FirstOrDefault(p => p.Username == item.Username);
            if (res != null && BCrypt.Net.BCrypt.Verify(item.Password, res.Password))
            {
                var Jwt = JwtService.CreateJwt(res);
                var User = mapper.Map<UserResponse>(res);
                return Ok(new { User, Jwt });
            }
            return NotFound(new { message = "Aucun compte user trouvé avec ces identifiants" });
        }

        [HttpPost, Route("check")]
        public ActionResult Check(Check data)
        {
            var item = mapper.Map<Check>(data);
            if (item == null) throw new ArgumentNullException(nameof(item));
            var res = context.Users.FirstOrDefault(p => p.Username == item.Username || p.Email == item.Email);
            if (res == null) return NotFound();
            return Ok();
        }

    }

}
