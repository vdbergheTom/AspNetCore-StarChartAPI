using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
            if (celestialObject != null)
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject modify)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (celestialObject != null)
            {
                celestialObject.Name = modify.Name;
                celestialObject.OrbitalPeriod = modify.OrbitalPeriod;
                celestialObject.OrbitedObjectId = modify.OrbitedObjectId;
                _context.Update(celestialObject);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (celestialObject != null)
            {
                celestialObject.Name = name;
                _context.CelestialObjects.Update(celestialObject);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(o => o.Id == id || o.OrbitedObjectId == id);
            if (celestialObjects.Any())
            {
                _context.CelestialObjects.RemoveRange(celestialObjects);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }
    }
}
