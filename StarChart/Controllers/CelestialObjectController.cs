using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if(celestialObject != null)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(o => o.Name.Equals(name)).ToList();
            if (celestialObjects.Any())
            {
                celestialObjects.ForEach(o => o.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == o.Id).ToList());
                return Ok(celestialObjects);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            var celestialObjects = _context.CelestialObjects.ToList();
            celestialObjects.ForEach(o => o.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == o.Id).ToList());
            return Ok(celestialObjects);
        }
    }
}
