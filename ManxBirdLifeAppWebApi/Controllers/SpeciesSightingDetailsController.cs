using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ManxBirdLifeAppWebApi.Data;
using System.IO;
using ManxBirdLifeAppWebApi.Models;

namespace ManxBirdLifeAppWebApi.Controllers
{
    public class SpeciesSightingDetailsController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public IQueryable<MBL_SpeciesSightingDetails> GetMBL_SpeciesSightingDetails()
        {
            return db.MBL_SpeciesSightingDetails;
        }

        [ResponseType(typeof(MBL_SpeciesSightingDetails))]
        public async Task<IHttpActionResult> GetMBL_SpeciesSightingDetails(int id)
        {
            MBL_SpeciesSightingDetails mBL_SpeciesSightingDetails = await db.MBL_SpeciesSightingDetails.FindAsync(id);
            if (mBL_SpeciesSightingDetails == null)
            {
                return NotFound();
            }

            return Ok(mBL_SpeciesSightingDetails);
        }

        public async Task<IHttpActionResult> PostMBL_SpeciesSightingDetails()
        {
            string fileNameWithExtension = "";
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = MultipartFormDataUtility.GetMultipartProvider();
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FileData.Count > 0)
                {
                    // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
                    // so this is how you can get the original file name
                    var originalFileName = MultipartFormDataUtility.GetDeserializedFileName(result.FileData.First());


                    //rename file with extension
                    string defaultExtension = ".jpg";

                    try
                    {
                        defaultExtension = MultipartFormDataUtility.GetDefaultExtension(result.FileData.First().Headers.ContentType.MediaType);
                    }
                    catch { }

                    if (String.IsNullOrEmpty(defaultExtension))
                    {
                        defaultExtension = ".jpg";
                    }

                    fileNameWithExtension = result.FileData.First().LocalFileName + defaultExtension;

                    try
                    {
                        //rename file with .jpg extension
                        File.Move(result.FileData.First().LocalFileName, fileNameWithExtension);
                    }
                    catch (IOException ex)
                    {
                        var error = ex.InnerException.Message;
                        return BadRequest("Error saving uploaded file to gallery - " + error);                        
                    }
                    catch (Exception ex)
                    {
                        var error = ex.InnerException.Message;
                        return BadRequest("Error saving species sight details - " + error);
                    }

                    // uploadedFileInfo object will give you some additional stuff like file length,
                    // creation time, directory name, a few filesystem methods etc..
                    //uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
                }

                //extract species information from REQUEST and copnvert it into 'Species' object                
                Species speciesUploadData = MultipartFormDataUtility.GetFormData<Species>(result);

                if (speciesUploadData == null)
                {                    
                    return BadRequest("Error saving species sight details, as cannot deserialized into <Species> object.");
                }
                                
                //attempt to read species 
                int speciedID;
                if (!int.TryParse(speciesUploadData.speciesSelected.ToString(), out speciedID))
                {
                    //NOTE - this part is redundant now, as user can no longer enter species on their own. They have to mandotorily select from the drop down.
                    //it's a new species, add it to species lookup
                    var speciesEntity = new SpeciesLookup()
                    {
                        Name = speciesUploadData.speciesSelected.ToString(),
                        TaxaLevel = 1, //default taxa level
                        boolManuallyAddedRecord = true,
                        boolApprovedByAdmin = false                        
                    };

                    db.SpeciesLookups.Add(speciesEntity);
                    db.SaveChanges();
                    //get newly added species ID
                    speciedID = speciesEntity.SpeciesID;
                }


                var speciesSightEntity = new MBL_SpeciesSightingDetails()
                {        
                    SightDetailsID = speciesUploadData.SightDetailsID,                    
                    SpeciesID = speciedID,
                    Number = speciesUploadData.speciesnumber,
                    Approximation = speciesUploadData.approximation,
                    Details = speciesUploadData.details,
                    boolSensitiveSpeciesRecord = speciesUploadData.chkSensitiveDoNotDisplay,
                    boolOverrideSensitiveSpeciesRecord = speciesUploadData.chkSensitiveDoNotDisplay //initially set override flag to same as what user has selected on screen
                };

                db.MBL_SpeciesSightingDetails.Add(speciesSightEntity);
                db.SaveChanges(); //have to save this first to get the ID, which will be used below as FK.

                var speciesSightDetailsID = speciesSightEntity.SpeciesSightingDetailsID;

                //create entries for file (if uplaoded by user)
                if (result.FileData.Count > 0)
                {
                    var speciesFileEntity = new MBL_SpeciesSightingUploadFile
                    {
                        SpeciesSightingDetailsID = speciesSightDetailsID,
                        FileName = Path.GetFileName(fileNameWithExtension)//uploadedFileInfo.Name
                    };

                    db.MBL_SpeciesSightingUploadFile.Add(speciesFileEntity);
                    db.SaveChanges();
                }


                // Through the request response you can return an object to the Angular controller
                // You will be able to access this in the .success callback through its data attribute
                // If you want to send something to the .error callback, use the HttpStatusCode.BadRequest instead
                var returnData = "Success";
                return Ok(new { returnData });
            }
            catch (Exception ex)
            {
                var error = ex.InnerException.Message;                
                return BadRequest("Error saving species sight details - " + error);
            }


        }

        [ResponseType(typeof(MBL_SpeciesSightingDetails))]
        public async Task<IHttpActionResult> DeleteMBL_SpeciesSightingDetails(int id)
        {
            MBL_SpeciesSightingDetails mBL_SpeciesSightingDetails = await db.MBL_SpeciesSightingDetails.FindAsync(id);
            if (mBL_SpeciesSightingDetails == null)
            {
                return NotFound();
            }

            db.MBL_SpeciesSightingDetails.Remove(mBL_SpeciesSightingDetails);
            await db.SaveChangesAsync();

            return Ok(mBL_SpeciesSightingDetails);
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