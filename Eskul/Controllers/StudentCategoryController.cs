﻿using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class StudentCategoryController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public StudentCategoryController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(StudentCategory model,int id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                if (id != 0)
                {
                    ApiResponse respons = await _myUtilities.LoadStudentCat(id);
                    if (respons.Success)
                    {
                        model = JsonConvert.DeserializeObject<List<StudentCategory>>(respons.PayLoad);
                    }
                }
                ApiResponse response = await _myUtilities.LoadStudentCats();
                if (response.Success)
                {
                    model.studentCategories = JsonConvert.DeserializeObject<List<StudentCategory>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else if (response.ResponseCode == 500)
                {
                    TempData["error"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = "Response Unkown";
                }


            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin";
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }
        // POST: StudentCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentCategory model)
        {
            Url = "Students/Categories";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                if (model.StatusId == 0) { model.StatusId = 3; }
                //if (string.IsNullOrEmpty(model.Code)) { model.Code = "00000"; }
                resp = await request.AddAsync<StudentCategory>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: StudentCategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            
            return RedirectToAction(nameof(Index), new {id=id});
        }
        // GET: StudentCategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = $"Students/Categories/{SessionData.ClientCode}/{id}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: StudentCategoryController/Delete/5
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
