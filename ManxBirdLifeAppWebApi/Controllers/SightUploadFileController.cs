using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ManxBirdLifeAppWebApi.Data;
using ManxBirdLifeAppWebApi.Models;
using System;

namespace ManxBirdLifeAppWebApi.Controllers
{

    //Gallery controller
    public class SightUploadFileController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public SightUploadFile[] GetMBL_SpeciesSightingUploadFile()
        {

            var strFileUploadDirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/FileUploads");

            var returnData = db.MBL_SpeciesSightingUploadFile.ToList()
                .Where(x => !(x.MBL_SpeciesSightingDetails.boolOverrideSensitiveSpeciesRecord.HasValue && x.MBL_SpeciesSightingDetails.boolOverrideSensitiveSpeciesRecord.Value) &&
                            CommonUtility.SpeciesNotInSenstiveDateRange(x.MBL_SpeciesSightingDetails)
                      )
                .Select(upload => new SightUploadFile
                    {
                        url = upload.FileName,
                        thumbUrl = upload.FileName,
                        caption = upload.MBL_SpeciesSightingDetails.SpeciesLookup.Name + ", " + upload.MBL_SpeciesSightingDetails.MBL_SightDetails.LocationLookup.Name + " \r\n © " + (!upload.MBL_SpeciesSightingDetails.MBL_SightDetails.MBL_SightReportUser.boolKeepAnonymous ? upload.MBL_SpeciesSightingDetails.MBL_SightDetails.MBL_SightReportUser.Name : "Undisclosed") + " (" + upload.MBL_SpeciesSightingDetails.MBL_SightDetails.SightDateTime.ToString("dd MMMM yyyy") + ")  ",
                        speciesName = upload.MBL_SpeciesSightingDetails.SpeciesLookup.Name,
                        UploadDateTime = upload.MBL_SpeciesSightingDetails.MBL_SightDetails.SubmitDateTime
                    })
                .OrderByDescending(o => o.UploadDateTime)
                .ToArray();

            return returnData;
        }
        
        [ResponseType(typeof(MBL_SpeciesSightingUploadFile))]
        public async Task<IHttpActionResult> GetMBL_SpeciesSightingUploadFile(int id)
        {
            MBL_SpeciesSightingUploadFile mBL_SpeciesSightingUploadFile = await db.MBL_SpeciesSightingUploadFile.FindAsync(id);
            if (mBL_SpeciesSightingUploadFile == null)
            {
                return NotFound();
            }

            return Ok(mBL_SpeciesSightingUploadFile);
        }

        [ResponseType(typeof(MBL_SpeciesSightingUploadFile))]
        public async Task<IHttpActionResult> PostMBL_SpeciesSightingUploadFile(MBL_SpeciesSightingUploadFile mBL_SpeciesSightingUploadFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MBL_SpeciesSightingUploadFile.Add(mBL_SpeciesSightingUploadFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mBL_SpeciesSightingUploadFile.SpeciesSightingUploadFileID }, mBL_SpeciesSightingUploadFile);
        }

        [ResponseType(typeof(MBL_SpeciesSightingUploadFile))]
        public async Task<IHttpActionResult> DeleteMBL_SpeciesSightingUploadFile(int id)
        {
            MBL_SpeciesSightingUploadFile mBL_SpeciesSightingUploadFile = await db.MBL_SpeciesSightingUploadFile.FindAsync(id);
            if (mBL_SpeciesSightingUploadFile == null)
            {
                return NotFound();
            }

            db.MBL_SpeciesSightingUploadFile.Remove(mBL_SpeciesSightingUploadFile);
            await db.SaveChangesAsync();

            return Ok(mBL_SpeciesSightingUploadFile);
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

    public class SightUploadFile
    {
        public string url { get; set; }
        public string thumbUrl { get; set; }
        public string caption { get; set; }

        public string speciesName { get; set; }
        public DateTime UploadDateTime { get; set; }
    }
}