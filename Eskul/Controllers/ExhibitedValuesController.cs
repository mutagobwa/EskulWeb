using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class ExhibitedValuesController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ExhibitedValuesController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(ExhibitedValues model, string Level,int Class,string Stream)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp = null;
                resp = await _myUtilities.LoadClassess();

                if (resp != null && resp.Success)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadClassStream(model.Class);

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadValueDefinitions();

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<ExhibitedValues> classLists = JsonConvert.DeserializeObject<List<ExhibitedValues>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.valueDescription, Value = p.valueId.ToString() }).ToList();

                    ViewBag.ValueDefinitions = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                if (model.Class != 0 || Class != 0)
                {
                    ApiResponse res = await _myUtilities.LoadStudentsExhibtedValues(model.Level ?? "O", model.Class, model.Stream);
                    if (res!=null && res.Success && res.ResponseCode == 100)
                    {
                        model.exhibitedValues = JsonConvert.DeserializeObject<List<ExhibitedValues>>(res.PayLoad);
                    }
                    else if (res?.ResponseCode == 101)
                    {
                        TempData["info"] = res.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = res?.ResponseMessage;
                    }
                }
                else
                {
                    var model1 = new ExhibitedValues();
                    return View(model1);
                }
            }
            catch (Exception ex)
            {
               _logger.Error(ex.Message, ex);
               TempData["error"] ="Error Occured Please Contact Admin";
            }
            return View(model);
        }

        // GET: ExhibitedValuesController/Details/5
        public async Task<ActionResult> Details(ExhibitedValues model, string id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp = null;
                resp = await _myUtilities.LoadClassess();

                if (resp != null && resp.Success)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadClassStream(model.Class);

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadValueDefinitions();

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<ExhibitedValues> classLists = JsonConvert.DeserializeObject<List<ExhibitedValues>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.valueDescription, Value = p.valueId.ToString() }).ToList();

                    ViewBag.ValueDefinitions = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
               ApiResponse response = await _myUtilities.LoadStudentExhibitedValues(id);
                if (response != null && response.Success && response.ResponseCode == 100)
                {
                    model.exhibitedValues = JsonConvert.DeserializeObject<List<ExhibitedValues>>(response.PayLoad);
                    model.Stream = model.Stream;
                    model.Class = model.Class;
                }
                else if (response?.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response?.ResponseMessage;
                }
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
            }
            return (View(model));
        }
        // POST: ExhibitedValuesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExhibitedValues model)
        {
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                Url = "Academics/StudentValues/Exhibited/Save";
                
                model.schoolCode = SessionData.ClientCode;
                model.year = DateTime.Now.Year.ToString();
                model.Term = SessionData.Term;
                model.statusId = 3;
                resp = await request.Add<ExhibitedValues>(model,Url);
                if (resp.Contains("successfully"))
                { TempData["success"] = resp; }
                else { TempData["error"] = "Error Occured " + resp; }
                return model.studentValueId == 0.0 ? RedirectToAction("Index",new{
                    Level = model.Level,Class = model.Class, Stream = model.Stream
                }) : RedirectToAction("Details",new{id = model.StudentId});
            }
            catch (Exception ex)
            {
               _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured" + resp;
                return RedirectToAction("Index");
            }
        }

        // GET: ExhibitedValuesController/Edit/5
        public async Task<ActionResult> Edit(double id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            try
            {
                var model = new ExhibitedValues();
                ApiResponse response= await _myUtilities.LoadStudentExhibitedValue(id);
                return Json(model);
            }
            catch (Exception ex)
            {
              _logger.Error(ex.Message, ex);
             TempData["error"] = (object)("Error Occured" + resp);
                return RedirectToAction("Index");
            }
        }
        // GET: ExhibitedValuesController/Delete/5
        public async Task<ActionResult> Delete(double id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string json = "";
            Url = "Academics/StudentValues/Category/Save";
            ExhibitedValues model = new ExhibitedValues();
            try
            {
                ApiResponse response = await _myUtilities.LoadStudentExhibitedValue(id);
                model.schoolCode = SessionData.ClientCode;
                model.statusId = 5;
                string str = await request.Add<ExhibitedValues>(model, ExhibitedValuesController.Url);
                if (str.Contains("successfully"))
                {
                    var data = new { status = 200, res = str };
                    json = JsonConvert.SerializeObject(data);
                    return RedirectToAction("Details",new{id = model.StudentId});}
                var data1 = new { status = 201, res = str };
                json = JsonConvert.SerializeObject(data1);
                return (Content(json, "application/json"));
            }
            catch (Exception ex)
            {
                TempData["error"] = (object)"Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
            }
        }
        public async Task<ActionResult> StreamsByClass(int classs)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";

                object model = "";

                if (classs != 0)
                {
                    var req = await _myUtilities.LoadClassStream(classs);
                    model = JsonConvert.DeserializeObject<List<streamList>>(req.PayLoad);
                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model);
                    return Content(json, "application/json");
                }

                else
                {
                    return Content(JsonConvert.SerializeObject(new { status = 404, res = "Error" }), "application/json");
                }
            }
            catch (Exception ex)
            {

                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");

            }

        }


        public async Task<ActionResult> ClassByLevel(string Level)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";

                object model = "";

                if (Level != null)
                {

                    model = await _myUtilities.LoadClasses(Level);

                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model);
                    return Content(json, "application/json");
                }

                else
                {
                    return Content(JsonConvert.SerializeObject(new { status = 404, res = "Error" }), "application/json");
                }
            }
            catch (Exception ex)
            {

                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");

            }

        }
    }
}
