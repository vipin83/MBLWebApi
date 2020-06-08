using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ManxBirdLifeAppWebApi.Data;
using ManxBirdLifeAppWebApi.Models;

namespace ManxBirdLifeAppWebApi.Controllers
{
    public class SightDetailsController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public IQueryable<MBL_SightDetails> GetMBL_SightDetails()
        {
            return db.MBL_SightDetails;
        }

        [ResponseType(typeof(MBL_SightDetails))]
        public async Task<IHttpActionResult> GetMBL_SightDetails(int id)
        {
            MBL_SightDetails mBL_SightDetails = await db.MBL_SightDetails.FindAsync(id);
            if (mBL_SightDetails == null)
            {
                return NotFound();
            }

            return Ok(mBL_SightDetails);
        }
               
        [ResponseType(typeof(MBL_SightDetails))]
        public async Task<IHttpActionResult> PostMBL_SightDetails(SightingData postData)
        {
            try
            {
                //This is currently redundant, as user cannot enter a location on UI. They have to manually select one from the drop-down.
                int locationID;
                if (!int.TryParse(postData.locationSelected.ToString(), out locationID))
                {
                    //it's a new location, add it to location lookup
                    LocationLookup locationEntity = new LocationLookup()
                    {
                        Name = postData.locationSelected.ToString(),
                        GridReference = postData.gridReference,
                        boolManuallyAddedRecord = true,
                        boolApprovedByAdmin = false                        
                    };

                    db.LocationLookups.Add(locationEntity);
                    db.SaveChanges();
                    //get new location ID
                    locationID = locationEntity.LocationID;
                }

                //convert date time from web page into SQL format
                DateTime tempSightDate = Convert.ToDateTime(postData.dt);
                DateTime tempSightTime = Convert.ToDateTime(postData.myTime);

                DateTime sightDate = tempSightDate.Date;
                TimeSpan sightTime = tempSightTime.TimeOfDay;

                DateTime sightDateTime = sightDate.Date.Add(sightTime);

                var newSightDetails = new MBL_SightDetails
                {
                    SightReportUserID = postData.sightReportUserID,
                    LocationID = locationID,
                    GridReference = postData.gridReference,
                    SightDateTime = sightDateTime,
                    SubmitDateTime = DateTime.Now
                };

                db.MBL_SightDetails.Add(newSightDetails);

                await db.SaveChangesAsync();
                return CreatedAtRoute("DefaultApi", new { id = newSightDetails.SightDetailsID }, newSightDetails);
            }
            catch (Exception ex)
            {

                var error = ex.InnerException.Message;
                return BadRequest("Error saving Sight details - " + error);
            }

        }

        [ResponseType(typeof(MBL_SightDetails))]
        public async Task<IHttpActionResult> DeleteMBL_SightDetails(int id)
        {
            MBL_SightDetails mBL_SightDetails = await db.MBL_SightDetails.FindAsync(id);
            if (mBL_SightDetails == null)
            {
                return NotFound();
            }

            db.MBL_SightDetails.Remove(mBL_SightDetails);
            await db.SaveChangesAsync();

            return Ok(mBL_SightDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}