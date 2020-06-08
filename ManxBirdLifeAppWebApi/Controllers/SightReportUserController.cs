using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ManxBirdLifeAppWebApi.Data;

namespace ManxBirdLifeAppWebApi.Controllers
{
    public class SightReportUserController : ApiController
    {
        private ManxBirdLifeAppEntities db = new ManxBirdLifeAppEntities();

        public IQueryable<MBL_SightReportUser> GetMBL_SightReportUser()
        {
            return db.MBL_SightReportUser;
        }

        [ResponseType(typeof(MBL_SightReportUser))]
        public async Task<IHttpActionResult> GetMBL_SightReportUser(int id)
        {
            MBL_SightReportUser mBL_SightReportUser = await db.MBL_SightReportUser.FindAsync(id);
            if (mBL_SightReportUser == null)
            {
                return NotFound();
            }

            return Ok(mBL_SightReportUser);
        }

       
        [ResponseType(typeof(MBL_SightReportUser))]
        public async Task<IHttpActionResult> PostMBL_SightReportUser(MBL_SightReportUser mBL_SightReportUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MBL_SightReportUser.Add(mBL_SightReportUser);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mBL_SightReportUser.SightReportUserID }, mBL_SightReportUser);
        }


        [ResponseType(typeof(MBL_SightReportUser))]
        public async Task<IHttpActionResult> DeleteMBL_SightReportUser(int id)
        {
            MBL_SightReportUser mBL_SightReportUser = await db.MBL_SightReportUser.FindAsync(id);
            if (mBL_SightReportUser == null)
            {
                return NotFound();
            }

            db.MBL_SightReportUser.Remove(mBL_SightReportUser);
            await db.SaveChangesAsync();

            return Ok(mBL_SightReportUser);
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