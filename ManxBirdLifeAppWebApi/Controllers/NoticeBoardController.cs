using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using ManxBirdLifeAppWebApi.Data;
using ManxBirdLifeAppWebApi.Models;

namespace ManxBirdLifeAppWebApi.Controllers
{
    public class NoticeBoardController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public IHttpActionResult GetMBL_SightDetails()
        {
            //This is common API which gets called from both public and admin noticeboards query.
            //So, this returns all the rows with appropriate flags to indicate they are sensitive or require admin intervention.
            var response = db.MBL_SpeciesSightingDetails.AsEnumerable()
                .Select(species => new SightListData
                {
                    SightDate = species.MBL_SightDetails.SightDateTime.Date,//.ToString("dd MMM yy"),
                    SightTime = species.MBL_SightDetails.SightDateTime.ToString("HH:mm"),
                    SpeciesName = species.SpeciesLookup.Name,
                    Number = species.Approximation == "=" ?
                                species.Number.ToString() :
                                species.Approximation.Equals("c.", StringComparison.InvariantCultureIgnoreCase) ?
                                                        species.Approximation + species.Number :
                                                        species.Number + "+",
                    LocationName = species.MBL_SightDetails.LocationLookup.Name,
                    GridRef = species.MBL_SightDetails.GridReference,
                    Observer = species.MBL_SightDetails.MBL_SightReportUser.Name,
                    HasImage = species.MBL_SpeciesSightingUploadFile.Count > 0 ? true : false,
                    ImagePath = species.MBL_SpeciesSightingUploadFile.Count > 0 ? species.MBL_SpeciesSightingUploadFile.First().FileName : null,
                    Details = species.Details,
                    ObserverEmail = species.MBL_SightDetails.MBL_SightReportUser.Email,
                    boolDoNotDisplayObserver = species.MBL_SightDetails.MBL_SightReportUser.boolKeepAnonymous,
                    boolDoNotDisplaySpecies = species.boolSensitiveSpeciesRecord,
                    boolSpeciesWithinSensitiveDateRange = !CommonUtility.SpeciesNotInSenstiveDateRange(species),
                    boolNeedApproval = CommonUtility.DoesSightEntryNeedApproval(species),
                    boolNewUnApprovedSpeciesName = CommonUtility.HasManuallyAddedUnApprovedSpeciesName(species),
                    boolNewUnApprovedLocationName = CommonUtility.HasManuallyAddedUnApprovedLocationName(species),
                    boolOverrideSensitiveSpeciesRecord = species.boolOverrideSensitiveSpeciesRecord.HasValue && species.boolOverrideSensitiveSpeciesRecord.Value,
                    SightDetailsID = species.MBL_SightDetails.SightDetailsID,
                    SpeciesSightingDetailsID = species.SpeciesSightingDetailsID,
                    SpeciesID = species.SpeciesID,
                    LocationID = species.MBL_SightDetails.LocationID
                })
            .OrderByDescending(d => d.SightDate)
            .ThenByDescending(t => t.SightTime)            
            .ToArray();

            return Ok(response);
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

    public class SightListData
    {
        public DateTime SightDate { get; set; }
        public string SightTime { get; set; }
        public string SpeciesName { get; set; }
        public string Number { get; set; }
        public string LocationName { get; set; }
        public string GridRef { get; set; }
        public string Observer { get; set; }
        public bool HasImage { get; set; }
        public string ImagePath { get; set; }
        public string Details { get; set; }
        public string ObserverEmail { get; set; }
        public bool boolDoNotDisplayObserver { get; set; }
        public bool boolDoNotDisplaySpecies { get; set; }       

        //Need approval
        public bool boolNeedApproval { get; set; }
        public bool boolNewUnApprovedLocationName { get; set; }
        public bool boolNewUnApprovedSpeciesName { get; set; }
        public bool boolOverrideSensitiveSpeciesRecord { get; set; }
        public bool boolSpeciesWithinSensitiveDateRange { get; set; }

        public int SightDetailsID { get; set; }
        public int SpeciesSightingDetailsID { get; set; }
        public int SpeciesID { get; set; }
        public int LocationID { get; set; }
    }
}