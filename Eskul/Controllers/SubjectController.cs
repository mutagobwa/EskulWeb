using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;
using static iText.Signatures.LtvVerification;

namespace Eskul.Controllers
{
    public class SubjectController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SubjectController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;   
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: SubjectController
        public async Task<ActionResult> Index(SchoolSubject model, string Level="O")   
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                
                if (!string.IsNullOrEmpty(Level))
                {
                    ApiResponse res = await _myUtilities.LoadSchoolSubjects(Level);
                    if (res.ResponseCode == 100 && res.PayLoad != null)
                    {
                        model.SchoolSubjects = JsonConvert.DeserializeObject<List<SchoolSubject>>(res.PayLoad);
                    }

                    
                        model.Level = Level;
                    
                }
                else
                {

                     model = new SchoolSubject();
                    if (Request.Form.ContainsKey("Level"))
                    {
                        model.Level = Request.Form["Level"];
                    }
                    return View(model);
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";

            }
            return View(model);
        }
        // POST: SubjectController/Create
        [HttpPost]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            List<_SchoolSubject> model = new List<_SchoolSubject>();

            // Process paper checkboxes
            foreach (string key in collection.Keys)
            {
                if (key.StartsWith("paper_"))
                {
                    string paperCode = key.Substring("paper_".Length);
                    string subjectCode = paperCode.Split('/')[0];
                    var schoolSubject = model.FirstOrDefault(s => s.SubjectCode == subjectCode);

                    if (schoolSubject == null)
                    {
                        bool isOffered = collection.ContainsKey($"isOffered_{subjectCode}") && collection[$"isOffered_{subjectCode}"].ToString().Split(',').Contains("on");
                        bool isCompulsory = collection.ContainsKey($"isCompulsory_{subjectCode}") && collection[$"isCompulsory_{subjectCode}"].ToString().Split(',').Contains("on");

                        schoolSubject = new _SchoolSubject()
                        {
                            SubjectCode = subjectCode,
                            isOffered = isOffered,
                            IsCompulsory = isCompulsory,
                            SubjectPapers = new List<SchoolSubjectPaper>()
                        };
                        model.Add(schoolSubject);
                    }

                    bool isChecked = collection[key].ToString().Split(',').Contains("on");
                    SchoolSubjectPaper schoolSubjectPaper = new SchoolSubjectPaper()
                    {
                        SubjectCode = subjectCode,
                        PaperCode = paperCode,
                        StatusId = isChecked ? 3 : 5
                    };
                    schoolSubject.SubjectPapers.Add(schoolSubjectPaper);
                }
            }

            // Ensure subject checkboxes are processed even if no papers are checked
            foreach (string key in collection.Keys)
            {
                if (key.StartsWith("isOffered_"))
                {
                    string subjectCode = key.Substring("isOffered_".Length);
                    var schoolSubject = model.FirstOrDefault(s => s.SubjectCode == subjectCode);

                    if (schoolSubject == null)
                    {
                        bool isOffered = collection[key].ToString().Split(',').Contains("on");
                        bool isCompulsory = collection.ContainsKey($"isCompulsory_{subjectCode}") && collection[$"isCompulsory_{subjectCode}"].ToString().Split(',').Contains("on");

                        schoolSubject = new _SchoolSubject()
                        {
                            SubjectCode = subjectCode,
                            isOffered = isOffered,
                            IsCompulsory = isCompulsory,
                            SubjectPapers = new List<SchoolSubjectPaper>()
                        };
                        model.Add(schoolSubject);
                    }
                }
                else if (key.StartsWith("isCompulsory_"))
                {
                    string subjectCode = key.Substring("isCompulsory_".Length);
                    var schoolSubject = model.FirstOrDefault(s => s.SubjectCode == subjectCode);

                    if (schoolSubject == null)
                    {
                        bool isOffered = collection.ContainsKey($"isOffered_{subjectCode}") && collection[$"isOffered_{subjectCode}"].ToString().Split(',').Contains("on");
                        bool isCompulsory = collection[key].ToString().Split(',').Contains("on");

                        schoolSubject = new _SchoolSubject()
                        {
                            SubjectCode = subjectCode,
                            isOffered = isOffered,
                            IsCompulsory = isCompulsory,
                            SubjectPapers = new List<SchoolSubjectPaper>()
                        };
                        model.Add(schoolSubject);
                    }
                    else
                    {
                        schoolSubject.IsCompulsory = collection[key].ToString().Split(',').Contains("on");
                    }
                }
            }

            List<_SchoolSubject> schoolSubjectList = new List<_SchoolSubject>();

            foreach (_SchoolSubject schoolSubject in model)
            {
                string subjectCode = schoolSubject.SubjectCode;
                bool isOffered = collection.ContainsKey($"isOffered_{subjectCode}") && collection[$"isOffered_{subjectCode}"].ToString().Split(',').Contains("on");
                bool isCompulsory = collection.ContainsKey($"isCompulsory_{subjectCode}") && collection[$"isCompulsory_{subjectCode}"].ToString().Split(',').Contains("on");

                schoolSubjectList.Add(new _SchoolSubject()
                {
                    SubjectCode = schoolSubject.SubjectCode,
                    SubjectName = schoolSubject.SubjectName,
                    SubjectPapers = schoolSubject.SubjectPapers,
                    StatusId = isOffered ? 3 : 5,
                    SchoolCode = SessionData.ClientCode,
                    isOffered = isOffered,
                    IsCompulsory = isCompulsory
                });
            }

            var json = "";
            string Url = "Settings/Subjects";
            ApiResponse resp = await request.AddAsync<List<_SchoolSubject>>(schoolSubjectList, Url);
            if (resp != null && resp.ResponseCode == 100)
            {
                var response = new
                {
                    status = resp.ResponseCode,
                    res = resp.ResponseMessage
                };

                json = JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = 101,
                    res = resp.ResponseMessage
                };

                json = JsonConvert.SerializeObject(response);
            }

            return Content(json, "application/json");
        }


        // GET: SubjectController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string EditUrl = "Academics/SubjectByCode/" + id + "";
            var model = new subject();
            //model.subjectLists = await LoadSubjects(true);
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<subjectList>(EditUrl);

                model.Compulsory = c.FirstOrDefault().Compulsory;
                model.Examlevel = c.FirstOrDefault().Examlevel;
                model.Subjectcode = c.FirstOrDefault().Subject_code;
                model.Subjectname = c.FirstOrDefault().Subject_name;
                model.Offered = c.FirstOrDefault().Offered;
                model.subjectAbbrev = c.FirstOrDefault().SubjectAbbrev;
                model.delete = false;
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index),model);

        }

        // POST: SubjectController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(subjectpaper model)
        {
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }

                string EditUrl = "Academics/UpdateSubject/";

                resp = await request.Update<subjectpaper>(model, EditUrl);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;

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

        // GET: SubjectController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var json = "";
            string resp = "";
            string EditUrl = "Academics/UpdateSubject/";
            string Getsub = "Academics/SubjectByCode/" + id + "";
            var model = new subject();
            //model.subjectLists = await LoadSubjects(true);
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<subjectList>(Getsub);

                model.Compulsory = c.FirstOrDefault().Compulsory;
                model.Examlevel = c.FirstOrDefault().Examlevel;
                model.Subjectcode = c.FirstOrDefault().Subject_code;
                model.Subjectname = c.FirstOrDefault().Subject_name;
                model.Offered = c.FirstOrDefault().Offered;
                model.subjectAbbrev = c.FirstOrDefault().SubjectAbbrev;
                model.delete = true;
                resp = await request.Update<subject>(model, EditUrl);
                if (resp.Contains("successfully"))
                {
                    var data = new { status = 200, res = resp };
                    json = JsonConvert.SerializeObject(data);

                }
                else
                {
                    var data = new { status = 201, res = resp };
                    json = JsonConvert.SerializeObject(data);
                }
               
                return Content(json, "application/json");

            }
            catch (Exception ex)
            {

                var data = new { status = 201, message = ex };
                json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }
        }


        // POST: SubjectController/Delete/5
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
