using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{

    [Authorize(Policy = AuthorizationPolicies.ITManagers)]
    [Area("Admin")]
    public class Item_IssueController : Controller
    {
    
        public ActionResult Item_Issue_Details(string PO_Id)
        {

            if (PO_Id is null)
                PO_Id = string.Empty;

            ItemIssue_Mod model = new ItemIssue_Mod();

            BL_Item_Issue item = new BL_Item_Issue();

            if (PO_Id != string.Empty)
            {
                model.Item_Issues = item.Get_Item_By_Sl(PO_Id, "PONo_Wise");
            }
            else
            {
                model.Item_Issues = item.Get_Item_IssueData();
            }
                

            return View(model);
        }

        [HttpGet]
        public ActionResult Item_Issue_Create_Item(string Message)
        {
            ViewBag.Message = Message;

            return View();
        }

        [HttpPost]
        public ActionResult Item_Issue_Create_Post(Mod_Item_Issue Get_Data)
        {


            string Message = "";
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {
                    BL_Item_Issue save_data = new BL_Item_Issue();
                    int status = save_data.Save_Item_Issue_data(Get_Data, "Add_new", "");

                    if (status < 1)
                    {
                        TempData["Message"] = String.Format("Data is not saved");
                    }
                    else
                    {
                        TempData["Message"] = String.Format("Data save successfully");
                    }
                }
                else
                {
                    TempData["Message"] = String.Format("Required field are not provided");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("Item_Issue_Create_Item", "Item_Issue");
        }
     
        public ActionResult Edit_Item_Issue(string id)
        {
            BL_Item_Issue Md_Com = new BL_Item_Issue();
            Mod_Item_Issue data = Md_Com.Get_Data_By_ID(id);

            return View( data);
        }
       
        public ActionResult Update_Item_Issue(Mod_Item_Issue Get_Data, string Asset_ID)
        {
            int status = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    BL_Item_Issue Md_Asset = new BL_Item_Issue();

                    status = Md_Asset.Save_Item_Issue_data(Get_Data, "Update", Asset_ID);

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data have saved successfully");
                    }
                    else
                    {
                        TempData["Message"] = String.Format("Data is not saved");
                    }
                }
                else
                {
                    TempData["Message"] = String.Format("Required Data are not Provided");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("Item_Issue_Details", "Item_Issue");
        }
    
        public ActionResult Delete_Item_Issue(Mod_Item_Issue Get_Data, string id)
        {
            int status = 0;
            try
            {

                if (ModelState.IsValid)
                {


                    BL_Item_Issue Md_Asset = new BL_Item_Issue();

                    status = Md_Asset.Save_Item_Issue_data(Get_Data, "Delete", id);

                    if (status == 1)
                    {
                        TempData["Message"] = String.Format("Data saved successfully");
                    }
                    else
                    {
                        TempData["Message"] = String.Format("Data is not saved");
                    }
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("Item_Issue_Details", "Item_Issue");
        }


        [HttpPost]
        public JsonResult AutoComplete(string SL_No)
        {

            BL_Item_Issue data = new BL_Item_Issue();
            List<Item_SL_Wise> list = data.Item_SLnumber_List(SL_No);


            return Json(list);
        }


        
        [HttpPost]
        public JsonResult Find_Item_Issue(string Item_Id)
        {

            Mod_Item_Issue_Employee Emp_Details= new Mod_Item_Issue_Employee();

            BL_Item_Issue Item_data = new BL_Item_Issue();

            Emp_Details = Item_data.Issue_Employee(Emp_Details, Item_Id, "Item_Issue");

            return Json(Emp_Details);

        }

        
        [HttpPost]
        public JsonResult AutoComplete_TransferEmployee(string EmpID)
        {

            BL_Item_Issue data = new BL_Item_Issue();
            List<Mod_Item_Issue_Employee> list = data.Emp_List(EmpID);

            return Json(list);
        }


        public JsonResult Search_Item(string SearchVal, string SearchType)
        {

            BL_Item_Issue com = new BL_Item_Issue();

            return Json(com.Get_Item_By_Sl( SearchVal,  SearchType));

        }


       

    }
}