using ManxBirdLifeAppWebApi.Data;
using ManxBirdLifeAppWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ManxBirdLifeAppWebApi.Controllers
{
    public class AdminSightListingController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public void Post(EditSightingData postData)
        {
            try
            {
                //fetch sight data
                var sightData =  db.MBL_SightDetails.Where(x => x.SightDetailsID == postData.SightDetailsID).FirstOrDefault();
                
                //save details and 

                sightData.MBL_SpeciesSightingDetails.FirstOrDefault().Details = postData.Details;
                sightData.MBL_SpeciesSightingDetails.FirstOrDefault().boolOverrideSensitiveSpeciesRecord = postData.boolOverrideSensitiveSpeciesRecord;

				//find the location entry
				var location = db.LocationLookups.Where(l => l.Name == postData.LocationName).FirstOrDefault();
				sightData.LocationLookup = location;

				//find the species entry
				var species = db.SpeciesLookups.Where(s => s.Name == postData.SpeciesName).FirstOrDefault();
				sightData.MBL_SpeciesSightingDetails.FirstOrDefault().SpeciesLookup = species;

				//sightData.LocationLookup.Name = postData.LocationName;
				//sightData.LocationLookup.boolApprovedByAdmin = true;
				//sightData.MBL_SpeciesSightingDetails.FirstOrDefault().SpeciesLookup.Name = postData.SpeciesName;
				//sightData.MBL_SpeciesSightingDetails.FirstOrDefault().SpeciesLookup.boolApprovedByAdmin = true;
				//sightData.LocationID = postData.LocationID;
				//sightData.MBL_SpeciesSightingDetails.FirstOrDefault().SpeciesID = postData.SpeciesID;

				sightData.GridReference = postData.GridRef;

				//update date and time
				//convert date time from web page into SQL format
				DateTime tempSightDate = Convert.ToDateTime(postData.SightDate);
				DateTime tempSightTime = Convert.ToDateTime(postData.SightTime);

				DateTime sightDate = tempSightDate.Date;
				TimeSpan sightTime = tempSightTime.TimeOfDay;
				sightData.SightDateTime = sightDate.Date.Add(sightTime);				

				db.Entry(sightData).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();				            
            
            }
            catch (Exception ex)
            {
                var error = ex.InnerException.Message;
                //return BadRequest("Error saving Edit sight details - " + error);
            }
        }

    }
}
