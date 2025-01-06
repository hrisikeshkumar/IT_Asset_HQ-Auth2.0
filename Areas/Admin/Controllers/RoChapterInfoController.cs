using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IT_Hardware.Areas.Admin.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.ITManagers)]
    [Area("Admin")]
    public class RoChapterInfoController : Controller
    {     
        public ActionResult ChapterInfo(string chapterName)
        {
            RoChapterInfo_BL com = new RoChapterInfo_BL();

            RoChapterInfo_Mod data = new RoChapterInfo_Mod();

            data.ChapterList = new List<SelectListItem>();

            data.chapterDetail = com.Get_ChaptersData(chapterName);

            return View(data);
        }

    }
}
