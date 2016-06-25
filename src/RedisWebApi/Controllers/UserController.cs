using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedisWebApi.Models;
using StackExchange.Redis;

namespace RedisWebApi.Controllers
{
    [Route("api/[controller]s")]
    public class UserController : Controller
    {
        private readonly IDatabase redis;

        public UserController(IDatabase redis)
        {
            this.redis = redis;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = redis.HashGet("users", id);
            if (!result.HasValue) return NotFound();
            return Ok(JsonConvert.DeserializeObject<User>(result.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await redis.HashValuesAsync("users");
            return Ok(values.Where(v => v.HasValue).Select(v => JsonConvert.DeserializeObject<User>(v)).ToArray());
        }

        [HttpGet("aoData")]
        public async Task<IActionResult> GetAoData()
        {
            var values = await redis.HashValuesAsync("users");
            return Ok(new
            {
                aaData = values.Where(v => v.HasValue).Select(v => JsonConvert.DeserializeObject<User>(v))
                    .Select(u => new string[] { u.FirstName, u.LastName, u.Email, u.Gender.ToString() }).ToArray()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            user.Id = Guid.NewGuid().ToString();
            var operation = await redis.HashSetAsync("users", user.Id, JsonConvert.SerializeObject(user));
            if (!operation) return BadRequest("Operation did not succeed");
            return CreatedAtAction("Get", new { id = user.Id }, user);
        }
    }
}
