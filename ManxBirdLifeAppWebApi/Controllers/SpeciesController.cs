using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ManxBirdLifeAppWebApi.Data;

namespace ManxBirdLifeAppWebApi.Controllers
{
    public class SpeciesController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public object[] GetSpeciesLookups()
        {
            return db.SpeciesLookups
                .Where(sp => !sp.boolManuallyAddedRecord.HasValue ? true: (sp.boolApprovedByAdmin.HasValue && sp.boolApprovedByAdmin.Value))
                     .Select(x => new { ID = x.SpeciesID, Name = x.Name, level = x.TaxaLevel })
                     .OrderBy(o=>o.Name)
                     .ToArray();
        }

        [ResponseType(typeof(SpeciesLookup))]
        public IHttpActionResult GetSpeciesLookup(int id)
        {
            SpeciesLookup speciesLookup = db.SpeciesLookups.Find(id);
            if (speciesLookup == null)
            {
                return NotFound();
            }

            return Ok(speciesLookup);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutSpeciesLookup(int id, SpeciesLookup speciesLookup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != speciesLookup.SpeciesID)
            {
                return BadRequest();
            }

            db.Entry(speciesLookup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeciesLookupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(SpeciesLookup))]
        public IHttpActionResult PostSpeciesLookup(SpeciesLookup speciesLookup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SpeciesLookups.Add(speciesLookup);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = speciesLookup.SpeciesID }, speciesLookup);
        }

        [ResponseType(typeof(SpeciesLookup))]
        public IHttpActionResult DeleteSpeciesLookup(int id)
        {
            SpeciesLookup speciesLookup = db.SpeciesLookups.Find(id);
            if (speciesLookup == null)
            {
                return NotFound();
            }

            db.SpeciesLookups.Remove(speciesLookup);
            db.SaveChanges();

            return Ok(speciesLookup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpeciesLookupExists(int id)
        {
            return db.SpeciesLookups.Count(e => e.SpeciesID == id) > 0;
        }
    }
}