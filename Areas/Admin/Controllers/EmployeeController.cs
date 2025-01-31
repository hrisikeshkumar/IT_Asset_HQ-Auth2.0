using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{

    [Authorize(Policy = AuthorizationPolicies.ITManagers)]
    [Area("Admin")]
    public class EmployeeController : Controller
    {

        
        public ActionResult Employee_Details()
        {
            BL_Employee data = new BL_Employee(); 

            List<Mod_Employee> Emp_List = data.Get_EmployeeData();

            return View( Emp_List);
        }


        [HttpGet]
        public ActionResult Create_Employee(string Message)
        {
            ViewBag.Message = Message;

            BL_Employee data = new BL_Employee();

            Mod_Employee Mod_emp = new Mod_Employee();

            Mod_emp.Emp_Type_List = data.Bind_EmpType();

            Mod_emp.Dept_List = data.Bind_Dept();

            return View( Mod_emp);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Employee_Create_Post(Mod_Employee Get_Data)
        {
            string Message = "";
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {
                    BL_Employee save_data = new BL_Employee();
                    int status = save_data.Save_Employee_data(Get_Data, "Add_new");

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
                    TempData["Message"] = String.Format("Data not Entered Properly");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("Create_Employee", "Employee");
        }


        public ActionResult Edit_Employee(string id)
        {
            BL_Employee Emp_Data = new BL_Employee();
            Mod_Employee Mod_emp = new Mod_Employee();

            Mod_emp.Emp_Type_List = Emp_Data.Bind_EmpType();
            Mod_emp.Dept_List = Emp_Data.Bind_Dept();
           
            Mod_emp = Emp_Data.Get_Data_By_ID(id, Mod_emp);

            Mod_emp.Designation_List = Emp_Data.Bind_Designation(Mod_emp.Emp_Type);

            return View(  Mod_emp);
        }


        [HttpPost]
        public ActionResult Update_Employee(Mod_Employee Get_Data)
        {
            int status = 0;
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {
                    BL_Employee Md_Emp = new BL_Employee();

                    status = Md_Emp.Save_Employee_data(Get_Data, "Update");

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data have saved successfully");
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

            return RedirectToAction("Employee_Details", "Employee");
        }



        public ActionResult Delete_Employee(Mod_Employee Get_Data)
        {
            int status = 0;
            try
            {

                if (ModelState.IsValid)
                {
                    BL_Employee Md_Emp = new BL_Employee();
                    status = Md_Emp.Save_Employee_data(Get_Data, "Delete");

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

            return RedirectToAction("Employee_Details", "Employee");
        }



        
        public JsonResult Get_Designation(string Emp_Type)
        {
            BL_Employee data = new BL_Employee();
            Mod_Employee Mod_emp = new Mod_Employee();
            Mod_emp.Designation_List = data.Bind_Designation(Emp_Type);
            return Json(Mod_emp.Designation_List);
          
        }

    }
}