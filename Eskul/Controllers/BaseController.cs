using Eskul.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net;
using System.Security.Principal;
using Eskul.APIClient;
using Eskul.Custom;
using System.Text;
using SmartPaperEdms.Web.App_Code;
using Eskul.Hubs;
using Microsoft.AspNetCore.SignalR;
using static iText.Signatures.LtvVerification;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Runtime.CompilerServices;
using NPOI.SS.Formula.Functions;

namespace Eskul.Controllers
{
    public class MyUtilities
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration _configuration;
        private readonly ILoggerErr _logger;

        public MyUtilities(IConfiguration configuration, ILoggerErr logger)
        {
            _configuration = configuration;
            _logger = logger;
            request = new RequestHandler(configuration);
        }
        #region Academics
        #endregion
        #region AccountsAndFinance
        #endregion
        #region Assignments
        #endregion
        #region Behaviours
        #endregion
        #region Examinations
        #endregion
        #region FeesCollection
        #endregion
        #region HostelManagement
        #endregion
        #region Library
        #endregion
        #region Menu
        #endregion
        #region Miscellaneous
        #endregion
        #region Parents
        #endregion
        #region Projects
        #endregion
        #region Settings
        #endregion
        #region Staffs
        #endregion
        #region Students
        #endregion
        #region UserManagement
        #endregion
        public async Task<ApiResponse> LoadAssignmentMarkList(decimal assignmentId, int classId, string streamCode)
        {
            Url = $"Examinations/Assignment/Scores/Load/{SessionData.ClientCode}/{assignmentId}/{classId}/{streamCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<MarksAddList> LoadMarkList(string id)
        {
            Url = "Examination/AssignmentScore/Add/" + id;
            //if (fromDb)
            return await request.GetSingle<MarksAddList>(Url);
            //return await request.GetAll<MarksAddList>(Url);
        }
        public async Task<ApiResponse> _LoadMarkList(string id)
        {
            Url = $"Projects/Scores/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadSubjectPapers(string code)
        {
            Url = $"Academics/Subject/Papers/Get/{SessionData.ClientCode}/{code}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<subjectpaperList>> LoadTopic(string code)
        {
            Url = "Academics/SubjectsPapers/" + code;
            return await request.GetAll<subjectpaperList>(Url);
        }
        public async Task<List<subjectpaperList>> LoadTopicCompetency(string code)
        {
            Url = "Academics/SubjectsPapers/" + code;
            return await request.GetAll<subjectpaperList>(Url);
        }
        public async Task<List<subjectpaperList>> _LoadSubjectPapers(string userid, int classs, string streamcode, string subjectcode)
        {
            Url = "Academics/SubjectTeacher/Get/TaughtPaper/" + userid + "/" + classs + "/" + streamcode + "/" + subjectcode;
            return await request.GetAll<subjectpaperList>(Url);
        }
        public async Task<List<ReporTypeList>> LoadReportTypes(reportfil model)
        {
            Url = "Examination/ReportCard/GetAll/?Class=" + model.Class;
            return await request.GetAll<ReporTypeList>(Url);
        }
        public async Task<ApiResponse> LoadBranches()
        {
            Url = $"Settings/Branches/GetAllBySchoolCode/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadMenus(string ProfileCode, string ClientCode)
        {
            Url = $"Menu/Profile/Menus/Get/{ProfileCode}/{ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<string> CheckLicence(string clientCode, string productCode, string licenceCode)
        {
            Url = "Menu/Licence/Validate/" + licenceCode + "/" + productCode + "/" + clientCode;

            return await request.GetB(Url);
        }
        public async Task<List<Curriculum>> LoadCurriculum(bool fromDb)
        {
            Url = "Miscellaneous/Curriculums";
            if (fromDb)
                return await request.GetAll<Curriculum>(Url);
            return await request.GetAll<Curriculum>(Url);
        }
        public async Task<ApiResponse> LoadClassTeacherAsync(int id)
        {
            Url = $"Academics/ClassTeachers/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadClassTeachersAsync()
        {
            Url = $"Academics/ClassTeachers/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadBranch(int id)
        {
            Url = $"Settings/Branch/GetById/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadSubjects(string level = "O")
        {
            Url = $"Academics/Subjects/Offered/{SessionData.ClientCode}/{level}";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<List<subjectList>> _LoadSubjects(string userid, int classs, string streamcode)
        {
            Url = "Academics/SubjectTeacher/Get/TaughtSubject/" + userid + "/" + classs + "/" + streamcode;
            return await request.GetAll<subjectList>(Url);
        }
        public async Task<List<subjectList>> LoadSubjects()
        {
            Url = "Academics/Subject/GetAll";
            return await request.GetAll<subjectList>(Url);
        }
        public async Task<List<subjectList>> LoadOfferedSubjects()
        {
            Url = "Academics/Subject/GetAllOffered";
            return await request.GetAll<subjectList>(Url);
        }
        public async Task<List<streamList>> LoadStream(int classs)
        {
            Url = $"Settings/Class/GetStreams/{SessionData.ClientCode}/{classs}";
            return await request.GetAll<streamList>(Url);
        }
        public async Task<List<streamList>> _LoadStreams(string userid, int classs)
        {
            Url = "Academics/SubjectTeacher/Get/TaughtStreams/" + userid + "/" + classs;
            return await request.GetAll<streamList>(Url);
        }
        public async Task<ApiResponse> LoadClassess(string level = "O")
        {
            Url = $"Settings/Class/GetByLevel/{level}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadSkillDefinitions()
        {
            Url = $"Academics/GenericSkill/Definition/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<ClassList>> LoadClasses(string level = "O")
        {
            Url = $"Settings/Class/GetByLevel/{level}";
            return await request.GetAll<ClassList>(Url);
        }
        public async Task<List<ClassList>> _LoadClasses(string userid)
        {

            Url = "Academics/SubjectTeacher/Get/TaughtClasses/" + userid;
            return await request.GetAll<ClassList>(Url);
        }
        public async Task<List<StaffList>> LoadStaffs(bool fromDb)
        {
            Url = "StaffManagement/Staffs";
            if (fromDb)
                return await request.GetAll<StaffList>(Url);
            return await request.GetAll<StaffList>(Url);
        }
        public async Task<List<StaffList>> LoadAllStaffs(bool fromDb)
        {
            Url = $"StaffManagement/Staffs/GetAll/{SessionData.ClientCode}";
            return fromDb ? await request.GetAll<StaffList>(Url) : await request.GetAll<StaffList>(Url);
        }
        public async Task<ApiResponse> LoadFeesGroups(bool fromDb)
        {
            Url = $"FeesCollection/FeesGroup/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadFeesGroup(string id)
        {
            Url = $"FeesCollection/FeesGroup/GetById/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStreamList()
        {
            Url = $"Settings/ClassStream/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadClassStream(int classid)
        {
            Url = $"Settings/Class/GetStreams/{SessionData.ClientCode}/{classid}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStream(string StreamId)
        {
            Url = $"Settings/ClassStream/GetByCode/{SessionData.ClientCode}/{StreamId}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> SignOutUser(string Username)
        {
            Url = $"Users/SignOut/{SessionData.ClientCode}/{Username}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadExams()
        {
            Url = "Examinations/ExamType/Get/All";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<Students>> LoadStudents(int Class, string Stream)
        {
            Url = $"StudentManagement/Students/{SessionData.ClientCode}/{Class}/{Stream}/{SessionData.UserBranchId}";
            return await request.GetAll<Students>(Url);
        }
        public async Task<ApiResponse> LoadParents()
        {
            Url = $"Parents/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        
        public async Task<List<Book>> LoadBooks(bool fromDb)
        {
            Url = "Library/LibraryBook";
            if (fromDb)
                return await request.GetAll<Book>(Url);
            return await request.GetAll<Book>(Url);
        }
        public async Task<List<LibraryMembers>> LoadLibraryMembers(bool fromDb)
        {
            Url = "Library/AllLibraryMemberShip";
            if (fromDb)
                return await request.GetAll<LibraryMembers>(Url);
            return await request.GetAll<LibraryMembers>(Url);
        }
        public async Task<List<PaymentMode>> LoadPayModes(bool fromDb)
        {
            Url = "AccountsAndFinance/PaymentMode";

            if (fromDb)
                return await request.GetAll<PaymentMode>(Url);

            return await request.GetAll<PaymentMode>(Url);
        }
        public async Task<List<Designation>> LoadDesignations(bool fromDb)
        {
            Url = $"StaffManagement/Designations/GetAllBySchool/{SessionData.ClientCode}";

            if (fromDb)
                return await request.GetAll<Designation>(Url);

            return await request.GetAll<Designation>(Url);
        }
        public async Task<List<documenttype>> LoadDocumentTypes(bool fromDb)
        {
            Url = "Miscellaneous/DocumentTypes";

            if (fromDb)
                return await request.GetAll<documenttype>(Url);

            return await request.GetAll<documenttype>(Url);
        }
        public async Task<ApiResponse> LoadHouses()
        {
            Url = $"Settings/Houses/GetAll/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadHouse(string Code)
        {
            Url = $"Settings/House/GetByCode/{SessionData.ClientCode}/{Code}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<ResultCodes>> LoadResCodes(bool fromDb)
        {
            Url = "Miscellaneous/PleResults";

            if (fromDb)
                return await request.GetAll<ResultCodes>(Url);

            return await request.GetAll<ResultCodes>(Url);
        }
        public async Task<List<acaward>> LoadAwards(bool fromDb)
        {
            Url = "Miscellaneous/AcademicAwards";
            if (fromDb)
                return await request.GetAll<acaward>(Url);
            return await request.GetAll<acaward>(Url);
        }
        public async Task<ApiResponse> LoadStudentCats()
        {
            Url = $"Students/Categories/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadTermSessions()
        {
            Url = $"Settings/TermSessions/GetAllBySchool/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadTermSession(int Code)
        {
            Url = $"Settings/TermSession/GetById/{SessionData.ClientCode}/{Code}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<BloodGroup>> LoadBloodGroup(bool fromDb)
        {
            Url = "Miscellaneous/BloodGroup";
            if (fromDb)
                return await request.GetAll<BloodGroup>(Url);
            return await request.GetAll<BloodGroup>(Url);
        }
        public async Task<List<citizentype>> LoadCitizentypes(bool fromDb)
        {
            Url = "Miscellaneous/CitizenType";
            if (fromDb)
                return await request.GetAll<citizentype>(Url);
            return await request.GetAll<citizentype>(Url);
        }
        public async Task<List<section>> LoadSections(bool fromDb)
        {
            Url = "Miscellaneous/Sections";
            if (fromDb)
                return await request.GetAll<section>(Url);
            return await request.GetAll<section>(Url);
        }
        public async Task<List<religions>> LoadREgion(bool fromDb)
        {
            Url = "Miscellaneous/Religions";
            if (fromDb)
                return await request.GetAll<religions>(Url);
            return await request.GetAll<religions>(Url);
        }
        public async Task<ApiResponse> LoadDisTypesAsync()
        {
            Url = $"Students/DisableReasons?schoolCode={SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadDisTypeAsync(int id)
        {
            Url = $"Students/DisableReasons/{id}?schoolCode={SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<country>> LoadCountries(bool fromDb)
        {
            Url = "Miscellaneous/country";
            if (fromDb)
                return await request.GetAll<country>(Url);
            return await request.GetAll<country>(Url);
        }
        public async Task<List<district>> LoadDistricts(bool fromDb)
        {
            Url = "Miscellaneous/District";
            if (fromDb)
                return await request.GetAll<district>(Url);
            return await request.GetAll<district>(Url);
        }
        public async Task<List<funding>> LoadFundings(bool fromDb)
        {
            Url = "Miscellaneous/Funding";
            if (fromDb)
                return await request.GetAll<funding>(Url);
            return await request.GetAll<funding>(Url);
        }
        public async Task<List<BursaryList>> LoadBursaries(bool fromDb)
        {
            Url = "AccountsAndFinance/Bursary";
            if (fromDb)
                return await request.GetAll<BursaryList>(Url);
            return await request.GetAll<BursaryList>(Url);
        }
        public async Task<List<BursaryList>> LoadBursary(BursaryVm model)
        {
            try
            {
                if (model.BursaryId == null)
                {
                    model.BursaryId = "0";
                }
                Url = "AccountsAndFinance/Bursary/" + model.BursaryId;
                return await request.Get<BursaryList>(Url);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<Students>> LoadStudent(StudentAdd model)
        {
            if (model.Studentid == null)
            {
                model.Studentid = "-1";
            }
            try
            {
                Url = "StudentManagement/Student/" + model.Studentid;
                return await request.Get<Students>(Url);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<Students>> LoadStudents(int Class, string Stream,int BranchId)
        {
            Url = $"Students/{SessionData.ClientCode}{Class}/{Stream}/{BranchId}";

            return await request.GetAll<Students>(Url);

        }
        public async Task<ApiResponse> LoadStudentsAsync(string Level, int Class, string Stream, bool IsDisabled,bool allDetails)
        {
            Url = $"Students/{SessionData.ClientCode}/{Level}/{Class}/{Stream}/{IsDisabled}/{allDetails}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentAsync(string StudentId,bool allDetails)
        {
            Url = $"Students/{SessionData.ClientCode}/{StudentId}/{allDetails}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<StudentSubject>> LoadStudentSubjects(string StudentId)
        {
            Url = $"Students/Subjects/{SessionData.ClientCode}/{StudentId}";

            return await request.GetAll<StudentSubject>(Url);

        }
        public async Task<ApiResponse> LoadStudentSubjectsAsync(string StudentId)
        {
            Url = $"Students/Subjects/{SessionData.ClientCode}/{StudentId}";

            var response = await request.GetAsync(Url);

            return response;

        }
        public async Task<ApiResponse> LoadStudentsAttendanceAsync(int Class, string Stream, string date)
        {
            Url = $"StudentManagement/Student/Attendance/Get/List/{SessionData.ClientCode}/{SessionData.Term}/{Class}/{Stream}/{date}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<StudentProfile> LoadStudentProfile(string id)
        {

            Url = "StudentManagement/Get/StudentProfile/" + id;


            return await request.GetSingle<StudentProfile>(Url);
        }
        public async Task<List<subjectList>> LoadSubjects(bool fromDb)
        {
            Url = "Academics/Subject/GetAll";

            if (fromDb)
                return await request.GetAll<subjectList>(Url);

            return await request.GetAll<subjectList>(Url);
        }
        public async Task<ApiResponse> LoadSubjectAsync(string level)
        {
            Url = $"Academics/Subjects/Offered/{SessionData.ClientCode}/{level}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<subjectList>> LoadSubject(subject model)
        {

            Url = "Academics/SubjectByCode/" + model.Subjectcode;


            return await request.Get<subjectList>(Url);

            //return await request.Get<subjectList>(Url);
        }
        public async Task<List<subjectpaperList>> LoadSubjectPapers(bool fromDb)
        {
            Url = "Academics/Papers";

            if (fromDb)
                return await request.GetAll<subjectpaperList>(Url);

            return await request.GetAll<subjectpaperList>(Url);
        }
        public async Task<List<subjectpaperList>> LoadSubjectPaper(subjectpaper model)
        {
            var id2 = model.PaperCode.Replace("/", "-");

            var EditUrl = "Academics/PaperByCode/" + id2 + "";
            Url = "Academics/SubjectPaper/" + model.PaperCode.Replace("%2F", "-");


            return await request.Get<subjectpaperList>(EditUrl);

            //return await request.Get<subjectList>(Url);
        }
        public async Task<ApiResponse> LoadSubjecteacherAsync(int id)
        {
            Url = $"Academics/SubjectTeacher/GetById/{SessionData.ClientCode}/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadSubjecteachersAsync()
        {
            Url = $"Academics/SubjectTeacher/GetAll/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<List<Subjecteacher>> LoadSubjecteacher(Subjecteacher model)
        {
            Url = $"Academics/SubjectTeacher/GetById/{SessionData.ClientCode}/{((int)model.Id)}";
            return await request.Get<Subjecteacher>(Url);
        }
        public async Task<List<SysConfigVm>> LoadSysconfigarations(bool fromDb)
        {
            Url = "Settings/SystemConfigs";
            if (fromDb)
                return await request.GetAll<SysConfigVm>(Url);
            return await request.GetAll<SysConfigVm>(Url);
        }
        public async Task<List<SysConfigVm>> LoadSysconfigaration(SysConfigVm model)
        {
            Url = "Settings/SystemConfig/" + model.Id;
            return await request.Get<SysConfigVm>(Url);
        }
        public async Task<ApiResponse> LoadSysparameters()
        {
            Url = $"Settings/SystemParameters/GetBySchoolCode/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadSysparameter(int SysParamId)
        {
            Url = $"Settings/SystemParameter/{SessionData.ClientCode}/{SysParamId}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadSysparamByName(string ParamName)
        {
            Url = $"Settings/SystemParameter/{SessionData.ClientCode}/{ParamName}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<TermsessionList>> LoadTermSession(TermsessionVm model)
        {

            Url = "Settings/TermSession/GetById/" + SessionData.ClientCode + "/" + model.TermSessionId;
            return await request.Get<TermsessionList>(Url);
        }
        public async Task<ApiResponse> LoadUsers(UserS model)
        {
            Url = $"Users/{SessionData.ClientCode}/{model.UserType}/{model.ProfileCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        
        public async Task<ApiResponse> LoadUserDetails(string Username)
        {
            Url = $"UserManagement/GetUserDetails/{Username}/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadUser(string UserId)
        {
            Url = $"Users/{SessionData.ClientCode}/{UserId}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadProfiles()
        {
            Url = $"Users/Profiles/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
      
        public async Task<ApiResponse> ChangePassword(string RawPassword)
        {
            Url = $"UserManagement/ChangeUserPassword/{SessionData.UserId}/{RawPassword}/{SessionData.ClientCode}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadProfile(string Id)
        {
            Url = $"UserManagement/UserProfileByCode/{SessionData.ClientCode}/{Id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadParent(string Id)
        {
            Url = $"Parents/{SessionData.ClientCode}/{Id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> GenerateReport(GenerateReport model)
        {
            Url = $"Examinations/Reports/Generate/{SessionData.ClientCode}/{SessionData.UserBranchId}/{model.year}/{model.term.ToString()}/{model.classs.ToString()}/{model.stream.ToString()}/{model.reportType.ToString()}/{model.checkAll}/{model.studentId}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> GenerateMarkSheet(SubmarkVm model)
        {
            Url = $"Examinations/MarkSheet/Subject/WritePDF/{SessionData.ClientCode}/{model.Year}/{model.TermCode}/{model.Class}/{model.Stream}/{model.SubjectCode}/{model.PaperCode}/{model.ExamCode}?subject={model.SubjectCode}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> GenerateMarkList(SubmarkVm model)
        {
            Url = $"Examinations/MarksList/Subject/WritePDF/{SessionData.ClientCode}/{model.Year}/{model.TermCode}/{model.Class}/{model.Stream}/{model.SubjectCode}/{model.PaperCode}/{model.ExamCode}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadList(reportfil model)
        {
            //Url = $"Examinations/Report/Students/Search/{SessionData.ClientCode}/{model.Branch.ToString()}/{model.Year}/{model.Term.ToString()}/{model.Class.ToString()}/{model.Stream}";
            Url = $"Examinations/Reports/{SessionData.ClientCode}/{model.Level}/{model.Class.ToString()}/{model.Stream}/{false}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> _LoadMarkListStudents(ExamMarksList model)
        {
            string Url = $"Examinations/MarksLists/{SessionData.ClientCode}/{model.Year}/{model.TermCode}/{SessionData.UserBranchId}/{model.Class}/{model.Stream}/{model.ExamCode}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> _GenerateMarklist(MarksList model)
        {
             Url= $"Examinations/MarksList/WritePDF/{SessionData.ClientCode}/{model.Year}/{model.TermCode}/{SessionData.UserBranchId}/{model.Class}/{model.Stream}/{model.ExamCode}";


            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> _GenerateMarksheet(MarksList model)
        {
            Url = $"Examinations/MarkSheet/WritePDF/{SessionData.ClientCode}/{model.Year}/{model.TermCode}/{SessionData.UserBranchId}/{model.Class}/{model.Stream}/{model.ExamCode}";


            var response = await request.GetAsync(Url);

            return response;
        }
        #region Department
        public async Task<ApiResponse> LoadDepartments()
        {
            Url = $"Settings/Departments/GetAll/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadDepartment(string id)
        {
            Url = $"Settings/Department/GetByCode/{SessionData.ClientCode}/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion Department
        public async Task<List<Designation>> LoadDesignation(Designation model)
        {

            Url = $"StaffManagement/Designation/GetByCode/{SessionData.ClientCode}/{model.DesignationCode}";
            return await request.Get<Designation>(Url);
        }
        public async Task<List<disabileReason>> LoadDisType(disabileReason model)
        {
            Url = $"StudentManagement/DisableReason/{SessionData.ClientCode}/{model.reasonId.ToString()}";
            return await this.request.Get<disabileReason>(MyUtilities.Url);
        }
        public async Task<List<ExamGradeList>> LoadExamGrades(bool fromDb)
        {
            Url = "Examination/ExamGrade/Get/All";

            if (fromDb)
                return await request.GetAll<ExamGradeList>(Url);

            return await request.GetAll<ExamGradeList>(Url);
        }
        public async Task<List<ExamGradeList>> LoadExamGrade(ExamGradeAdd model)
        {

            Url = "Examination/ExamGrade/Get/ByCode/" + model.ExamGradeId;


            return await request.Get<ExamGradeList>(Url);

            //return await request.Get<subjectList>(Url);
        }
        public async Task<ApiResponse> SaveExamarks(decimal scoreId, string studentId, string score, string comment)
        {
            Url = $"Examinations/Scores/{SessionData.ClientCode}/{scoreId}/{studentId}/{score}/{comment}";

            var response = await request.UpdateAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadExamss(ExamScore model)
        {
            Url = $"Examinations/Scores/{SessionData.ClientCode}/{model.YearCode}/{model.TermCode}/{model.Class}/{model.SubjectCode}/{model.PaperCode.Replace("/", "-") + "/" + model.ExamCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<ExamScore>> _LoadExamss(ExamScore model)
        {

            Url = $"Examinations/Scores/{SessionData.ClientCode}/{model.YearCode}/{model.TermCode}/{model.Class}/{model.SubjectCode}/{model.PaperCode.Replace("/", "-") + "/" + model.ExamCode}";
            return await request.Get<ExamScore>(Url);
        }
        public async Task<List<MarksList>> LoadMarkList(MarksList model)
        {

            Url = "Examination/MarksList/Get/" + model.Year + "/" + model.Branch + "/" + model.TermCode + "/" + model.Class + "/" + model.ExamCode + "/" + model.Stream;
            return await request.Get<MarksList>(Url);
        }
        public async Task<List<Assignment>> LoadAssignment(Assignment model)
        {

            Url = "Examination/Assignment/" + model.AssignmentId;


            return await request.Get<Assignment>(Url);

            //return await request.Get<subjectList>(Url);
        }
        public async Task<Assignment> LoadAssignmentById(string id)
        {

            Url = "Examination/Assignment/" + id;


            return await request.GetSingle<Assignment>(Url);
        }
        public async Task<ProjectAddModel> LoadProjectById(string id)
        {

            Url = "Examination/Project/Get/" + id;


            return await request.GetSingle<ProjectAddModel>(Url);
        }
        public async Task<List<ExamTypeVm>> LoadExam(ExamTypeVm model)
        {
            Url = $"Academics/ExamType/{model.ExamCode}";
            return await request.Get<ExamTypeVm>(Url);
        }
        public async Task<List<ExpenseType>> LoadExpenseTypes(bool fromDb)
        {
            Url = "AccountsAndFinance/ExpenseType";

            if (fromDb)
                return await request.GetAll<ExpenseType>(Url);

            return await request.GetAll<ExpenseType>(Url);
        }
        public async Task<List<ExpenseType>> LoadExpenseType(ExpenseType model)
        {

            Url = $"AccountsAndFinance/ExpenseType/{model.ExpenseCode}";
            return await request.Get<ExpenseType>(Url);
        }

        public async Task<List<FeesGroup>> LoadFeesGroup(FeesGroup model)
        {

            Url = $"AccountsAndFinance/FeesGroup/{model.GroupCode}";
            return await request.Get<FeesGroup>(Url);
        }
        public async Task<ApiResponse> LoadFeeTypes(bool fromDb)
        {
            Url = $"FeesCollection/FeesType/GetAll/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadFeeType(string id)
        {
            Url = $"FeesCollection/FeesType/GetById/{SessionData.ClientCode}/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<GLAccount>> LoadGLAccounts(bool fromDb)
        {
            Url = "AccountsAndFinance/GLAccountSetting";

            if (fromDb)
                return await request.GetAll<GLAccount>(Url);

            return await request.GetAll<GLAccount>(Url);
        }
        public async Task<List<GLAccount>> LoadGLAccount(GLAccount model)
        {

            Url = $"AccountsAndFinance/GLAccountSetting/{model.SettingId}";
            return await request.Get<GLAccount>(Url);
        }
        public async Task<List<IncomeType>> LoadIncomeTypes(bool fromDb)
        {
            Url = "AccountsAndFinance/IncomeType";

            if (fromDb)
                return await request.GetAll<IncomeType>(Url);

            return await request.GetAll<IncomeType>(Url);
        }
        public async Task<List<IncomeType>> LoadIncomeType(IncomeType model)
        {

            Url = $"AccountsAndFinance/IncomeType/{model.IncomeCode}";
            return await request.Get<IncomeType>(Url);
        }
        public async Task<List<Book>> LoadBook(Book model)
        {
            try
            {
                Url = $"Library/LibraryBook/{model.BookId}";
                return await request.Get<Book>(Url);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<Students>> LoadStudents()
        {
            Url = "StudentManagement/Students";
            return await request.GetAll<Students>(Url);
        }

        public async Task<List<NonLibStudents>> LoadLibStudents(StudentMember model)
        {

            Url = $"Library/LibraryStudentMemberShip/{model.ClassId}/{model.Stream}";
            return await request.Get<NonLibStudents>(Url);
        }
        public async Task<List<PayModeAccount>> LoadPayModeAccounts(bool fromDb)
        {
            Url = "AccountsAndFinance/PaymentModeAccount";

            if (fromDb)
                return await request.GetAll<PayModeAccount>(Url);

            return await request.GetAll<PayModeAccount>(Url);
        }
        public async Task<List<PayModeAccount>> LoadPayModeAccount(PayModeAccount model)
        {

            Url = "AccountsAndFinance/PaymentModeAccount/" + model.SettingId;
            return await request.Get<PayModeAccount>(Url);
        }

        public async Task<List<PaymentMode>> LoadPayMode(PaymentMode model)
        {

            Url = "AccountsAndFinance/PaymentMode/" + model.ModeCode;
            return await request.Get<PaymentMode>(Url);
        }
        public async Task<List<PositionTittle>> LoadPTitles(bool fromDb)
        {
            Url = "Academics/PositionTitles";

            if (fromDb)
                return await request.GetAll<PositionTittle>(Url);

            return await request.GetAll<PositionTittle>(Url);
        }
        public async Task<List<PositionTittle>> LoadPTitle(PositionTittle model)
        {

            Url = "Academics/PositionTitle/" + model.titleCode;


            return await request.Get<PositionTittle>(Url);
        }
        public async Task<List<ProfileList>> LoadProfile(ProfileVm model)
        {

            Url = $"UserManagement/UserProfileByCode/{model.Code}";


            return await request.Get<ProfileList>(Url);
        }
        public async Task<List<acaward>> LoadAclevels(bool fromDb)
        {
            Url = "Miscellaneous/AcademicAwards";
            if (fromDb)
                return await request.GetAll<acaward>(Url);
            return await request.GetAll<acaward>(Url);
        }

        #region Report
        public async Task<List<ReportTemplate>> LoadReportTemplates(int Class)
        {
            Url = "Examination/ReportTemplate/Search/" + Class;
            return await request.GetAll<ReportTemplate>(Url);
        }
        public async Task<List<streamList>> GenerateReport(int classs)
        {
            Url = "Settings/Class/GetStreams/" + classs;
            return await request.GetAll<streamList>(Url);
        }
        #endregion Report
        #region SalaryScale
        public async Task<List<SalaryScale>> LoadSalaryScales(bool fromDb)
        {
            Url = "StaffManagement/SalaryScales";

            if (fromDb)
                return await request.GetAll<SalaryScale>(Url);

            return await request.GetAll<SalaryScale>(Url);
        }
        public async Task<List<SalaryScale>> LoadSalaryScale(SalaryScale model)
        {

            Url = "StaffManagement/SalaryScale/" + model.Code;
            return await request.Get<SalaryScale>(Url);
        }
        #endregion SalaryScale
        #region SysConfigVm
        public async Task<ApiResponse> LoadSchoolDetails()
        {
            Url = $"Settings/SystemConfigs/GetBySchool/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion SysConfigVm
        #region ScoreDescriptor
        public async Task<ApiResponse> LoadScoreDescriptorsAsync()
        {
            Url = $"Examinations/ScoreDescriptors/Get/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<List<ScoreDescriptor>> LoadScoreDescriptor(ScoreDescriptor model)
        {

            Url = $"Examination/ScoreDescriptor/GetById/{SessionData.ClientCode}/{model.descriptorId}";
            return await request.Get<ScoreDescriptor>(Url);
        }
        #endregion ScoreDescriptor
        #region securitypram
        public async Task<ApiResponse> LoadSecparameters()
        {
            Url = $"Settings/SecurityParameters/Get/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadAsyncSecparameter()
        {
            Url = $"Settings/SecurityParameters/Get/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<securitypram>> LoadSecparameter(securitypram model)
        {
            Url = "Settings/SecurityParameter/" + SessionData.ClientCode + "/" + model.parameterid;
            return await request.Get<securitypram>(Url);
        }
        #endregion securitypram
        #region Staff
        public async Task<List<staffcategory>> LoadStaffCats(bool fromDb)
        {
            Url = "StaffManagement/StaffCategories";

            if (fromDb)
                return await request.GetAll<staffcategory>(Url);

            return await request.GetAll<staffcategory>(Url);
        }
        public async Task<List<staffpaytype>> LoadPayTypes(bool fromDb)
        {
            Url = "StaffManagement/StaffPayTypes";

            if (fromDb)
                return await request.GetAll<staffpaytype>(Url);

            return await request.GetAll<staffpaytype>(Url);
        }
        public async Task<List<StaffImageList>> LoadStaffImage(string StaffNo, int d)
        {

            try
            {
                string StaffImageUrl = "StaffManagement/RetrieveStaffImage/" + StaffNo + "/" + d + "";
                return await request.Get<StaffImageList>(StaffImageUrl);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<StaffList>> _LoadStaff(StaffAdd model)
        {
            if (model.Staffid == null)
            {
                model.Staffid = "0";
            }
            try
            {
                Url = $"StaffManagement/Staff/GetById/{SessionData.ClientCode}/{model.Staffid}";
                return await request.Get<StaffList>(Url);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<ApiResponse> LoadStaffsByCategory(string Staffcat)
        {
            Url = $"Staffs/{SessionData.ClientCode}/{Staffcat}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStaffProfile(string StaffId)
        {
            Url = $"Staffs/Profile?StaffId={StaffId}";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<List<StaffList>> LoadNonTeachingStaffs(bool fromDb)
        {
            Url = $"StaffManagement/GetAllByCategory/{SessionData.ClientCode}/{"NT"}";
            if (fromDb)
                return await request.GetAll<StaffList>(Url);
            return await request.GetAll<StaffList>(Url);
        }
        public string GetInitials(string fName, string mName, string lName)
        {
            try
            {
                var result = new StringBuilder();

                if (fName.Length > 0)
                {
                    result.Append(char.ToUpper(fName[0]));
                }

                if (!string.IsNullOrEmpty(mName))
                {
                    result.Append(char.ToUpper(mName[0]));
                }

                if (lName.Length > 0)
                {
                    result.Append(char.ToUpper(lName[0]));
                }
                return result.ToString().TrimEnd();

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);

            }


            return "";
        }
        public async Task<ApiResponse> LoadStaff(string id)
        {
            Url = $"Staffs/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion Staff
        #region Streams
        public async Task<List<streamList>> LoadStream(stream model)
        {

            Url = "Settings/ClassStream/GetByCode/" + SessionData.ClientCode + "/" + model.streamId;


            return await request.Get<streamList>(Url);

            //return await request.Get<subjectList>(Url);
        }
        #endregion Streams  
        #region StudentCategory
        public async Task<ApiResponse> LoadStudentCat(int id)
        {
            Url = $"Students/Categories/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion  StudentCategory
        #region House
        //public async Task<List<HouseList>> LoadHouse(HouseVm model)
        //{
        //    Url = "Academics/House/GetAllByHouseCode/"+SessionData.ClientCode+"/" + model.Code;
        //    return await request.Get<HouseList>(Url);
        //}
        #endregion House
        #region Assignment
        public async Task<ApiResponse> LoadAssignments(SubjectAssignments model)
        {
            Url = $"Assignments?school={SessionData.ClientCode}&class={model.Class}&year={model.Year}&term={model.Term}&subject={model.SubjectCode}";

            var response = await request.GetAsync(Url);

            return response;
        }

            public async Task<ApiResponse> LoadProjects(SubjectProjects model)
        {
            Url = $"Projects/{SessionData.ClientCode}/{model.Year}/{model.Term}/{model.Class}/{model.SubjectCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
       public async Task<ApiResponse> LoadProject(double id)
        {
            Url = $"Projects/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadAssignment(decimal id)
        {
            Url = $"Assignments/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion Assignment
        #region Topics
        public async Task<List<SubTopic>> LoadSubTopics(String year,int ClassId,string SubCode)
        {
            Url = "Academics/LessonPlan/SubjectTopic/Search/"+year+"/"+ClassId+"/"+SubCode;

                return await request.GetAll<SubTopic>(Url);
        }
        public async Task<List<SubTopic>> LoadSubTopic(SubTopic model)
        {

            Url = "Settings/Branch/" + model.TopicId;
            return await request.Get<SubTopic>(Url);
        }
        public async Task<ApiResponse> LoadTopics(String year, int ClassId, string SubCode)
        {
            Url = $"Academics/Topics/{SessionData.ClientCode}/{year}/{ClassId}/{SubCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadTopic(decimal topicId)
        {
            Url = $"Academics/Topics/{SessionData.ClientCode}/{topicId}/";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadTopicComps(decimal id)
        {
            Url = $"Academics/Topics/Subject/Competency/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion Topics
        #region ScoreRanks
        public async Task<ApiResponse> LoadScoreRanksAsync(ScoreRank model)
        {
            Url = $"Examinations/ScoreRank/GetByClass/{model.ClassCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadScoreRankByIdAsync(decimal id)
        {
            Url = $"Examinations/ScoreRank/GetById/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<ScoreRank>> LoadScoreRank(ScoreRank model)
        {

            Url = "Academics/Exam/ScoreRank/SearchById/" + model.ScoreRankId;
            return await request.Get<ScoreRank>(Url);
        }
        #endregion ScoreRanks
        #region Hostels
        public async Task<ApiResponse> LoadHostels()
        {
            Url = $"HostelManagement/HostelBlock/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadHostelRooms()
        {
            Url = $"HostelManagement/HostelRoom/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion Hostels
        #region ExhibitedValue
        public async Task<ApiResponse> LoadValueDefinitions()
        {
            Url = $"Academics/StudentValues/Definition/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentsExhibtedValues(string Level, int Class, string Stream)
        {
            Url = $"Academics/StudentValues/Exhibited/GetCount/{SessionData.ClientCode}/{Level}/{Class}/{Stream}/{DateTime.Now.Year}/{SessionData.Term}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentExhibitedValues(string id)
        {
            Url = $"Academics/StudentValues/Exhibited/GetByStudentId/{SessionData.ClientCode}/{id}/{DateTime.Now.Year}/{SessionData.Term}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentExhibitedValue(string id)
        {
            Url = $"Academics/StudentValues/Exhibited/GetByStudentId/{SessionData.ClientCode}/{id}/{DateTime.Now.Year}/{SessionData.Term}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentExhibitedValue(double id)
        {
            Url = $"Academics/StudentValues/Exhibited/GetById/{SessionData.ClientCode}/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<ApiResponse> LoadSchoolSubjects(string level = "O")
        {
            Url = $"Settings/Subjects/{SessionData.ClientCode}/{level}";

            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion
        #region User
        public async Task<ApiResponse> GetUserByUsername(LoginModel model)
        {
            Url = $"Users/Details/{model.ClientCode}/{model.UserName}";
            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion User
        public async Task<ApiResponse> LoadExamSettings()
        {
            Url = $"Academics/Topics";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadExamSetting(int id)
        {
            Url = $"Examinations/ExamSetting/Get/ByCode/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadGSCategories()
        {
            Url=$"Academics/GenericSkill/Category/GetAll/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadDefinitions(int id)
        {
            Url = $"Academics/GenericSkill/Definition/GetByCaterogyId/{SessionData.ClientCode}/{id}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadDefinition(int definitionid)
        {
            Url = $"Academics/GenericSkill/Definition/GetById/{SessionData.ClientCode}/{definitionid}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadGSCat(int categoryId)
        {
            Url = $"Academics/GenericSkill/Category/GetById/{SessionData.ClientCode}/{categoryId}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<List<GsCat>> LoadStudentCatValues(bool fromDb)
        {
            MyUtilities.Url = $"Academics/StudentValues/Category/GetAll/{SessionData.ClientCode}";
            if (fromDb)
                return await request.GetAll<GsCat>(Url);

            return await request.GetAll<GsCat>(Url);
        }
        public async Task<List<ValueDefinitions>> LoadValueDefinitions(int id)
        {
            Url=$"Academics/StudentValues/Definition/GetByCaterogyId/{SessionData.ClientCode}/{id}";
            return await request.GetAll<ValueDefinitions>(Url);
        }
        public async Task<ApiResponse> LoadValueDefinition(int valueid)
        {
            Url = $"Academics/StudentValues/Definition/GetById/{SessionData.ClientCode}/{valueid}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<GsCat> _LoadGSCat(int categoryId)
        {
            Url=$"Academics/StudentValues/Category/GetById/{SessionData.ClientCode}/{categoryId}";
            return await request.GetSingle<GsCat>(Url);
        }
        #region StudentBehavior
        public async Task<ApiResponse> LoadBehaviorIncidentsAsync()
        {
            Url = $"Behaviours/Incidents/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadBehaviorIncidentAsync(int id)
        {
            Url = $"Behaviours/Incidents/{SessionData.ClientCode}/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadBehaviorActionAsync(int id)
        {
            Url = $"Behaviours/Incidents/{SessionData.ClientCode}/{id}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadBehaviorActionsAsync()
        {
            Url = $"Behaviours/Actions/{SessionData.ClientCode}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<BehaviorIncident> LoadBehaviorIncident(int id)
        {
            Url = $"BehaviourManagement/BehaviorIncident/GetById/{SessionData.ClientCode}/{id}";
            return await this.request.GetSingle<BehaviorIncident>(Url);
        }
        public async Task<ApiResponse> LoadReportedIncidentsCapturedAsync()
        {
            Url = $"Behaviours/Incident/Reported?schoolCode={SessionData.ClientCode}&incidentId={SessionData.UserId}&captured=true&reviewed=false&confirmed=false&resolved=false";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadReportedIncidentsReviewedAsync()
        {
            Url = $"Behaviours/Incident/Reported?schoolCode={SessionData.ClientCode}&incidentId={SessionData.UserId}&captured=true&reviewed=true&confirmed=false&resolved=false";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadReportedIncidentsConfirmedAsync()
        {
            Url = $"Behaviours/Incident/Reported?schoolCode={SessionData.ClientCode}&incidentId={SessionData.UserId}&captured=true&reviewed=true&confirmed=true&resolved=false";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<List<IncidentsByBehavior>> LoadReportedIncidentTopBehaviors(bool fromDb)
        {
            Url = $"BehaviourManagement/BehaviorAnalytics/GetTopBehaviors/{SessionData.ClientCode}";
            return fromDb ? await request.GetAll<IncidentsByBehavior>(Url) : await request.GetAll<IncidentsByBehavior>(Url);
        }
        public async Task<ApiResponse> LoadTotalReportedIncidentsByTermAsync()
        {
            Url = $"Behaviours/Analytics/GetIncidentsByTerm/{SessionData.ClientCode}/{DateTime.Now.Year}";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<ApiResponse> LoadIncidentsResolvedByTerm()
        {
            Url = $"Behaviours/Analytics/GetIncidentsResolvedByTerm/{SessionData.ClientCode}/{SessionData.Term}/{DateTime.Now.Year}";
            var response = await request.GetAsync(Url);

            return response;
        }   
        public async Task<ApiResponse> LoadIncidentsConfirmedByTerm()
        {
            Url = $"Behaviours/Analytics/GetIncidentsByConfirmationStatusByTerm/{SessionData.ClientCode}/{SessionData.Term}/{DateTime.Now.Year}";
            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadIncidentsUnReviewedByTerm()
        {
            Url = $"Behaviours/Analytics/GetUnReviewedCases/{SessionData.ClientCode}/{SessionData.Term}/{DateTime.Now.Year}";
            var response = await request.GetAsync(Url);

            return response;
        }

        public async Task<List<IncidentsByBehavior>> LoadIncidentsOpenByTerm()
        {
            Url = $"BehaviourManagement/BehaviorAnalytics/GetIncidentsByReviewStatusByTerm/{SessionData.ClientCode}/{SessionData.Term}/{DateTime.Now.Year}";
            return await request.GetAll<IncidentsByBehavior>(Url);
        }

        public async Task<List<IncidentsByBehavior>> LoadIncidentsInProgressByTerm()
        {
            Url = $"BehaviourManagement/BehaviorAnalytics/GetIncidentsByTerm/{SessionData.ClientCode}/{SessionData.Term}/{DateTime.Now.Year}";
            return await request.GetAll<IncidentsByBehavior>(Url);
        }
        public async Task<ApiResponse> LoadStudentCount()
        {
            Url = $"Students/Analytics/GetTotalStudents/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentPopByGender()
        {
            Url = $"Students/Analytics/GetStudentsByGenderDistribution/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentPopByClass()
        {
            Url = $"Students/Analytics/GetStudentsByClass/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadStudentPopByClassStream()
        {
            Url = $"Students/Analytics/StudentsByGenderDistributionPerClassAndStream/{SessionData.ClientCode}";

            var response = await request.GetAsync(Url);

            return response;
        }
        public async Task<ApiResponse> LoadReportedIncidentByIdAsync(int incidentid,bool captured,bool reviewed,bool confirmed,bool resolved)
        {
            Url = $"Behaviours/Incident/Reported?schoolCode={SessionData.ClientCode}&incidentId={incidentid}&captured={captured}&reviewed={reviewed}&confirmed={confirmed}&resolved={resolved}";
            var response = await request.GetAsync(Url);

            return response;
        }
        #endregion StudentBehavior
        #region Exam
        #endregion Exam
    }

    public abstract class BaseController : Controller
    {
        public SelectList LoadListItems(List<ListItems> l, bool load = false)
        {
           
            if (load)
                l.Insert(0, new ListItems() { Text ="Select", Value = "" });

            return new SelectList(l, "Value", "Text");
        }
        public void GetRequestDetails(HttpContext request, out string WindowsUser, out string IpAdress, out string WorkStation)
        {
            string[] ip = null;
            ip = RemoteMachine(request);
            IpAdress = ip[0];
            //ModuleName = request.Request.Method + ":" + request.Request.Path.ToString();
            WindowsUser = WindowsIdentity.GetCurrent().Name;
            WorkStation = ip[1];
        }

        public string[] RemoteMachine(HttpContext context)
        {
            var d = new string[2];
            d[0] = GetClientIPAddress(context);

            try { d[1] = Dns.GetHostEntry(d[0]).HostName; }
            catch { d[1] = d[0]; }
            return d;
        }
        public string GetClientIPAddress(HttpContext context)
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
            {
                ip = context.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            }
            return ip;
        }

    }
    
}
