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
        public ActionResult ChapterInfo(RoChapterInfo_Mod data)
        {
            RoChapterInfo_BL com = new RoChapterInfo_BL();

            if (data.ChapterName == null)
                data.ChapterName = "Agra";

            data.ChapterList = com.Get_AllOfficeList();
            data.chapterDetail = com.Get_ChaptersData(data.ChapterName);

            return View(data);
        }


    }
}
