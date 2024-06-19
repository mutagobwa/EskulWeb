using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class ExpenseTypeController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ExpenseTypeController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(ExpenseType model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.expenseTypes = await _myUtilities.LoadExpenseTypes(true);



            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: ExpenseTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExpenseTypeController/Create
        public async Task<ActionResult> Create(ExpenseType model,int Id)
        {
            try
            {

                string resp = "";
                string Url = "AccountsAndFinance/GenerateExpenseTypeCode";
                resp = await request.GetB(Url);
                model.ExpenseCode = resp.Split('\"')[3];
                return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ExpenseTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExpenseType model)
        {
            Url = "AccountsAndFinance/AddExpenseType";
            string resp = "";
            try
            {
                var Exists = await _myUtilities.LoadExpenseType(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "AccountsAndFinance/UpdateExpenseType";
                    resp = await request.Update<ExpenseType>(model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + " " + resp;
                    }
                }
                else
                {
                    Url = "AccountsAndFinance/AddExpenseType";
                    resp = await request.Add<ExpenseType>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ExpenseTypeController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string resp = "";
            string EditUrl = "AccountsAndFinance/ExpenseType/" + id + "";
            var model = new ExpenseType();
            try
            {
                var c = await request.Get<ExpenseType>(EditUrl);
                model.ExpenseCode = c.FirstOrDefault().ExpenseCode;
                model.ExpenseName = c.FirstOrDefault().ExpenseName;
                model.ExpenseDesc = c.FirstOrDefault().ExpenseDesc;

                model.delete = false;
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }

        // POST: ExpenseTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExpenseTypeController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string resp = "";
            string UpdateUrl = "AccountsAndFinance/UpdateExpenseType";
            string EditUrl = "AccountsAndFinance/ExpenseType/" + id + "";
            var model = new ExpenseType();
            try
            {
                var c = await request.Get<ExpenseType>(EditUrl);
                model.ExpenseCode = c.FirstOrDefault().ExpenseCode;
                model.ExpenseName = c.FirstOrDefault().ExpenseName;
                model.ExpenseDesc = c.FirstOrDefault().ExpenseDesc;
                model.delete = true;
                resp = await request.Update<ExpenseType>(model, UpdateUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: ExpenseTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
       
    }
}
