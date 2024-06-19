using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Nancy;
using Newtonsoft.Json;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Security;
using SmartPaperEdms.Web.App_Code;
using System.Reflection.Metadata;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class AdmissionController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: AdmissionController
        public AdmissionController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(StudentViewDTO model)
       {
            try
            {

                if (!SessionData.IsSignedIn) {  return RedirectToAction("Index", "Login"); }

                ApiResponse response;

                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response != null && response.ResponseCode == 100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadClassStream(model.Class);

                if (response != null && response.Success)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadBranches();

                if (response != null && response.Success)
                {
                    List<Branch> classLists = JsonConvert.DeserializeObject<List<Branch>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();

                    ViewBag.Branches = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Branches = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadHouses();

                if (response != null && response.Success)
                {
                    List<HouseVm> HouseLists = JsonConvert.DeserializeObject<List<HouseVm>>(response.PayLoad);
                    var l = (from p in HouseLists select new ListItems { Text = p.Name, Value = p.Code.ToString() }).ToList();

                    ViewBag.Houses = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Houses = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadStudentCats();

                if (response != null && response.Success)
                {
                    List<StudentCategory> CatLists = JsonConvert.DeserializeObject<List<StudentCategory>>(response.PayLoad);
                    var l = (from p in CatLists select new ListItems { Text = p.categoryName, Value = p.categoryId.ToString() }).ToList();

                    ViewBag.Cats = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Cats = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadDisTypesAsync();

                if (response != null && response.Success)
                {
                    List<disabileReason> rsnLists = JsonConvert.DeserializeObject<List<disabileReason>>(response.PayLoad);
                    var l = (from p in rsnLists select new ListItems { Text = p.reasonName, Value = p.reasonId.ToString() }).ToList();

                    ViewBag.DisableReason = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.DisableReason = LoadListItems(l, true);
                }
                
                if (!string.IsNullOrEmpty(model.Level))
                {
                    ApiResponse res = await _myUtilities.LoadStudentsAsync(model.Level, model.Class, model.StreamCode??"0",false,true);
                    if (res.ResponseCode==100 && res.PayLoad!=null)
                    {
                        model.students= JsonConvert.DeserializeObject<List<StudentViewDTO>>(res.PayLoad);
                    }

                }
                else
                {

                    var model1 = new StudentViewDTO();
                    model1.BranchId = SessionData.UserBranchId;
                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;

            }
            return View(model);
        }

        // GET: AdmissionController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }


            var model = new StudentViewDTO();
            try
            {
                ApiResponse res = await _myUtilities.LoadStudentAsync(id,true);

                if (res.ResponseCode == 100 && res.PayLoad != null)
                {
                    model = JsonConvert.DeserializeObject<StudentViewDTO>(res.PayLoad);
                    if (!string.IsNullOrEmpty(model?.Image?.Image64String))
                    {
                        ViewBag.StudImg = "data:image/png;base64," + model.Image.Image64String;
                    }
                    if (model?.Parents != null && model.Parents.Parents != null && model.Parents.Parents.Count > 0)

                    {
                        //foreach (var parent in model.Parents.Parents) { ViewBag.Parents.Add(parent);}
                        foreach (var parent in model.Parents.Parents) {
                            if (!string.IsNullOrEmpty(model?.Parents?.Parents?.First()?.Parent?.Image?.Image64String))
                            {
                                ViewBag.PrtImg = "data:image/png;base64," + model?.Parents?.Parents?.First()?.Parent?.Image?.Image64String;
                            }
                            
                            ;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return View();
            }
            return View(model);
        }
        public async Task<ActionResult> Disable(DisabledStudent model, string StudentId,string Remarks,string enableRemarks)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            
            try
            {
                if (string.IsNullOrEmpty(enableRemarks)) { enableRemarks = "Null"; }
               Url = $"Students/{SessionData.ClientCode}/{StudentId}/{true}/{model.DisableReasonId}/{Remarks}/{SessionData.UserId}/{enableRemarks}";
                ApiResponse resp = await request.UpdateAsync<DisabledStudent>(model, Url);

                if (resp.ResponseCode == 100 && resp.PayLoad != null)
                {
                    model = JsonConvert.DeserializeObject<DisabledStudent>(resp.PayLoad);
                }
                if (resp.PayLoad!=null)
                {
                    if (resp.ResponseCode ==100)
                        TempData["success"] = resp.ResponseMessage;
                    else
                        TempData["error"] = "Error Occured Contact Admin ";
                }
                return RedirectToAction("Index", new { Class = model.Class, StreamCode = model.StreamCode, Level = model.Level });
            }
            catch (Exception ex)
            {
               _logger.Error(ex.Message, ex);
              TempData["error"] = "Error Occured Please Contact Admin";
                return RedirectToAction("Index", new { Class = model.Class, StreamCode = model.StreamCode, Level = model.Level });
            }
        }

        public async Task<ActionResult> Enable(DisabledStudent model, string StudentId, string Remarks,string enableRemarks)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
               
            try
            {
                if (model.DisableReasonId==null) { model.DisableReasonId = 1; }
                if (string.IsNullOrEmpty(Remarks)) { Remarks = "Null"; }

                Url = $"Students/{SessionData.ClientCode}/{StudentId}/{false}/{model.DisableReasonId}/{Remarks}/{SessionData.UserId}/{enableRemarks}";

                ApiResponse resp = await request.UpdateAsync<DisabledStudent>(model, Url);

                if (resp.ResponseCode == 100 && resp.PayLoad != null)
                {
                    model = JsonConvert.DeserializeObject<DisabledStudent>(resp.PayLoad);
                }
                if (resp.PayLoad != null)
                {
                    if (resp.ResponseCode == 100)
                        TempData["success"] = resp.ResponseMessage;
                    else
                        TempData["error"] = "Error Occured Contact Admin ";
                }
                return RedirectToAction("Index", new { Class = model.Class, StreamCode = model.StreamCode,Level=model.Level });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Please Contact Admin";
                return RedirectToAction("Index", new { Class = model.Class, StreamCode = model.StreamCode, Level = model.Level });
            }
        }
        // GET: AdmissionController/Create
        public async Task<ActionResult> Create(CreateStudentDTO model, IFormFileCollection files, int id)
        {

            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try {
                ApiResponse response = null;

                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response != null && response.ResponseCode == 100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadParents();

                if (response != null && response.ResponseCode == 100)
                {
                    List<ParentViewDTO> ParentLists = JsonConvert.DeserializeObject<List<ParentViewDTO>>(response.PayLoad);
                    var l = (from p in ParentLists select new ListItems { Text = p.FullName, Value = p.ParentId.ToString() }).ToList();

                    ViewBag.Parent = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Parent = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadClassStream(model.Class);

                if (response != null && response.Success)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadBranches();

                if (response != null && response.Success)
                {
                    List<Branch> classLists = JsonConvert.DeserializeObject<List<Branch>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();

                    ViewBag.Branches = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Branches = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadSubjectAsync(model.Level??"O");

                if (response != null && response.Success)
                {
                    List<subject> classLists = JsonConvert.DeserializeObject<List<subject>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Subjectname, Value = p.subjectAbbrev.ToString() }).ToList();

                    ViewBag.Subjects = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Subjects = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadHouses();

                if (response != null && response.Success)
                {
                    List<HouseVm> HouseLists = JsonConvert.DeserializeObject<List<HouseVm>>(response.PayLoad);
                    var l = (from p in HouseLists select new ListItems { Text = p.Name, Value = p.Code.ToString() }).ToList();

                    ViewBag.Houses = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Houses = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadStudentCats();

                if (response != null && response.Success)
                {
                    List<StudentCategory> CatLists = JsonConvert.DeserializeObject<List<StudentCategory>>(response.PayLoad);
                    var l = (from p in CatLists select new ListItems { Text = p.categoryName, Value = p.categoryId.ToString() }).ToList();

                    ViewBag.Cats = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Cats = LoadListItems(l, true);
                }
                
                if (model.BranchId == 0) { model.BranchId = SessionData.UserBranchId; }
                if (model.TermRegistered == 0) { model.TermRegistered = SessionData.Term; }
                List<SelectListItem> items = new List<SelectListItem>
    {
        new SelectListItem { Text = "Select", Value = "" },
        new SelectListItem { Text = "ENGLISH", Value = "ENG" },
        new SelectListItem { Text = "MATHEMATICS", Value = "MATH" },
        new SelectListItem { Text = "SCIENCE", Value = "SCI" },
        new SelectListItem { Text = "SOCIAL STUDIES", Value = "SST" }
    };
                ViewBag.Primary = items;
                List<SelectListItem> aggs = new List<SelectListItem>
    {
        new SelectListItem { Text = "Select", Value = "" },
        new SelectListItem { Text = "D1", Value = "D1" },
        new SelectListItem { Text = "D2", Value = "D2" },
        new SelectListItem { Text = "C3", Value = "C3" },
        new SelectListItem { Text = "C4", Value = "C4" },
        new SelectListItem { Text = "C5", Value = "C5" },
        new SelectListItem { Text = "C6", Value = "C6" },
        new SelectListItem { Text = "P7", Value = "P7" },
        new SelectListItem { Text = "P8", Value = "P8" },
        new SelectListItem { Text = "F9", Value = "F9" },
        new SelectListItem { Text = "X", Value = "X" }
    };
                ViewBag.aggs=aggs;
                ViewBag.DocTypes = new List<SelectListItem>
    {
        new SelectListItem{Text="Select", Value=""},
        new SelectListItem{Text="P.L.E SLIP", Value="A"},
        new SelectListItem{Text="U.A.C.ECERTIFICATE", Value="C"},
        new SelectListItem{Text="TRANSCRIPT", Value="D"},
        new SelectListItem{Text="U.C.E CERTIFICATE", Value="B"},
        new SelectListItem{Text="PASSPORT", Value="E"},
        new SelectListItem{Text="NATIONAL ID", Value="F"},
        new SelectListItem{Text="OTHER", Value="H"},
        new SelectListItem{Text="CERTIFICATE", Value="G"},
    };
                List<SelectListItem> relations = new List<SelectListItem>
    {
        new SelectListItem { Text = "Select", Value = "" },
        new SelectListItem { Text = "Father", Value = "1" },
        new SelectListItem { Text = "Mother", Value = "2" },
        new SelectListItem { Text = "Guardian", Value = "3" },
        new SelectListItem { Text = "Friend", Value = "4" },
        new SelectListItem { Text = "Other", Value = "5" }
    };
                ViewBag.Relations=relations;
                model.HouseInfo = new StudentHouse();
                model.FormerSchool = new FormerSchoolDtls();

                model.Documents = new StudentUploadedDocument();

                if (files.Count > 0)
                {
                    model.Image = new StudentImage();
                    model.Image.ImageType = 1;
                    model.Image.StudentNo = model.StudentNo??"";
                    model.Image.Image64String = model.Image.Image64String;
                }



                if (model.BranchId == 0)
                {
                    model.BranchId = SessionData.UserBranchId;
                }
                
                if (model.CategoryId == 0)
                {
                    model.CategoryId = 3;
                }
                
                if (model.TermRegistered == 0)
                {
                    model.TermRegistered = SessionData.Term;

                        return View(model);
                  

                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }

            return View(model);
        }

        // POST: AdmissionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection formCollection, IFormCollection collection, CreateStudentDTO model, IFormFileCollection files,
           IFormFileCollection docs, IFormFileCollection docs1, IFormFileCollection docs2, IFormFileCollection docs3, formerSchool model3, List<documents> model4)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            ApiResponse resp = null;
            Url = "Students";
            try
            {
                model.DateRegistered = DateTime.Now;
                var ddl1 = collection["ddl1"].ToString();
                var ddl2 = collection["ddl2"].ToString();
                var ddl3 = collection["ddl3"].ToString();
                var ddl4 = collection["ddl4"].ToString();
                var sbp = collection["sbp"].ToString();
                var sbp1 = collection["sbp1"].ToString();

                if (!string.IsNullOrEmpty(model.FormerSchool.SchoolName))
                {
                    model.FormerSchool.SchoolName = model.FormerSchool.SchoolName;
                    model.FormerSchool.AcademicAward = model.FormerSchool.AcademicAward;
                    model.FormerSchool.ResultCode = model.FormerSchool.ResultCode;
                    model.FormerSchool.ChangeReason = model.FormerSchool.ChangeReason;
                    model.FormerSchool.StudentId = model.StudentId??"";
                }
                else { model.FormerSchool = null; }

                List<FormerSchoolSubjectScore> Scores = new List<FormerSchoolSubjectScore>();

                for (int i = 0; i < 14; i++)
                {
                    var sbs = Request.Form[$"sbs{i}"].ToString();
                    var scs = Request.Form[$"scs{i}"].ToString();
                    if (string.IsNullOrEmpty(sbs) || string.IsNullOrEmpty(scs))
                    {
                        continue;
                    }

                    var prevschlscores = new FormerSchoolSubjectScore
                    {
                        Subject = sbs,
                        Score = scs
                    };

                    Scores.Add(prevschlscores);
                }
                if (Scores.Count>0)
                {
                    model.FormerSchool.Scores = Scores;
                }
                //else
                //{
                //    model.FormerSchool.Scores = new  List<FormerSchoolSubjectScore?>();
                //}
                string base64Pdf = "";
                byte[] PdfBytes = null;
                var ind = 0;
                List<DocumentFile> uploadedDocument = new List<DocumentFile>();

                if (docs.Count > 0)
                {

                    BinaryReader reader = new BinaryReader(docs.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);


                    // Add docs to the list.
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl1 /*, re = model.StudentNo, owner = "STD" */}
                    );
                    ind++;
                }
                if (docs1.Count > 0)
                {

                    BinaryReader reader = new BinaryReader(docs1.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs1.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl2 }
                    );
                    ind++;
                }
                if (docs2.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(docs2.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs2.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl3 }
                    );
                }
                if (docs3.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(docs3.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs3.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl4 }
                    );
                }
                if (uploadedDocument == null)
                {
                    model.Documents = new StudentUploadedDocument(); // Assuming Documents is a class, make sure to replace it with the actual class name
                }

                model.Documents = new StudentUploadedDocument();
                model.Documents.DocumentFiles = uploadedDocument;
                model.Documents.Owner = "STD";
                model.Documents.ReferenceNo = model.StudentId ?? "";


                if (model.HouseId != null)
                {
                    model.HouseInfo = new StudentHouse();
                    model.HouseInfo.HouseId = model.HouseId;
                    model.HouseInfo.StudentNo = model.StudentNo ?? "";
                }
                List<_StudentParentInfo> ParentInfo = new List<_StudentParentInfo>();

                for (int i = 0; i < 4; i++) 
                {
                    var prt = Request.Form[$"prt{i}"].ToString();
                    var  r = Request.Form[$"r{i}"].ToString();
                    if (string.IsNullOrEmpty(prt) || string.IsNullOrEmpty(r))
                    {
                        continue;
                    }

                    var newParentInfo = new _StudentParentInfo
                    {
                        ParentId = prt,
                        StudentId = model.StudentId??"",
                        RelationShip = int.Parse(r)
                    };

                    ParentInfo.Add(newParentInfo);
                }
                model.ParentInfo = ParentInfo;
                if (model.HealthInfo!=null) { model.HealthInfo.StudentId = model.StudentId;}
                string base64String = "";
                byte[] imageBytes = null;
                if (files.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(files.First().OpenReadStream());
                    imageBytes = reader.ReadBytes((int)files.First().Length);
                    base64String = Convert.ToBase64String(imageBytes);
                    model.Image = new StudentImage();
                    model.Image.Image64String = base64String;
                    model.Image.ImageType = 1;
                    model.Image.StudentNo = model.StudentNo ?? "";
                }

                if (model.BranchId == 0) { model.BranchId = SessionData.UserBranchId; }
                if (string.IsNullOrEmpty(model.StreamCode)) { model.StreamCode = "000"; }
                if (model.TermRegistered == 0) { model.TermRegistered = SessionData.Term; }
                if (string.IsNullOrEmpty(model.StudentId)) { model.StudentId = ""; }
                if (string.IsNullOrEmpty(model.StudentNo)) { model.StudentNo = ""; }

                if (string.IsNullOrEmpty(model.Type)) { model.Type = "L"; }
                if (model.CategoryId == 0) { model.CategoryId = 3; }
                if (string.IsNullOrEmpty(model.MiddleName)) { model.MiddleName = ""; }
                model.SchoolCode = SessionData.ClientCode;
                if (!string.IsNullOrEmpty(model.StudentId))
                {
                    Url = $"Students/{model.StudentId}";
                    if (string.IsNullOrEmpty(model.StreamCode)) { model.StreamCode = "000"; }
                    resp = await request.UpdateAsync<CreateStudentDTO>(model, Url);
                    if (resp.ResponseCode == 100)
                    {
                        TempData["success"] = resp.ResponseMessage;

                    }
                    else if (resp.ResponseCode == 101)
                    {
                        TempData["info"] = resp.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = resp.ResponseMessage;
                    }
                }
                else {
                    resp = await request.AddAsync<CreateStudentDTO>(model, Url);
                    if (resp != null && resp.ResponseCode == 100)
                    {
                        TempData["success"] = resp.ResponseMessage;
                    }
                    else if (resp != null && resp.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                    else { TempData["error"] = "Error Occured Contact Admin"; }
                }
                
                return RedirectToAction(nameof(Index), new {model.Level, model.Class,model.StreamCode});
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index), new { model.Level, model.Class, model.StreamCode });
            }
        }
        // GET: AdmissionController/QuickAdmission
        public async Task<ActionResult> QuickAdmission(StudentAdd model,int id)
        {
            ApiResponse response = null;
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {


                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response!=null && response.ResponseCode==100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadClassStream(model.Class);

                if (response != null && response.Success)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadBranches();

                if (response != null && response.Success)
                {
                    List<Branch> classLists = JsonConvert.DeserializeObject<List<Branch>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();

                    ViewBag.Branches = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Branches = LoadListItems(l, true);
                }

                if (model.BranchId == 0) { model.BranchId = SessionData.UserBranchId; }
                if (model.Termregistered == 0) { model.Termregistered = SessionData.Term;   }
                if (model.Studentid != null) { return View(model); }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> QuickAdmission(IFormCollection collection, CreateStudentDTO model, IFormFileCollection files,
       IFormFileCollection docs, IFormFileCollection docs1, IFormFileCollection docs2, IFormFileCollection docs3, formerSchool model3, List<documents> model4)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = "Students";
            ApiResponse resp = null;
            try
            {
                model.DateRegistered = DateTime.Now;
                var ddl1 = collection["ddl1"].ToString();
                var ddl2 = collection["ddl2"].ToString();
                var ddl3 = collection["ddl3"].ToString();
                var ddl4 = collection["ddl4"].ToString();
                var sbp = collection["sbp"].ToString();
                var sbp1 = collection["sbp1"].ToString();
               

                //model.Dateregistered = DateTime.Now;

                if (model.FormerSchool!= null)
                {
                    model.FormerSchool = new FormerSchoolDtls();
                    model.FormerSchool.SchoolName = model.FormerSchool.SchoolName;
                    model.FormerSchool.AcademicAward = model.AcademicAward;
                    model.FormerSchool.ResultCode = model.ResultCode;
                    model.FormerSchool.ChangeReason = model.FormerSchool.ChangeReason;
                    model.FormerSchool.StudentId = model.StudentId;
                }
                List<Scores> Scores = new List<Scores>();
                if (Scores.Count > 0)
                {
                    var sco = 0;
                    Scores.Add(new Models.Scores() { Subject = sbp1, score = ddl1 }
                    );
                    sco++;
                }
                else
                {

                }
                string base64Pdf = "";
                byte[] PdfBytes = null;
                var ind = 0;
                List<DocumentFile> uploadedDocument = new List<DocumentFile>();

                if (docs.Count > 0)
                {

                    BinaryReader reader = new BinaryReader(docs.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);


                    // Add docs to the list.
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl1 /*, re = model.StudentNo, owner = "STD" */}
                    );
                    ind++;
                }
                if (docs1.Count > 0)
                {

                    BinaryReader reader = new BinaryReader(docs1.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs1.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl2 }
                    );
                    ind++;
                }
                if (docs2.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(docs2.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs2.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl3 }
                    );
                }
                if (docs3.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(docs3.First().OpenReadStream());
                    PdfBytes = reader.ReadBytes((int)docs3.First().Length);
                    base64Pdf = Convert.ToBase64String(PdfBytes);
                    uploadedDocument.Add(new DocumentFile() { DocumentStream = base64Pdf, DocumentypeCode = ddl4 }
                    );
                }
                if (model.Documents == null)
                {
                    model.Documents = new StudentUploadedDocument(); // Assuming Documents is a class, make sure to replace it with the actual class name
                }

                // Assign the DocumentFiles property
                model.Documents.DocumentFiles = uploadedDocument;
                model.Documents.Owner = "STD";
                model.Documents.ReferenceNo = model.StudentId ?? "";

                
                if (model.HouseInfo != null)
                {
                    model.HouseInfo = new StudentHouse();
                    model.HouseInfo.HouseId = model.HouseId;
                    model.HouseInfo.StudentNo = model.StudentNo??"";
                }
                List<_StudentParentInfo> parents = new List<_StudentParentInfo>();
                if (model.ParentInfo != null) {
                    parents.Add(new _StudentParentInfo { ParentId = "", StudentId = "", RelationShip =0});
                };
                model.ParentInfo= parents;
                string base64String = "";
                byte[] imageBytes = null;
                if (files.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(files.First().OpenReadStream());
                    imageBytes = reader.ReadBytes((int)files.First().Length);
                    base64String = Convert.ToBase64String(imageBytes);
                    model.Image = new StudentImage();
                    model.Image.Image64String = base64String;
                    model.Image.ImageType = 1;
                    model.Image.StudentNo = model.StudentNo??"";
                }

                if (model.BranchId == 0) { model.BranchId = SessionData.UserBranchId; }
                if (string.IsNullOrEmpty(model.StreamCode)) { model.StreamCode = "000"; }
                if (model.TermRegistered == 0) { model.TermRegistered = SessionData.Term; }
                if (string.IsNullOrEmpty(model.StudentId)) { model.StudentId = ""; }
                if (string.IsNullOrEmpty(model.StudentNo)) { model.StudentNo = ""; }

                if (string.IsNullOrEmpty(model.Type)) { model.Type = "L"; }
                if (model.CategoryId == 0) { model.CategoryId = 3; }
                if (string.IsNullOrEmpty(model.MiddleName)) { model.MiddleName = ""; }
                //model.Statusid = 3;
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.AddAsync<CreateStudentDTO>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;
                }
                else if (resp.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                else { TempData["error"] = resp.ResponseMessage; }
                return RedirectToAction(nameof(QuickAdmission));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(QuickAdmission));
            }
        }
        public async Task<ActionResult> Edit(string id)
        {
            var model = new StudentViewDTO();
            try { 
                  if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                 
                 ApiResponse res = await _myUtilities.LoadStudentAsync(id,true);
                
                if (res.ResponseCode==100 && res.PayLoad!=null) 
                {
                  model= JsonConvert.DeserializeObject<StudentViewDTO>(res.PayLoad);
                    if (!string.IsNullOrEmpty(model?.Image?.Image64String))
                    {
                        ViewBag.StudImg = "data:image/png;base64," + model.Image.Image64String;
                    }
                    model.StudentId= id;

                }
                else if (res.ResponseCode == 101)
                {
                    TempData["info"] = res.ResponseMessage;
                }
                else if (res.ResponseCode == 500)
                {
                    TempData["error"] = "Error Occured Contact Admin";
                }
               

                    ApiResponse response = null;

                    response = await _myUtilities.LoadClassess(model.Level ?? "O");

                    if (response != null && response.ResponseCode == 100)
                    {
                        List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                        var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                        ViewBag.Classes = LoadListItems(l, true);
                    }
                    else
                    {
                        var l = new List<ListItems>();
                        ViewBag.Classes = LoadListItems(l, true);
                    }

                    response = await _myUtilities.LoadClassStream(model.Class);

                    if (response != null && response.Success)
                    {
                        List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
                        var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                        ViewBag.Streams = LoadListItems(l, true);
                    }
                    else
                    {
                        var l = new List<ListItems>();
                        ViewBag.Streams = LoadListItems(l, true);
                    }
                    response = await _myUtilities.LoadBranches();

                    if (response != null && response.Success)
                    {
                        List<Branch> classLists = JsonConvert.DeserializeObject<List<Branch>>(response.PayLoad);
                        var l = (from p in classLists select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();

                        ViewBag.Branches = LoadListItems(l, true);
                    }
                    else
                    {
                        var l = new List<ListItems>();
                        ViewBag.Branches = LoadListItems(l, true);
                    }
                    response = await _myUtilities.LoadHouses();

                    if (response != null && response.Success)
                    {
                        List<HouseVm> HouseLists = JsonConvert.DeserializeObject<List<HouseVm>>(response.PayLoad);
                        var l = (from p in HouseLists select new ListItems { Text = p.Name, Value = p.Code.ToString() }).ToList();

                        ViewBag.Houses = LoadListItems(l, true);
                    }
                    else
                    {
                        var l = new List<ListItems>();
                        ViewBag.Houses = LoadListItems(l, true);
                    }
                    response = await _myUtilities.LoadStudentCats();

                    if (response != null && response.Success)
                    {
                        List<StudentCategory> CatLists = JsonConvert.DeserializeObject<List<StudentCategory>>(response.PayLoad);
                        var l = (from p in CatLists select new ListItems { Text = p.categoryName, Value = p.categoryId.ToString() }).ToList();

                        ViewBag.Cats = LoadListItems(l, true);
                    }
                    else
                    {
                        var l = new List<ListItems>();
                        ViewBag.Cats = LoadListItems(l, true);
                    }
                response = await _myUtilities.LoadSubjectAsync("O");

                if (response != null && response.Success)
                {
                    List<subject> classLists = JsonConvert.DeserializeObject<List<subject>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Subjectname, Value = p.subjectAbbrev.ToString() }).ToList();

                    ViewBag.Subjects = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Subjects = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadParents();

                if (response != null && response.ResponseCode == 100)
                {
                    List<ParentViewDTO> ParentLists = JsonConvert.DeserializeObject<List<ParentViewDTO>>(response.PayLoad);
                    var l = (from p in ParentLists select new ListItems { Text = p.FullName, Value = p.ParentId.ToString() }).ToList();

                    ViewBag.Parent = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Parent = LoadListItems(l, true);
                }
                List<SelectListItem> relations = new List<SelectListItem>
    {
        new SelectListItem { Text = "Select", Value = "" },
        new SelectListItem { Text = "Father", Value = "1" },
        new SelectListItem { Text = "Mother", Value = "2" },
        new SelectListItem { Text = "Guardian", Value = "3" },
        new SelectListItem { Text = "Friend", Value = "4" },
        new SelectListItem { Text = "Other", Value = "5" }
    };
                ViewBag.Relations = relations;
                List<SelectListItem> items = new List<SelectListItem>
    {
        new SelectListItem { Text = "Select", Value = "" },
        new SelectListItem { Text = "ENGLISH", Value = "ENG" },
        new SelectListItem { Text = "MATHEMATICS", Value = "MATH" },
        new SelectListItem { Text = "SCIENCE", Value = "SCI" },
        new SelectListItem { Text = "SOCIAL STUDIES", Value = "SST" }
    };
                ViewBag.Primary = items;
                List<SelectListItem> aggs = new List<SelectListItem>
    {
        new SelectListItem { Text = "Select", Value = "" },
        new SelectListItem { Text = "D1", Value = "D1" },
        new SelectListItem { Text = "D2", Value = "D2" },
        new SelectListItem { Text = "C3", Value = "C3" },
        new SelectListItem { Text = "C4", Value = "C4" },
        new SelectListItem { Text = "C5", Value = "C5" },
        new SelectListItem { Text = "C6", Value = "C6" },
        new SelectListItem { Text = "P7", Value = "P7" },
        new SelectListItem { Text = "P8", Value = "P8" },
        new SelectListItem { Text = "F9", Value = "F9" },
        new SelectListItem { Text = "X", Value = "X" }
    };
                ViewBag.aggs = aggs;
                ViewBag.DocTypes = new List<SelectListItem>
    {
        new SelectListItem{Text="Select", Value=""},
        new SelectListItem{Text="P.L.E SLIP", Value="A"},
        new SelectListItem{Text="U.A.C.ECERTIFICATE", Value="C"},
        new SelectListItem{Text="TRANSCRIPT", Value="D"},
        new SelectListItem{Text="U.C.E CERTIFICATE", Value="B"},
        new SelectListItem{Text="PASSPORT", Value="E"},
        new SelectListItem{Text="NATIONAL ID", Value="F"},
        new SelectListItem{Text="OTHER", Value="H"},
        new SelectListItem{Text="CERTIFICATE", Value="G"},
    };


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
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

        // GET: AdmissionController/Delete/5
        public async Task<ActionResult> Delete(ParentAdd model)
        {
            string resp = "";
            string Url = "Miscellaneous/GenerateGuidId";
            string resp1 = "";
            string Url1 = "Miscellaneous/GenerateReferenceNumber/3";
            resp = await request.GetB(Url);
            resp1 = await request.GetB(Url1);

            var w = resp.Split('"')[3];
            model.Parentid = w;
            var M = resp1.Split('\"')[3];
            model.Parentno = M;
            return Json(new { response1 = w, response2 = M });
        }

        // POST: AdmissionController/Delete/5
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
        public async Task<ActionResult> StreamsByClass(int classs)
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
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
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
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ParentAddAsync(CreateStudentDTO model, IFormFileCollection files)
        {
            Url = "Parents";
            ApiResponse resp = null;

            try
            {
                if (!SessionData.IsSignedIn) { return Json(new { success = false, message = "User not signed in" }); }

                model.ParentAdd.SchoolCode= SessionData.ClientCode;
                model.ParentAdd.Parentno ??= "";
                model.ParentAdd.Contact2 ??= "";
                model.ParentAdd.Middlename ??= "";

                string base64String = "";
                byte[] imageBytes = null;

                if (files?.Count > 0 && files.First() != null)
                {
                    BinaryReader reader = new BinaryReader(files.First().OpenReadStream());
                    imageBytes = reader.ReadBytes((int)files.First().Length);
                    base64String = Convert.ToBase64String(imageBytes);

                    model.ParentAdd.Image = new ParentImage
                    {
                        Image64String = base64String,
                        ImageType = 1,
                        ParentNo = ""
                    };
                }

                resp = await request.AddAsync<ParentAdd>(model.ParentAdd, Url);

                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;
                    return Json(new { success = true, message = resp.ResponseMessage });
                }
                else if (resp.ResponseCode==101)
                {
                    TempData["info"] = resp.ResponseMessage;
                    return Json(new { success = true, message = resp.ResponseMessage });
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage;
                    return Json(new { success = false, message = "Error occurred, Contact Admin" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Json(new { success = false, message = "Error occurred: " + ex.Message });
            }
        }

        public async Task<IActionResult> GetDropdownOptions()
        {
            var options = new List<ListItems>();
            ApiResponse response = await _myUtilities.LoadParents();

            if (response != null && response.ResponseCode == 100)
            {
                List<ParentViewDTO> parentLists = JsonConvert.DeserializeObject<List<ParentViewDTO>>(response.PayLoad);

                options = parentLists.Select(p => new ListItems { Text = p.FullName, Value = p.ParentId.ToString() }).ToList();

                ViewBag.Parent = LoadListItems(options, true);
            }
            else
            {
                options = new List<ListItems>();
                ViewBag.Parent = LoadListItems(options, true);
            }

            return Json(options);
        }
        public async Task<ActionResult> Attendance(StdAttendance model)
        {
            try
            {

                if (!SessionData.IsSignedIn) {  return RedirectToAction("Index", "Login"); }

                ApiResponse response = null;

                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response != null && response.ResponseCode == 100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadClassStream(model.ClassId);

                if (response != null && response.Success)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }



                if (model.ClassId != 0)
                {

                    string formattedDate = new DateTime(model.AttendanceDate.Value.Year, model.AttendanceDate.Value.Month, model.AttendanceDate.Value.Day).ToString("yyyy-MM-dd");
                    ApiResponse respons = await _myUtilities.LoadStudentsAttendanceAsync(model.ClassId,model.SteamCode,formattedDate);
                    if (respons.Success && respons.ResponseCode == 100)
                    {
                        model = JsonConvert.DeserializeObject<StdAttendance>(respons.PayLoad);
                    }
                    else if (respons.ResponseCode == 101)
                    {
                        TempData["info"] = respons.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = respons.ResponseMessage;
                    }
                   
                 
                    model.AttendanceDate = model.AttendanceDate;
                }
                else
                {

                    var model1 = new Students();

                    return View(model);
                }


            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(AttendenceList model, string studentNo, string status, int classId, string steamCode, double attendanceId, string attendanceDate, string studentId, string studentname, string remarks)
        {
            
            try
            {
                if (!SessionData.IsSignedIn) {  return RedirectToAction("Index", "Login"); }
                Url = "StudentManagement/Student/Attendance/Save";
                model.StudentNo = studentNo.Trim();
                model.Status = status;
                model.AttendanceId = attendanceId;
                model.StudentId = studentId.Trim();
                model.ClassId = classId;
                model.Remarks = remarks ?? "";
                model.steamCode = steamCode;
                model.StudentName = studentname.Trim();
                model.attendanceDate = DateTime.Parse(attendanceDate);
                if (model.Status == "P")
                {
                    model.Remarks = "";
                }
                model.SchoolCode = SessionData.ClientCode;
                model.Term = SessionData.Term;
                ApiResponse resp = await request.AddAsync<AttendenceList>(model, Url);
                if (resp.Success && resp.ResponseCode == 100)
                {
                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
                }
                
                else
                {
                    var data = new { status = 201, res = resp };
                    var json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> BulkStudent(BulkImport model)
        {
            if (!SessionData.IsSignedIn)
            {
                return RedirectToAction("Index", "Login");
            }

            var contentRootPath = "";
            ApiResponse res = await _myUtilities.LoadSysparamByName("BULK_FILES_LOCATION");
            if (res.Success && res.ResponseCode == 100)
            {
                var model1 = new SysParamVm();
                model1 = JsonConvert.DeserializeObject<SysParamVm>(res.PayLoad);
                contentRootPath = model1.Value;
            }
            else if (res.ResponseCode == 101)
            {
                TempData["info"] = res.ResponseMessage;
            }
            else
            {
                TempData["error"] = res.ResponseMessage;
            }

            var files = Directory.GetFiles(contentRootPath);
            var bulkImports = new List<BulkImport>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var fileExtension = Path.GetExtension(file);
                var filePath = Path.GetDirectoryName(file);
                if (fileName.IndexOf("student", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var bulkImport = new BulkImport
                    {
                        FileName = fileName,
                        FileExt = fileExtension,
                        FilePath = filePath
                    };
                    bulkImports.Add(bulkImport);
                }
            }

            model.ImportFiles = bulkImports;
            ViewBag.ContentRootPath = contentRootPath;
            ViewBag.BulkImports = bulkImports;

            return View(model);
        }

        public async Task<ActionResult> BulkStudentImport(BulkImport model, string fileName)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse resp;
                model.FileName = fileName+model.FileExt;
                Url = $"Students/{SessionData.ClientCode}/{model.FileName}";
                resp = await request.AddAsync<BulkImport>(model, Url);
                if (resp!=null&& resp.Success && resp.ResponseCode == 100 && resp.PayLoad != null)
                {
                    TempData["success"] = resp.ResponseMessage;

                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured, Contact Admin";
            }


            return RedirectToAction(nameof(BulkStudent));
        }
        public async Task<ActionResult> StudentAttendanceDash()
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
