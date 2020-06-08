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
    public class LocationController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public object[] GetLocationLookups()
        {
            return db.LocationLookups
                .Where(loc => !loc.boolManuallyAddedRecord.HasValue ? true : (loc.boolApprovedByAdmin.HasValue && loc.boolApprovedByAdmin.Value))
                .Select(x => new { ID = x.LocationID, Name = x.Name, GridRef = x.GridReference })
                .OrderBy(o => o.Name)
                .ToArray();
        }

        [ResponseType(typeof(LocationLookup))]
        public IHttpActionResult GetLocationLookup(int id)
        {
            LocationLookup locationLookup = db.LocationLookups.Find(id);
            if (locationLookup == null)
            {
                return NotFound();
            }

            return Ok(locationLookup);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutLocationLookup(int id, LocationLookup locationLookup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationLookup.LocationID)
            {
                return BadRequest();
            }

            db.Entry(locationLookup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationLookupExists(id))
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

        [ResponseType(typeof(LocationLookup))]
        public IHttpActionResult PostLocationLookup(LocationLookup locationLookup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LocationLookups.Add(locationLookup);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = locationLookup.LocationID }, locationLookup);
        }

        [ResponseType(typeof(LocationLookup))]
        public IHttpActionResult DeleteLocationLookup(int id)
        {
            LocationLookup locationLookup = db.LocationLookups.Find(id);
            if (locationLookup == null)
            {
                return NotFound();
            }

            db.LocationLookups.Remove(locationLookup);
            db.SaveChanges();

            return Ok(locationLookup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationLookupExists(int id)
        {
            return db.LocationLookups.Count(e => e.LocationID == id) > 0;
        }
    }
}