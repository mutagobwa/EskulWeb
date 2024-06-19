using Eskul.Custom;
using iText.IO.Image;
using Org.BouncyCastle.Utilities.IO.Pem;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Eskul.Models
{
    #region General 
    public class LoginModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ClientCode { get; set; }

    }
    public class Prefix
    {
        public string Code { get; set; }
        public string PrefixDesc { get; set; }    
        public string StatusId { get; set; }   
    }
    public class SessionDetail
    {
        public bool IsSignedIn { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string SchoolLogo { get; set; }
        public string FullNames { get; set; }
        public string ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string UserProfileCode { get; set; }
        public string UserBranchName { get; set; }
        public string UserBranchCode { get; set; }
        public string ClientCode { get; set; }
        public string Licence { get; set; }
        public string ProductCode { get; set; }
        public int BranchId { get; set; }
        public DateTime LcyDate { get; set; }
        public string UserWorkStation { get; set; }
        public string UserIPAddress { get; set; }
        public string WindowsUser { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public string UserrefNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoggedInSession { get; set; }
        public int StatusId { get; set; }
        public string ProfileCode { get; set; }
        
        public DateTime LastPassChange { get; set; }
        public bool ActiveSession { get; set; }
        public bool ChangePass { get; set; }
        public DateTime LastLogin { get; set; }
        public int FailedAttempts { get; set; }
        public bool Can_Add { get; set; }
        public bool Can_Update { get; set; }
        public bool Can_Delete { get; set; }
        public bool Pwd_Expires { get; set; }
        public DateTime LastActivity { get; set; }
        public string UserType { get; set; }
      
        public string ProfileCode1 { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Previous_Stp { get; set; }
        public string Current_Stp { get; set; }
        public string Next_Stp { get; set; }
        public string Mult_Institutional { get; set; }
        public string Term { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class ListItems
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
  
    public class PayloadDetails
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

    }


    public class Test
    {
        public Payload[] payLoad { get; set; }
    }

    public class Payload
    {
        public string RESPONSECODE { get; set; }
        public string RESPONSEMESSAGE { get; set; }
        public bool success { get; set; }
        public List<Payload> payLoad { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public dynamic PayLoad { get; set; }
    }
    public class _ApiResponse<T>
    {
        public bool success { get; set; }
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
        public List<T> payLoad { get; set; }
    }
    public class MyPayload
    {
        public bool All { get; set; }

        public bool IsActive { get; set; }
    }
    #endregion General
    #region Award
    public class acaward
    {
        public string awardcode { get; set; }
        public string awardname { get; set; }
        public string awardesc { get; set; }
        public int statusid { get; set; }
        public List<acaward> acawards { get; set; }
    }
    #endregion Award
    #region Accessright
    public class accessright
    {
        public decimal accessrightid { get; set; }
        public int progileid { get; set; }
        public int menuid { get; set; }
        public int statusid { get; set; }
    }
    #endregion Accessright
    #region Aclevel
    public class aclevel
    {
        public string levelcode { get; set; }
        public string levelname { get; set; }
        public string leveldesc { get; set; }
        public int statusid { get; set; }
    }
    #endregion Aclevel
    #region Analytics
    public class StudentGenderDistribution
    {
        public string Gender { get; set; }
        public int GenderCount { get; set; }
    }
    public class StudentCountByClassAndStream
    {
        public int Class { get; set; }
        public string StreamName { get; set; }
        public string Gender { get; set; }
        public int GenderCount { get; set; }
    }
    public class StudentClassDistribution
    {
        public string Class { get; set; }
        public int StudentCount { get; set; }
    }
    #endregion
    #region Auditrail
    public class audittrail
    {
        public decimal auditid { get; set; }
        public string userrefno { get; set; }
        public string modulename { get; set; }
        public string auditaction { get; set; }
        public DateTime auditdate { get; set; }
        public string windowsuser { get; set; }
        public string ipaddress { get; set; }
        public string workstation { get; set; }
        public int statusid { get; set; }
        public string hashcode { get; set; }
    }
    #endregion Auditrail
    #region Assignment
    public class AssignmentScoreViewModel : AssignmentModel
    {
        public string StreamCode { get; set; }
        public List<AssignmentStudentListModel> StudentsList { get; set; }
    }
    public class AssignmentStudentListModel
    {
        public string StreamName { get; set; }
        public string StreamCode { get; set; }
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int Score { get; set; }
        public int GenericSkillId { get; set; }
        public string Comment { get; set; }
    }
    public class GSkillDefinition
    {
        public double DefinitionId { get; set; }
        public string SchoolCode { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SkillDefinition { get; set; }
        public int StatusId { get; set; }
    }
    public class MarksAddList
    {
        public double ScoreId { get; set; }
        public double AssignmentId { get; set; }
        public string Year { get; set; }
        public int Term { get; set; }
        public string AssignmentName { get; set; }
        public string StudentId { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public int StatusId { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int Score { get; set; }
        public string GradeCode { get; set; }
        public string Comment { get; set; }
        public List<MarksAddList> StudentsList { get; set; }
        public Assignment assignments { get; set; }
        public ProjectAddModel projs { get; set; }
    }
    public class AssignmentVm
    {
        public subject Subject { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
    public class AssignmentModel
    {
        public decimal AssignmentId { get; set; }
        public string SchoolCode { get; set; }
        public string Year { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public decimal CompetencyId { get; set; }
        public string? Description { get; set; }
        public string Competency { get; set; }
        public decimal TopicId { get; set; }
        public string TopicName { get; set; }
        public int StatusId { get; set; }
    }
    public class SubjectAssignments
    {
        public SubjectModel Subject { get; set; }
        public List<AssignmentModel>? Assignments { get; set; }
        public string Year { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string AssignmentName { get; set; }
        public string competancy { get; set; }
        public string competency { get; set; }
        public string Description { get; set; }
        public int? PassMark { get; set; }
        public string Stream { get; set; }
        public string streamCode { get; set; }
        public string Initials { get; set; }
        public int StatusId { get; set; }
        public decimal AssignmentId { get; set; }
        public bool delete { get; set; }
        public string Level { get; set; }
        public decimal TopicId { get; set; }
        public decimal competencyId { get; set; }
        public string CompetencyDesc { get; set; }
        public string TopicName { get; set; }
        public string SubjectName { get; set; }
    }
    public class SubjectModel
    {
        public string SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectAbbrev { get; set; }
        public string? ExamLevel { get; set; }
        public bool IsCompulsory { get; set; }
        public int StatusId { get; set; }
    }
    public class AssignmentAdd
    {
        public decimal AssignmentId { get; set; }
        public string schoolCode { get; set; }
        public string year { get; set; }
        public string SubjectCode { get; set; }
        public string Stream { get; set; }
        public int term { get; set; }
        public int Class { get; set; }
        public string subject { get; set; }
        public int competencyId { get; set; }
        public string description { get; set; }
    }
    public class Assignment
    {
        public string Year { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string AssignmentName { get; set; }
        public string competancy { get; set; }
        public string competency { get; set; }
        public string Description { get; set; }
        public int? PassMark { get; set; }
        public string Stream { get; set; }
        public string Initials { get; set; }
        public int StatusId { get; set; }
        public decimal AssignmentId { get; set; }
        public bool delete { get; set; }
        public string  Level { get; set; }
        public decimal TopicId { get; set; }
        public decimal competencyId { get; set; }
        public string CompetencyDesc { get; set; }
        public string TopicName { get; set; }
        public string SubjectName { get; set; }
        public List<Assignment> assignments { get; set; }
    }

    #endregion Assignment
    #region StudentBehavior
    public class ReportedIncident
    {
        public Decimal IncidentID { get; set; }

        public string SchoolCode { get; set; }

        public string StudentId { get; set; }

        public string Year { get; set; }

        public int Term { get; set; }

        public int BehaviorID { get; set; }

        public string Remarks { get; set; }

        public DateTime? IncidentDate { get; set; }

        public DateTime? IncidentTime { get; set; }

        public string Location { get; set; }

        public string? Witnesses { get; set; }

        public string Category { get; set; }

        public string IncidentStatus { get; set; }

        public bool Captured { get; set; }

        public string CapturedBy { get; set; }

        public string DateCaptured { get; set; }

        public string ReviewReferral { get; set; }

        public bool Reviewed { get; set; }

        public string? DateReviewed { get; set; }

        public string? ReviewRemarks { get; set; }

        public string ConfirmReferral { get; set; }

        public bool Confirmed { get; set; }

        public string? DateConfirmed { get; set; }

        public string? ConfirmRemarks { get; set; }

        public int? ActionID { get; set; }

        public bool Resolved { get; set; }

        public string? ResolutionRemarks { get; set; }

        public string Level { get; set; }

        public int Class { get; set; }

        public string Stream { get; set; }

        public int StatusId { get; set; }

        public string? CapturedByName { get; set; }

        public string? ConfirmReferralName { get; set; }

        public string? ReviewReferralName { get; set; }

        public string? StudentName { get; set; }

        public List<ReportedIncident> reportedIncidents { get; set; }
    }
    public class BehaviorIncident
    {
        public int BehaviorID { get; set; }

        public string SchoolCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public int Rating { get; set; }

        public int StatusId { get; set; }

        public List<BehaviorIncident> behaviorIncidents { get; set; }
    }
    public class IncidentsByBehavior
    {
        public string Name { get; set; }

        public int Term { get; set; }

        public int ResolvedCount { get; set; }

        public Decimal ResolvedPercentage { get; set; }

        public bool Confirmed { get; set; }

        public int IncidentCount { get; set; }

        public int TotalIncidentCount { get; set; }
    }
    public class BehaviorAction
    {
        public int ActionID { get; set; }

        public string SchoolCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StatusId { get; set; }

        public List<BehaviorAction> behaviorActions { get; set; }
    }
    #endregion StudentBehavior
    #region StudentProjects
    public class ProjectAddModel
    {
        public decimal ProjectId { get; set; }
        public string Year { get; set; }
        public string StreamCode { get; set; }
        public string Level { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string SubjectCode { get; set; }
        public string ProjectName { get; set; }
        public string SchoolCode { get; set; }
        
        public string? Description { get; set; }
        public int StatusId { get; set; }
        public bool Delete { get; set; }
        public List<ProjectAddModel> Projects { get; set; }
    }
    public class ProjectViewModel : ProjectAddModel
    {
        public string SubjectName { get; set; }
    }
    public class ProjectVm
    {
        public string Year { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public List<Project> Projects { get; set; }
    }
    public class Project
    {
        public string SubjectName { get; set; }
        public string SchoolCode { get; set; }
        public double ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public bool Delete { get; set; }
    }
    public class SubjectProjects
    {
        public string Year { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string Level { get; set; }
        public int StatusId { get; set; }
        public decimal ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SubjectCode { get; set; }
        public string? Description { get; set; }
        public string SubjectName { get; set; }
        public string SchoolCode { get; set; }
        public List<ProjectViewModel>? Projects { get; set; }
    }
    public class ProjectScoreViewModel : ProjectViewModel
    {
        public List<ProjectStudentListModel> StudentsList { get; set; }
    }
    public class ProjectStudentListModel
    {
        public string StreamCode { get; set; }
        public string StreamName { get; set; }
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
    }
    public class ProjectScoreAdd
    {
        public decimal ProjectId { get; set; }
        public string StudentId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
    }
    #endregion StudentProjects
    #region ExamScore
    public class ExamScore
    {
        public decimal ScoreId { get; set; }
        public string YearCode { get; set; }
        public int TermCode { get; set; }
        public int ClassCode { get; set; }
        public string Stream { get; set; }
        public string StreamName { get; set; }
        public string ExamCode { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int Score { get; set; }
        public string GradeCode { get; set; }
        public string Comment { get; set; }
        public string Level { get; set; }
        public int Class { get; set; }
        public List<ExamScore> examScores { get; set; }
    }
    public class ExamScore_
    {
        public decimal ScoreId { get; set; }
        public string YearCode { get; set; }
        public int TermCode { get; set; }
        public int ClassCode { get; set; }
        public string Stream { get; set; }
        public string StreamName { get; set; }
        public string ExamCode { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int Score { get; set; }
        public string GradeCode { get; set; }
        public string Comment { get; set; }
        public string Level { get; set; }
        public int Class { get; set; }
        public List<ExamScore> examScores { get; set; }
    }
    public class SaveScore
    {
        public decimal ScoreId { get; set; }
        public string StudentId { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; }
    }

    #endregion ExamScore
    #region Bank
    public class BANK
    {
        public string BANKCODE { get; set; }
        public string BANKNAME { get; set; }
        public int STATUSID { get; set; }
    }
    #endregion Bank
    #region BloodGroup
    public class BloodGroup
    {
        public string GroupName { get; set; }
        public string GroupDesc { get; set; }
        public int StatusId { get; set; }
    }
    #endregion BloodGroup
    #region Branch
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }
        public int StatusId { get; set; }
        public bool HeadOffice { get; set; }
        public string SchoolCode { set; get; }
        public bool delete { get; set; }
        public List<Branch> Branches { get; set; }
}
    #endregion Branch
    #region bursary
    public class BursaryList
    {
        public string BursaryCode { get; set; }
        public string BursaryName { get; set; }
        public string BursaryDesc { get; set; }
        public int DiscountRate { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public int StatusId { get; set; }
    }
    public class BursaryVm
    {
        public string BursaryId { get; set; }
        public string Name { get; set; }
        public string BursaryDesc { get; set; }
        public int DiscountRate { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public int StatusId { get; set; }
        public bool delete { get; set; }
        public List<BursaryList> Bursaries { get; set; }
    }
    #endregion bursary
    #region citizentype
    public class citizentype
    {
        public int citizentypecode { get; set; }
        public string citizendesc { get; set; }
        public int statusid { get; set; }
    }
    #endregion citizentype
    #region class
    public class ClassVm
    {
        public int classcode { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public int statusid { get; set; }
        public bool delete { get; set; }
        public List<ClassList> classes { get; set; }
    }
    public class ClassList
    {
        public int classcode { get; set; }
        public string ClassName { get; set; }
        public int Class { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public int statusid { get; set; }
    }
    #endregion class
    #region Classteacher
    public class Classteacher
    {
        public decimal Id { get; set; }
        public string year { get; set; }
        public int Class { get; set; }
        public string ClassName { get; set; }
        public string stream { get; set; }
        public string SteamName { get; set; }
        public string SchoolCode { get; set; }
        public string tearcherid { get; set; }
        public bool delete { get; set; }
        public string Level { get; set; }
        public string FullName { get; set; }
        public List<Classteacher> classteachers { get; set; }
    }
    #endregion Classteacher
    #region country
    public class country
    {
        public string countrycode { get; set; }
        public string countryname { get; set; }
        public string nationality { get; set; }
    }
    #endregion country
    #region Definitions
    public class Definitions
    {
        public double DefinitionId { get; set; }

        public string? SchoolCode { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? SkillDefinition { get; set; }

        public int StatusId { get; set; }

        public List<Definitions>? definitions { get; set; }
    }
    #endregion Definitions
    #region Department
    public class DepartmentVm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SchoolCode { set; get; }
        public int StatusId { get; set; }
        public List<DepartmentVm> Departments { get; set; }
    }
    public class DepartmentList
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string SchoolCode { set; get; }
    }
    #endregion Department
    #region Designation
    public class Designation
    {
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string SchoolCode { get; set; }
        public bool delete { get; set; }
        public List<Designation> designations { get; set; }
    }
    #endregion Designation
    #region disabilitytype
    public class disabilitytype
    {
        public string disabilitycode { get; set; }
        public string disabilitydesc { get; set; }
        public int statusid { get; set; }
    }
    #endregion disabilitytype
    #region disabileReason
    public class disabileReason
    {
        public string reasonName { get; set; }
        public string reasonDesc { get; set; }
        public int reasonId { get; set; }
        public int StatusId { get; set; }
        public string SchoolCode { get; set; }

        public List<disabileReason> disabileReassons { get; set; }
    }
    #endregion disabileReason
    #region ExhibitedValues
    public class ExhibitedValues
    {
        public double studentValueId { get; set; }

        public string year { get; set; }

        public int Class { get; set; }

        public string Level { get; set; }

        public int Term { get; set; }

        public string Stream { get; set; }

        public string StudentId { get; set; }

        public string StudentNo { get; set; }

        public string? StudentName { get; set; }

        public int CountOfValues { get; set; }

        public string schoolCode { get; set; }

        public double valueId { get; set; }

        public string valueDescription { get; set; }

        public int valueRating { get; set; }

        public int categoryId { get; set; }

        public string categoryName { get; set; }

        public string comments { get; set; }

        public int statusId { get; set; }

        public List<ExhibitedValues> exhibitedValues { get; set; }
    }
    #endregion ExhibitedValues
    #region Disciplinary
    public class Disciplinary
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool delete { get; set; }
        public List<DisciplinaryList> Disciplinaries { get; set; }
    }
    public class DisciplinaryList
    {
        public string CaseCode { get; set; }
        public string CaseName { get; set; }
        public bool delete { get; set; }
        public int StatusId { get; set; }
    }
    #endregion Disciplinary
    #region district
    public class district
    {
        public int districtcode { get; set; }
        public string districtname { get; set; }
        public string regionname { get; set; }
    }
    #endregion district
    #region documenttype
    public class documenttype
    {
        public string documenttypecode { get; set; }
        public string documentname { get; set; }
        public int statusid { get; set; }
    }
    #endregion documenttype
    #region eotsummary
    public class eotsummary
    {
        public string Tablename { get; set; }
        public string Errormessage { get; set; }
        public string Errorcode { get; set; }
    }
    #endregion eotsummary
    #region eottable
    public class eottable
    {
        public int eodtableid { get; set; }
        public string tablename { get; set; }
        public string archivecolumns { get; set; }
        public string filterclause { get; set; }
    }
    #endregion eottable
    #region examgrade
    public class ExamGradeList
    {
        public int ExamGradeId { get; set; }
        public int Classcode { get; set; }
        public string GradeCode { get; set; }
        public int GradePoints { get; set; }
        public int PercentageFrom { get; set; }
        public int PercentageTo { get; set; }
        public int PercentagefTo { get; set; }
        public string GradeRank { get; set; }
        public string Comment { get; set; }
        public string GradeDescriptor { get; set; }
        public int StatusId { get; set; }
    }
    public class ExamGradeAdd
    {
        public int Class { get; set; }
        public string GradeCode { get; set; }
        public int? GradePoints { get; set; }
        public int? PercentageFrom { get; set; }
        public int? PercentageTo { get; set; }
        public int? PercentagefTo { get; set; }
        public string GradeRank { get; set; }
        public int ExamGradeId { get; set; }
        public string Comment { get; set; }
        public bool delete { get; set; }
        public string GradeDescriptor { get; set; }
        public List<ExamGradeList>  examGrades { get; set; }
    }
    #endregion examgrade
    #region ExamSettings
    public class ExamSetting
    {
        public int ExamsettingId { get; set; }
        public int Class { get; set; }
        public int Term { get; set; }
        public string Year { get; set; }
        public bool ApplyToAllClasses { get; set; }
        public string Exam { get; set; }
        public string ExamCode { get; set; }
        public int PassMark { get; set; }
        public bool delete { get; set; }
        public string SchoolCode { get; set; }
        public int StatusId { get; set; }
    }
    public class ExamSettingAdd
    {
        public int ExamsettingId { get; set; }
        public int Class { get; set; }
        public int Term { get; set; }
        public string Year { get; set; }
        public string Exam { get; set; }
        public string ExamCode { get; set; }
        public string SchoolCode { get; set; }
        public string Level { get; set; }
        public int? PassMark { get; set; }
        public bool delete { get; set; }
        public bool ApplyToAllClasses { get; set; }
        public List<ExamSetting> examSettings { get; set; }
    }
    #endregion ExamSettings
    #region examtype
    public class ExamTypeVm
    {
        public string ExamCode { get; set; }
        public string ExamName { get; set; }
        public string ExamDescription { get; set; }
        public bool delete { get; set; }
        public int StatusId { get; set; }
        public int TermCode { get; set; }
        public List<ExamTypeVm> Exams { get; set; }
    }
    #endregion examtype
    #region ExpenseType
    public class ExpenseType
    {
        public string ExpenseCode { get; set; }
        public string ExpenseName { get; set; }
        public string ExpenseDesc { get; set; }
        public bool delete { get; set; }
        public List<ExpenseType> expenseTypes { get; set; } 
    }
    #endregion ExpenseType
    #region FeesGroup
    public class FeesGroup
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string SchoolCode { get; set; }
        public int StatusId { get; set; }
    }
    public class FeesGroupViewModel
    {
        public FeesGroup feesGroup { get; set; }
        public List<FeesGroup> feesGroups  { get; set; }
    }
    #endregion FeesGroup
    #region Feemaster
    public class Feemaster
    {

        public decimal FeeMasterId { get; set; }
        public int BranchId { get; set; }
        public int ClassCode { get; set; }
        public string TypeCode { get; set; }
        public decimal Amount { get; set; }
        public string FineType { get; set; }
        public int Percentage { get; set; }
        public decimal FineAmount { get; set; }
        public bool delete { get; set; }
        public int Statusid { get; set; }
        public List<Feemaster> Feemasters { get; set; }
    }
    #endregion Feemaster
    #region FeesType
    public class FeesType
    {
        public string TypeCode { get; set; }
        public string GroupCode { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
        public string GlAccount { get; set; }
        public bool ApplyBursary { get; set; }
        public bool delete { get; set; }
        public int Statusid { get; set; }
        public List<FeesType> Feestypes { get; set; }
    }
    #endregion FeesType
    #region funding
    public class funding
    {
        public string fundcode { get; set; }
        public string fundname { get; set; }
        public string levelcode { get; set; }
        public int statusid { get; set; }
    }
    #endregion funding
    #region GeneralSettings
    public class GeneralSettingAdd
    {
        public string SchoolCode { set; get; }
        public string SchoolName { set; get; }
        public string EstablishedYear { set; get; }
        public string LocationAddress { set; get; }
        public string PostalAdress { set; get; }
        public string CountryCode { set; get; }
        public bool IsMultInstitutional { set; get; }
        public string RegistrationNo { set; get; }
        public bool IsUnebRegistered { set; get; }
        public string UNEBNo { set; get; }
        public string Contact { set; get; }
        public string Email { set; get; }
        public string Website { set; get; }
        public string DefaultLanguage { set; get; }
        public bool StudentNoAutGen { set; get; }
        public string StudentNoPrefix { set; get; }
        public bool StaffNoAutGen { set; get; }
        public string StaffNoPrefix { set; get; }
        public bool ParentNoAutGen { set; get; }
        public string ParentNoPrefix { set; get; }
        public bool TrRestrictedMode { set; get; }
        public string AcademicLevel { set; get; }
        public string Gender { set; get; }
        public string Sponser { set; get; }
        public string Logo { set; get; }
        public List<SysConfigVm> Generalconfigs { get; set; }
    }
    #endregion GeneralSettings
    #region gender
    public class gender
    {
        public int Genderid { get; set; }
        public string Gendercode { get; set; }
        public string Gendername { get; set; }
        public int Statusid { get; set; }
    }
    public class Addgender
    {
        public string Gendercode { get; set; }
        public string Gendername { get; set; }
        public int Statusid { get; set; }
    }
    #endregion gender
    #region GLAccount
    public class GLAccount
    {
        public int BranchId { get; set; }
        public string GlAccount { get; set; }
        public string GlAccountName { get; set; }
        public int SettingId { get; set; }
        public bool delete { get; set; }
        public List<GLAccount> gLAccounts { get; set; } 
    }

    #endregion GLAccount
    #region GSCAT
    public class GsCat
    {
        public int categoryId { get; set; }

        public string schoolCode { get; set; }

        public string categoryName { get; set; }

        public string categoryDescription { get; set; }

        public int statusId { get; set; }

        public List<GsCat> gsCats { get; set; }
    }
    #endregion GSCAT
    #region House
    public class HouseVm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SchoolCode { set; get; }
        public int StatusId { get; set; }
        public List<HouseVm> Houses { get; set; }
    }
    public class house
    {
        public string studentNo { get; set; }
        public string houseId { get; set; }
        public string SchoolCode { set; get; }
        public string? HouseName { get; set; }
    }
    #endregion House
    #region StudentHouse
    public class House
    {
        public string StudentNo { get; set; }
        public int houseId { get; set; }
    }
    #endregion StudentHouse 
    #region imagetype
    public class imagetype
    {
        public int imagetypeid { get; set; }
        public string imagetypedesc { get; set; }
        public int statusid { get; set; }
    }
    #endregion imagetype
    #region IncomeType
    public class IncomeType
    {
        public string IncomeCode { get; set; }
        public string IncomeName { get; set; }
        public string IncomeDesc { get; set; }
        public bool delete { get; set; }
        public List<IncomeType> incomeTypes { get; set; }   
    }

    #endregion IncomeType
    #region Library
    public class Book
    {
        public decimal BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookTittle { get; set; }
        public string BookNumber { get; set; }
        public string IsbnNumber { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Subject { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public string BookDescription { get; set; }
        public string RackNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public int Available { get; set; }
        public int StatusId { get; set; }
        public bool delete { get; set; }
        public string Level { get; set; }
    }
    public class LibraryMember
    {
        public string MembershipId { get; set; }
        public string AdmissionNo { get; set; }
        public string MemberType { get; set; }
        public string LibraryCardNo { get; set; }
    }
    public class LibraryMembers
    {
        public string MembershipNo { get; set; }
        public string AdmissionNo { get; set; }
        public string MemberType { get; set; }
        public string LibraryCardNo { get; set; }
        public int StatusId { get; set; }
        public string SchoolNumber { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string MemberType1 { get; set; }
        public List<LibraryMembers> libraryMembers { get; set; }    
    }
    public class IssueBook
    {
        public string MembershipId { get; set; }
        public decimal BookId { get; set; }
        [Display(Name = "Date Requested")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        public List<IssuedBooks> issuedBooks { get; set; }
        public List<LibraryMembers> libraryMembers { get; set; }

        public BookReturn BookReturn { get; set; }
        public LibraryMember libraryMember { get; set; }
        public List<NonLibStudents> Libstudents { get; set; }
    }
    public class BookReturn
    {
        public string Comment { get; set; }
        public int IssuedId { get; set; }
        public DateTime ReturnDate { get; set; }
    }
    public class IssuedBooks
    {
        public double IssueId { get; set; }
        public string MembershipNo { get; set; }
        public double BookId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime Due_ReturnDate { get; set; }
        public bool Returned { get; set; }
        public object Actual_ReturnDate { get; set; }
        public string Remark { get; set; }
        public int StatusId { get; set; }
        public string BookTittle { get; set; }
        public string BookNumber { get; set; }
    }
    #endregion Library
    #region MarksEntry
    public class MarkVm
    {
        public int StudentClass { get; set; }
        public string StudentSubject { get; set; }
        public string SubjectPaper { get; set; }
        public string Stream { get; set; }
        public string StudentNo { get; set; }
        public string Exam { get; set; }
        public int TermCode { get; set; }
        public string StudentNmae { get; set; }
        public string Level { get; set; }
        public List<Students> StudentList { get; set; }
        public List<StudentMarkList> studentMarkLists { get; set; }
    }
    public class StudentMarkList
    {
       
        public string StudentNo { get; set; }
        public int Score { get; set; }
        public string StudentName { get; set; }
    }
    #endregion MarksEntry
    #region menu
    public enum MenuCategory
    {
        Maintenance = 1,
        StaffManagement = 2,
        StudentManagement = 3,
        ParentManagement = 4,
        Academics = 5,
        Examination = 6,
        Accounts_and_Finance = 7,
        LibraryManagement = 8,
        BehaviourManagement = 9,
        HostelManagement = 10,
        Reports = 11,
        UserManagement = 12,
    }
    public class RightsSave
    {
        public bool Status { get; set; }
        public int statusId { get; set; }
        public int RightId { get; set; }
    }
        public class Menu
    {
        public int MenuId { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public int MenuLevel { get; set; }
        public bool Functional { get; set; }
        public bool Active { get; set; }
        public bool MainMenu { get; set; }
        public bool Status { get; set; }
        public decimal RightId { get; set; }
        public MenuCategory MenuCategory { get; set; }
        public string MenuAction { get; set; }
        public string MenuControler { get; set; }
        public string MenuCategoryName { get; set; }
        public List<Menu> Menuss { get; set; }
    }
    public class MenuViewModel
    {
        public IEnumerable<Menu> Menus { get; set; }
        public List<Menu> Menuss { get; set; }
    }
    #endregion menu
    #region menucategory
    public class menucategory
    {
        public int menucategoryid { get; set; }
        public string menucategoryname { get; set; }
        public string hashcode { get; set; }
    }
    #endregion menucategory
    #region Parent
    public class CreateParentApiModel
    {
        public string ParentNo { get; set; }
        public string ParentId { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Residence { get; set; }
        public string Nin { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string SchoolCode { get; set; }
        public ParentImage? Image { get; set; }
    }
    public class UpdateParentApiModel : CreateParentApiModel
    {
        public string ParentId { get; set; }
        public int StatusId { get; set; }
    }
    public class ParentInfo : UpdateParentApiModel
    {
        public DateTime EntryDate { get; set; }
    }
    public class ParentViewDTO : ParentInfo
    {
        public string FullName { get; set; }
        public List<ParentViewDTO> parents { get; set; }
    }

    public class ParentModel
    {
        public string Parentid { get; set; }
        public string Parentno { get; set; }
        public string Prefix { get; set; }
        public string Firstname { get; set; }
        public string? Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string? Nationality { get; set; }
        public string? Residence { get; set; }
        public string? Nin { get; set; }
        public string Contact1 { get; set; }
        public string? Contact2 { get; set; }
        public string? Email { get; set; }
        public string? Tribe { get; set; }
        public int Citizentype { get; set; }
        public string? Occupation { get; set; }
        public int Statusid { get; set; }
        public int BranchId { get; set; }
        public ParentImage? Image { get; set; }
        public bool Delete { get; set; }
    }
    public class ParentAdd
    {
        
        public string Parentid { get; set; }
        public string Parentno { get; set; }
        public string Prefix { get; set; }
        public string Firstname { get; set; }
        public string SchoolCode { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Initial { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string Residence { get; set; }
        public string Nin { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        public string Tribe { get; set; }
        public int Citizentype { get; set; }
        public string Occupation { get; set; }
        public ParentImage? Image { get; set; }
    }
    public class ParentEdit : ParentAdd
    {
        public bool delete { get; set; }
    }
    public class ParentImage
    {
        public string ParentNo { get; set; }
        public string Image64String { get; set; }
        public int ImageType { get; set; }
    }
    #endregion Parent
    #region passwordhistory
    public class passwordhistory
    {
        public decimal passwordhistoryid { get; set; }
        public string userrefno { get; set; }
        public string password { get; set; }
        public DateTime changedate { get; set; }
    }
    #endregion passwordhistory 
    #region PayMode
    public class PaymentMode
    {
        public string ModeCode { get; set; }
        public string ModeName { get; set; }
        public string ModeDesc { get; set; }
        public bool delete { get; set; }
        public List<PaymentMode> paymentModes { get; set; }
    }

    #endregion PayMode
    #region PayModeAcount
    public class PayModeAccount
    {
        public int BranchId { get; set; }
        public string PaymentMode { get; set; }
        public string AccountNo { get; set; }
        public int SettingId { get; set; }
        public bool delete { get; set; }
        public List<PayModeAccount> payModeAccounts { get; set; }   
    }
    #endregion PayModeAccount
    #region PositionTittle
    public class PositionTittle
    {
        public string titleCode { get; set; }
        public string titleName { get; set; }
        public string owner { get; set; }
        public bool delete { get; set; }
        public List<PositionTittle> positionTittles { get; set; }   
    }
    #endregion PositionTittle
    #region Profile
    public class MenuItem
    {
        public int MenuId { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public int MenuLevel { get; set; }
        public bool Functional { get; set; }
        public bool Active { get; set; }
        public bool MainMenu { get; set; }
        public bool Status { get; set; }
        public double RightId { get; set; }
        public int MenuCategory { get; set; }
        public string MenuCategoryName { get; set; }
        public string MenuAction { get; set; }
        public string MenuControler { get; set; }
    }

    public class MenuCategoryModel
    {
        public MenuCategory MenuCategory { get; set; }
        public List<Menu> Menus { get; set; }
        public List<MenuCategoryModel> MenuCategories { get; set; }
    }

    //public class MenuCategoryModel
    //{
    //    public MenuCategory MenuCategory { get; set; }
    //    public List<Menu> Menus { get; set; }
    //}
    public class MenuModel
    {
        public int MENUID { get; set; }
        public string MENUCODE { get; set; }
        public string MENUNAME { get; set; }
        public string MENUACTION { get; set; }
        public string MENUCONTROLLER { get; set; }
        public string MENUURL { get; set; }
        public int MENUCATEGORYID { get; set; }
        public int MENULEVEL { get; set; }
        public bool MAINMENU { get; set; }
        public bool FUNCTIONAL { get; set; }
        public bool ACTIVE { get; set; }
        public int STATUSID { get; set; }
    }

    public class ProfileVm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public bool delete { get; set; }
        public string SchoolCode { get; set; }
        public List<ProfileList> Profiles { get; set; }
    }
    public class ProfileList
    {
        public string SchoolCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
    }
    #endregion Profile
    #region relationship
    public class relationship
    {
        public int relationshipcode { get; set; }
        public string relationshipdesc { get; set; }
        public int statusid { get; set; }
    }
    #endregion relationship
    #region religion
    public class religions
    {
        public int religioncode   { get; set; }
        public string religionname { get; set; }
        public int statusid { get; set; }
    }
    #endregion religion
    #region report
    public class report
    {
        public int reportid { get; set; }
        public string reportname { get; set; }
        public string reportview { get; set; }
        public int reporttype { get; set; }
        public string groupfield { get; set; }
        public string displayfields { get; set; }
        public string reportfilter { get; set; }
        public string reportfilename { get; set; }
        public bool functional { get; set; }
    }
    public class reportfil
    {
        public int Branch { get; set; }
        public int BranchId { get; set; }
        public string Year { get; set; }
        public int Term { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public int ReportType { get; set; }
        public string resp { get; set; }
        public string Level { get; set; }
        
        public List<BioInfo> Studs { get; set; } 
    }
    public class BioInfo
    {
        public string StudentName { get; set; }
        public string FullName { get; set; }
        public string StreamName { get; set; }
        public string StudentNo { get; set; }
        public string StudentId { get; set; }
        public string NIN { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public int Term { get; set; }
        public string Year { get; set; }
        public int Branch { get; set; }
        public string BranchName { get; set; }
        public object StudentPhoto { get; set; }
        public List <BioInfo> bioInfos { get; set; }
    }
    public class ReportRes
    {
        public string StudentName { get; set; }
        public string StudentNo { get; set; }
        public string StudentId { get; set; }
        public string NIN { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public int Term { get; set; }
        public string Year { get; set; }
        public int Branch { get; set; }
    }
    public class ReportTemplate
    {
        public decimal TemplateId { get; set; }
        public int CurriculumType { get; set; }
        public int Class { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string CoreElective { get; set; }
        public int OrderLevel { get; set; }
        public bool delete { get; set; }
        public string Level { get; set; }
        public string CurriculumName { get; set; }
        public List<ReportTemplate> reportTemplates { get; set; }   
    }
    public class ReporTypeList
    {
        public int ReportCardId { get; set; }
        public string ReportCardName { get; set; }
        public string ReportCardDesc { get; set; }
        public int StatusId { get; set; }
        public int CurriculumType { get; set; }
        
    }
    public class GenerateReport
    {
        public string checkAll { get; set; }
        public int reportType { get; set; }
        public string year { get; set; }
        public int term { get; set; }
        public int classs { get; set; }
        public int stream { get; set; }
        public int branch { get; set; }
        public string studentId { get; set; }
    }
    public class ExamScoreSaveResponse
    {
        public decimal ScoreId { get; set; }
        public string SchoolCode { get; set; }
        public string StudentId { get; set; }
        public int Score { get; set; }
        public string Grade { get; set; }
        public string GradeCode { get; set; }
        
        public string Comment { get; set; }
    }
    #endregion report
    #region ResultCode
    public class ResultCodes
    {
        public string ResultCode { get; set; }

        public string ResultDesc { get; set; }

        public int StatusId { get; set; }
    }
    #endregion ResultCode
    #region SchoolSubjects
    public class SchoolSubjects
    {
        public bool Compulsory { get; set; }
        public List<SubjectPaper> SubjectPapers { get; set; }
        public string SchoolCode { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectAbbrev { get; set; }
        public string ExamLevel { get; set; }
        public string Level { get; set; }
        public int StatusId { get; set; }
        public List<SchoolSubjects> SchoolSubjectss { get; set; }
    }

    public class SubjectPaper
    {
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public int StatusId { get; set; }
    }
    #endregion SchoolSubjects
    #region section
    public class section
    {
        public string sectioncode { get; set; }
        public string sectionname { get; set; }
        public int statusid { get; set; }
    }
    #endregion section
    #region Securityparam
    public class securitypram
    {
        public int SYSCONFIGID { get; set; }
        public string SYSCONFIGVALUE { get; set; }
        public int parameterid { get; set; }
        public string parametername { get; set; }
        public string parametervalue { get; set; }
        public string parameterdesc { get; set; }
        public int statusid { get; set; }
        public bool delete { get; set; }
        public List<securitypram> securityparams { get; set; }
        public List<securitypramEdit> securitypramEdit { get; set; }
    }
    public class securitypramEdit
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public bool delete { get; set; }
    }

    public class SecuritySettings
    {
        public int Id { get; set; }
        public string SchoolCode { get; set; }
        public int AllowedFailedLogins { get; set; }
        public int PassExpiryDays { get; set; }
        public int PassHist { get; set; }
        public int PassLen { get; set; }
        public int PassNum { get; set; }
        public int PassScaps { get; set; }
        public int PassSchar { get; set; }
        public int PassUcaps { get; set; }
        public int SystemIdleTime { get; set; }
        public int UserLockDays { get; set; }
        public bool AllowAlphaNumUsernames { get; set; }
        public bool EnforceUniqueEmail { get; set; }
        public bool EnableUserLockout { get; set; }
        public bool AllowTwoFactorAuth { get; set; }
        public bool AllowTwoFactorAuthByPhone { get; set; }
        public bool AllowTwoFactorAuthByEmail { get; set; }
        public string DefaultUserPassword { get; set; }
        public List<SecuritySettings> securityparamSettings { get; set; }
    }
    public class SecurityParamAdd
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
    public class SecurityParamEdit : SecurityParamAdd
    {
        public int Id { get; set; }
        public bool delete { get; set; }
    }
    #endregion Securitypram
    #region SalaryScale
    public class SalaryScale
    {
        public string Code { get; set; }
        public string ScaleDesc { get; set; }
        public string Designation { get; set; }
        public double? Monthly { get; set; }
        public double? Annually { get; set; }
        public double? Annual { get; set; }
        public bool delete { get; set; }
        public List<SalaryScale> salaryScales{ get; set; }
    }
    #endregion SalaryScale
    #region staff_info
    public class StaffProfile
    {
        public StaffModel? StaffDetailsInfo { get; set; }
        public PayRollModel? PayRollInfo { get; set; }
        public LeaveAllocation? LeaveAllocationInfo { get; set; }
        public AttendanceModel? AttendanceInfo { get; set; }
    }
    public class StaffModel
    {
        public string StaffId { get; set; }
        public string StaffNo { get; set; }
        public string SchoolCode { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Initial { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string? NiN { get; set; }
        public string? Residence { get; set; }
        public string? PassPortNo { get; set; }
        public string Contact1 { get; set; }
        public string? Contact2 { get; set; }
        public string? Email { get; set; }
        public string? Tribe { get; set; }
        public CitizenType Citizentype { get; set; }
        public bool IsRegistered { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PositionTitle { get; set; }
        public string AcLevel { get; set; }
        public string PayType { get; set; }
        public string Category { get; set; }
        public int TermofEntry { get; set; }
        public int BranchId { get; set; }
        public DateTime? StartDate { get; set; }
        public string FullName { get; set; }
        public string? ContractType { get; set; }
        public string? CitizenTypeName { get; set; }
        public StaffImage? Image { get; set; }
        public List<StaffModel> Staffs { get; set; }

    }
    public enum CitizenType
    {
        ByBirth = 1,
        ByRegistration,
        ByPresumption,
        ByNaturalization,
        DualCitizenship
    }
    public class PayRollModel
    {
        public string StaffId { get; set; }
        public PayrollSummary? PayrollSummary { get; set; }
        public PayrollDetail? PayrollDetail { get; set; }
    }
    public class PayrollSummary
    {
        public decimal TotalNetSalary { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
    }
    public class PayrollDetail
    {
        public decimal PaySlipId { get; set; }
        public string PayPeriod { get; set; }
        public DateTime PayDate { get; set; }
        public string PayModel { get; set; }
        public string Status { get; set; }
        public decimal NextSalary { get; set; }
    }
    public class LeaveAllocation
    {
        public int NumberofDays { get; set; }
        public LeaveType LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public string EmployeeId { get; set; }
        public int period { get; set; }
    }
    public class LeaveRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime DateRequested { get; set; }
        public string RequestComments { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }
        public string RequetingEmployeeId { get; set; }
    }
    public class LeaveType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DefaultDays { get; set; }
    }
    public class AttendanceModel
    {
        public string Id { get; set; }
        public AttandanceSummary? AttandanceSummary { get; set; }
        public List<AttendanceDetails?> AttendanceDetails { get; set; }
    }
    public class AttendanceDetails
    {
        public string AttendanceId { get; set; }
        public string Year { get; set; }
        public int DateOfMonth { get; set; }
        public string January { get; set; }
        public string February { get; set; }
        public string March { get; set; }
        public string April { get; set; }
        public string May { get; set; }
        public string June { get; set; }
        public string July { get; set; }
        public string August { get; set; }
        public string September { get; set; }
        public string October { get; set; }
        public string November { get; set; }
        public string December { get; set; }
    }
    public class AttandanceSummary
    {
        public int TotalPresent { get; set; }
        public int TotalLate { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalHalfDay { get; set; }
    }
    public class StaffList
    {
        public string Staffid { get; set; }
        public string Staffno { get; set; }
        public string Prefix { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string FullName { get; set; }
        public string Lastname { get; set; }
        public string Initial { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string Residence { get; set; }
        public string Nin { get; set; }
        public string Tinno { get; set; }
        public string Nssfno { get; set; }
        public string Passportno { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        public string Tribe { get; set; }
        public int Citizentype { get; set; }
        public bool Registered { get; set; }
        public string Registrationno { get; set; }
        public string Maritalcode { get; set; }
        public string Designation { get; set; }
        public string Positiontitle { get; set; }
        public string Aclevel { get; set; }
        public string PayType { get; set; }
        public string Category { get; set; }
        public string Departmentcode { get; set; }
        public string Salaryscale { get; set; }
        public int Termofentry { get; set; }
        public string StaffId { get; set; }
        public string StaffNo { get; set; }
        //public string FullName { get; set; }
        public DateTime Entrydate { get; set; }
        public int Statusid { get; set; }
        public string Status { get; set; }
        public LibraryMember libraryMember { get; set; }
        public List<StaffList> staffLists { get; set; }
    }
    public class StaffAdd
    {
        public string Staffid { get; set; }

        public string Staffno { get; set; }

        public string Prefix { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string SchoolCode { get; set; }

        public string Initial { get; set; }

        public DateTime? Dob { get; set; }

        public DateTime? StartDate { get; set; }

        public string Gender { get; set; }

        public string Religion { get; set; }

        public string Nationality { get; set; }

        public string Residence { get; set; }

        public string Nin { get; set; }

        public string Tinno { get; set; }

        public string Nssfno { get; set; }

        public string Passportno { get; set; }

        public string Contact1 { get; set; }

        public string Contact2 { get; set; }

        public string Email { get; set; }

        public string Tribe { get; set; }

        public int Citizentype { get; set; }

        public bool Registered { get; set; }

        public string Registrationno { get; set; }

        public string Maritalcode { get; set; }

        public string Designation { get; set; }

        public string? Positiontitle { get; set; }

        public string Aclevel { get; set; }

        public string PayType { get; set; }

        public string Category { get; set; }

        public string Departmentcode { get; set; }

        public string Salaryscale { get; set; }

        public int BranchId { get; set; }

        [Required]
        public int Termofentry { get; set; }

        public bool delete { get; set; }

        public StaffImage staffImage { get; set; }
    }

    public class StaffImage
    {
        public string StaffNo { get; set; }
        public string Image64String { get; set; }
        public int ImageType { get; set; }
    }
    public class StaffImageList
    {
        public string StaffNo { get; set; }
        public string Image64String { get; set; }
        public int ImageType { get; set; }

    }
    public class NonLibStaff
    {
        public string StaffId { get; set; }
        public string StaffNo { get; set; }
        public string Contact1 { get; set; }
        public string Email { get; set; }
        public string StaffNo1 { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public List<NonLibStaff> NonLibStaffs { get; set; }
    }
    #endregion staff_info
    #region students
    public class CreateStudentDTO
    {
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Level { get; set; }
        public int Class { get; set; }
        public string? AcademicAward { get; set; }
        public string StreamCode { get; set; }
        public string Section { get; set; }
        public DateTime? DateRegistered { get; set; }
        public int TermRegistered { get; set; }
        public int BranchId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? DoB { get; set; }
        public string? NiN { get; set; }
        public string? ResultCode { get; set; }
        public string? PassportNo { get; set; }
        public string? Funding { get; set; }
        public string Type { get; set; }
        public string? Religion { get; set; }
        public string? Residence { get; set; }
        public string? Tribe { get; set; }
        public string HouseId { get; set; }
        public string SchoolCode { get; set; }
        public ParentAdd ParentAdd { get; set; }
        public StudentImage? Image { get; set; }
        public FormerSchoolDtls? FormerSchool { get; set; }
        public StudentUploadedDocument? Documents { get; set; }
        public StudentUploadedDocument? UploadedDocuments { get; set; }
        public StudentHouse? HouseInfo { get; set; }
        public StudentHealthDtl? HealthInfo { get; set; }
        public List<_StudentParentInfo?>? ParentInfo { get; set; }
        public List<StudentParentInfo?>? Parents { get; set; }
    }
    public class StudentUploadedDocument
    {
        public string? Owner { get; set; }
        public string? ReferenceNo { get; set; }
        public List<DocumentFile> DocumentFiles { get; set; }
    }
    public class DocumentFile
    {
        public string? DocumentStream { get; set; }
        public string? DocumentypeCode { get; set; }
        public string DocumentTypeDesc { get; set; }
    }
    public class _StudentParentInfo
    {
        public string StudentId { get; set; }
        public string ParentId { get; set; }
        public int RelationShip { get; set; }
    }
    public class StudentParentInfo
    {
        public string StudentId { get; set; }
        public string ParentId { get; set; }
        public string? ParentName { get; set; }
        public int RelationShip { get; set; }
        public string? RelationShipName { get; set; }
    }
    public class StudentHealthDtl
    {
        public string? StudentId { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public string? BloodGroup { get; set; }
        public bool Disabled { get; set; }
        public string? Disabilitycode { get; set; }
        public bool Chronic { get; set; }
        public string? MedHistory { get; set; }
        public int StatusId { get; set; }
    }
    public class StudentHouse
    {
        public string StudentNo { get; set; }
        public string HouseId { get; set; }
        public string? HouseName { get; set; }
    }
    public class PreviousSchoolDetail
    {
        public string? StudentId { get; set; }
        public string? SchoolName { get; set; }
        public string? AcademicAward { get; set; }
        public string? ResultCode { get; set; }
        public string? ChangeReason { get; set; }
        public List<FormerSchoolSubjectScore?>? Scores { get; set; }
    }
    public class FormerSchoolSubjectScore
    {
        public string? Subject { get; set; }
        public string? Score { get; set; }
    }
    public class FormerSchoolDtls
    {
        public string? StudentId { get; set; }
        public string? SchoolName { get; set; }
        public string? AcademicAward { get; set; }
        public string? ResultCode { get; set; }
        public string? ChangeReason { get; set; } 
        public List<FormerSchoolSubjectScore?>? Scores { get; set; } = default;
    }
    public class StudentImage
    {
        public string StudentNo { get; set; }
        public string Image64String { get; set; }
        public int ImageType { get; set; }
    }

    public class StudentAdd
    {
        public string Studentid { get; set; }
        public string Studentno { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Please select date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Nin { get; set; }
        public string Passportno { get; set; }
        public int Class { get; set; }
        public string SchoolCode { get; set; }
        public string StreamCode { get; set; }
        public string Level { get; set; }
        public string Section { get; set; }
        public string Funding { get; set; }
        public string Type { get; set; }
        public string Religion { get; set; }
        public bool Onbursary { get; set; }
        public string Bursarycode { get; set; }
        public string Nationality { get; set; }
        public int Districtcode { get; set; }
        public string Residence { get; set; }
        public bool Disabled { get; set; }
        public string Disabilitycode { get; set; }
        public string Tribe { get; set; }
        public string prefix { get; set; }
        public int Citizentype { get; set; }
        [Required(ErrorMessage = "Please select date Registered")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Dateregistered { get; set; }
        public int Termregistered { get; set; }
        public bool IsDisabled { get; set; }
        public int DisabledReasonId { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
        public string BloodGroup { get; set; }
        public string MedHistory { get; set; }
        public string awardcode { get; set; }
        public string documentypeCode { get; set; }
        public string ResultCode { get; set; }
        public string houseId { get; set; }
        public bool delete { get; set; }
        public int Statusid { get; set; }
        public house house { get; set; }
        public Image  Image { get; set; }
        public string parentid { get; set; }
        public string parentno { get; set; }
        public ParentAdd  ParentAdd { get; set; }
        public List<studentParentInfo?> ?studentParentInfo { get; set; }
        public formerSchool?  previousSchool { get; set; }
        public studentHealthDtl? studentHealthDtl { get; set; }
        public List<documents?>? documents { get; set; }

    }
    public class studentParentInfo
    {
        public string studentId { get; set; }
        public string parentid { get; set; }
        public int relationShip { get; set; }
        public string? RelationShipName { get; set; }
    }
    //for testing marks entry
    public class MarksUpdate
    {
            public string SchoolCode { get; set; }
            public double Score { get; set; }
            public string Comment { get; set; }
            public int GenericSkillId { get; set; }
            public decimal ProjectsId { get; set; }
        public string StudentId { get; set; }
            public decimal AssignmentId { get; set; }
    }
    public class ProjectScoreSaveRequest
    {
        public string SchoolCode { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public string StudentId { get; set; }
        public decimal ProjectId { get; set; }
    }
    public class _Subject
    {
        public string Name { get; set; }
        public int Papers { get; set; }
        public int PaperStatus { get; set; }
    }
    public class FormModel
    {
        public List<string> Classes { get; set; }
        public List<string> Streams { get; set; }
        public List<string> Students { get; set; }
        public List<_Subject> Subjects { get; set; }
    }
    public class _student
    {
        public string Name { get; set; }
        
        public Dictionary<string, int> Scores { get; set; }
        public List<_student> _students { get; set; }
        public List<_student> subject { get; set; }
    }
    public class Student
    {
        public string Name { get; set; }
        public string StudentNo { get; set; }
        public string Score { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
    }
    public class Studentss
    {
        public int St { get; set; }
        public List<Studentss> stdCounts  { get; set; }
    }
    //public List<StudentAdd> studentAddss { get; set; }
    public class StudentProfile
    {
        public StudentBasicInfo BasicInfo { get; set; }
        public Image? Image { get; set; }
        public house? House { get; set; }
        public studentHealthDtl? StudentHealthDtl { get; set; }
        public formerSchool? formerSchool { get; set; }
        public List<documents?>? documents { get; set; }
        public List<studentParentInfo?>? studentParentInfo { get; set; }
    }
    public class StudentBasicInfo
    {
        public string Studentid { get; set; }
        public string Studentno { get; set; }
        public string StudentName { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public string StreamName { get; set; }
        public string Level { get; set; }
        public string Section { get; set; }
        public DateTime Dateregistered { get; set; }
        public int Termregistered { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string FundingName { get; set; }
        public string Funding { get; set; }
        public string Studenttype { get; set; }
        public string Religion { get; set; }
        public bool Onbursary { get; set; }
        public string Bursary { get; set; }
        public string Bursarycode { get; set; }
        public string Nationality { get; set; }
        public string District { get; set; }
        public int Districtcode { get; set; }
        public string Residence { get; set; }
        public string Tribe { get; set; }
        public string CitizentypeName { get; set; }
        public int Citizentype { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
    }
    public class Students
    {
        public int Marks { get; set; }
        public string Studentid { get; set; }
        public string Studentno { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Remarks { get; set; }
        public int? DisableReasonId { get; set; }
        public string Status { get; set; }
        public string Lastname { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Nin { get; set; }
        public bool IsDisabled { get; set; }
        public string Passportno { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public string Section { get; set; }
        public string Funding { get; set; }
        public string Studenttype { get; set; }
        public string Religion { get; set; }
        public bool Onbursary { get; set; }
        public string Bursarycode { get; set; }
        public string Nationality { get; set; }
        public int Districtcode { get; set; }
        public string Residence { get; set; }
        public bool Disabled { get; set; }
        public string? Disabilitycode { get; set; }
        public string DisableReason { get; set; }
        public string Tribe { get; set; }
        public int Citizentype { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Dateregistered { get; set; }
        public int Termregistered { get; set; }
        public string Level { get; set; }
        public DateTime Entrydate { get; set; }
        public int CategoryId { get; set; }
        public int Category { get; set; }
        public string? Fullname { get; set; }
       
        //public int BranchId { get; set; }
        public string BloodGroup { get; set; }
        public string MedHistory { get; set; }
        public int Statusid { get; set; }
        public int BranchId { get; set; }
        
        //public Image? Image { get; set; }
        //public List<studentParentInfo?>? studentParentInfo { get; set; }
        //public PreviousSchool? previousSchool { get; set; }
        //public studentHealthDtl? studentHealthDtl { get; set; }
        //public List<uploadedDocument?>? uploadedDocument { get; set; }
        public List<StudentViewDTO>  students { get; set; }
        


    }
    public class DisabledStudent
    {
        public int? DisableReasonId { get; set; }
        public string Remarks { get; set; }
        public string? enableRemarks { get; set; }
        public string? ModifiedBy { get; set; }
        public string? StudentId { get; set; }
        public string StreamCode { get; set; }
        public string? Level { get; set; }
        public int Class { get; set; } 

    }
    public class DisabledStudentDetailsDTO
    {
        public bool IsDisabled { get; set; }
        public int? DisableReasonId { get; set; }
        public string DisableReason { get; set; }
        public string DisableReasonDesc { get; set; }
        public string Remarks { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class StudentViewDTO : StudentInfo
    {
        public string FullName { get; set; }
        public string StreamName { get; set; }
        public string BranchName { get; set; }
        public string? CategoryName { get; set; }
        public string? FundingDesc { get; set; }
        public string DisableReasonDesc { get; set; }
        public string StudentId { get; set; }
        public DisabledStudentDetailsDTO DisabledStudentDetails { get; set; }
        public StudentAllParentsInfoViewModel Parents { get; set; }
        public List<StudentViewDTO> students { get; set; }
    }
    public class StudentAllParentsInfoViewModel
    {
        public string StudentId { get; set; }
        public List<StudentParentInfoViewModel> Parents { get; set; }
    }
    public class StudentParentInfoViewModel
    {
        public ParentViewDTO? Parent { get; set; }
        public string RelationShip { get; set; }
        public string? RelationShipName { get; set; }
    }
    public class StudentInfo : UpdateStudentDTO
    {
        public DateTime EntryDate { get; set; }
        public int StatusId { get; set; }
    }
    public class UpdateStudentDTO : CreateStudentDTO
    {
        public string StudentId { get; set; }
        public bool IsDisabled { get; set; }
        public int? DisableReasonId { get; set; }
        public string Remarks { get; set; }
        public string EnableRemarks { get; set; }
    }
    public class _Student
    {
        public string Studentid { get; set; }
        public string Studentno { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Nin { get; set; }
        public string Passportno { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public string Level { get; set; }
        public string Section { get; set; }
        public string Funding { get; set; }
        public string Studenttype { get; set; }
        public string Religion { get; set; }
        public bool Onbursary { get; set; }
        public string Bursarycode { get; set; }
        public string Nationality { get; set; }
        public int Districtcode { get; set; }
        public string Residence { get; set; }
        public string Tribe { get; set; }
        public int Citizentype { get; set; }
        public DateTime Dateregistered { get; set; }
        public int Termregistered { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
        public Image Image { get; set; }
        public string House { get; set; }
        public string PreviousSchool { get; set; }
        public object uploadedDocument { get; set; }
        public studentHealthDtl studentHealthDtl { get; set; }
        public object studentParentInfo { get; set; }
        public int Statusid { get; set; }
        public bool Delete { get; set; }
    }
    public class StudFilter

    {
        public string Stream { get; set; }
        public string Class { get; set; }
    }
    public class NonLibStudents
    {
        public string StudentId { get; set; }
        public string StreamName { get; set; }
        public int CLass { get; set; }
        public string Stream { get; set; }
        public string ClassName { get; set; }
        public string StudentNo { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
    }
    public class StudentHouseAdd
    {
        public int HouseId { get; set; }
        public string StudentNo { get; set; }
    }
    public class StudentMember
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public string Stream { get; set; }
        public List<NonLibStudents> Libstudents { get; set; }
        public LibraryMember libraryMember { get; set; }
    }
    public class Image
    {
        public string studentNo { get; set; }
        public string image64String { get; set; }
        public int imageType { get; set; }
    }
    public class formerSchool
    {
        public string studentId { get; set; }
        public string schoolName { get; set; }
        public string academicAward { get; set; }
        public string resultCode { get; set; }
        public string changeReason { get; set; }
        public List<Scores> Scores { get; set; }
        
    }
    public class documents
    {
        public string owner { get; set; }
        public string referenceNo { get; set; }
        public string documentStream { get; set; }
        public string documentypeCode { get; set; }
    }
    public class BulkImport
    {
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string  FilePath { get; set; }
        public List<BulkImport> ImportFiles { get; set; }
    }
    public class studentHealthDtl
    {
        public string studentId { get; set; }
        public decimal weight { get; set; }
        public decimal height { get; set; }
        public string bloodGroup { get; set; }
        public bool disabled { get; set; }
        public string disabilitycode { get; set; }
        public bool chronic { get; set; }
        public string medHistory { get; set; }
        public int statusId { get; set; }
    }
    public class Scores 
    {
        public string Subject { get; set; }
        public string  score { get; set; }
    }
    public class StdAttendance
    {
        public int ClassId { get; set; }
        public string SteamCode { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string SchoolCode  { get; set; }
        public int Term { get; set; }
        public string Level { get; set; }
        public List<AttendenceList> AttendenceLists { get; set; }
    }
    public class AttendenceList
    {
        public double AttendanceId { get; set; }
        public int Term { get; set; }
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string steamCode { get; set; }
        public string StudentName { get; set; }
        public DateTime attendanceDate { get; set; }
        public string Status { get; set; }
        public string SchoolCode { get; set; }
        public int ClassId { get; set; }
        public string Remarks { get; set; }
    }
    #endregion students
    #region StudentSubjects
    public class StudentSubjectsViewDTO
    {
        public string Level { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public string StudentId { get; set; }
        public int BranchId { get; set; }
        public StudentViewDTO Student { get; set; }
        public List<SchoolSubject> Subjects { get; set; }
    }
    public class StudentSubjectsApiModel
    {
        public string SchoolCode { get; set; }
        public string StudentId { get; set; }
        public List<OfferedSubject> Subjects { get; set; }
    }
    public class OfferedSubject
    {
        public string SubjectCode { get; set; }
        public bool IsOffered { get; set; }
        public Dictionary<string, bool> Papers { get; set; }
    }
    public class StudentSubject
    {
        public string StudentId { get; set; }
        public string Level { get; set; }
        public int Class { get; set; }
        public int BranchId { get; set; }
        public string Stream { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public List<StudentSubjectPaper?>? SubjectPapers { get; set; }
        public List<StudentSubject> StudentSubjects { get; set; }
        public int StatusId { get; set; }
    }
    public class _StudentSubject
    {
        public string StudentId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public List<StudentSubjectPaper?>? SubjectPapers { get; set; }
        public int StatusId { get; set; }
    }
    public class StudentSubjectPaper
    {
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public int StatusId { get; set; }
    }


    #endregion StudentSubjects
    #region staffcategory
    public class staffcategory
    {
        public string categorycode { get; set; }
        public string categoryname { get; set; }
        public int statusid { get; set; }
    }
    #endregion staffcategory
    #region staffpaytype
    public class staffpaytype
    {
        public string Paytype_Code { get; set; }
        public string Paytype_Name { get; set; }
        public int StatusId { get; set; }
    }
    #endregion staffpaytype
    #region status
    public class status
    {
        public int statusid { get; set; }
        public string statusdesc { get; set; }
    }
    #endregion status
    #region ScoreDescriptor
    public class ScoreDescriptor
    {
        public int Class { get; set; }
        public int ClassCode { get; set; }
        public string Level { get; set; }
        public string scoreCode { get; set; }
        public string Identifier { get; set; }
        public int ?scorePoints { get; set; }
        public decimal? lowerScore { get; set; }
        public decimal? upperScore { get; set; }
        public string scoreRank { get; set; }
        public string comment { get; set; }
        public string scoreDescriptor { get; set; }
        public string  SchoolCode { get; set; }
        public int descriptorId { get; set; }
        public bool delete { get; set; }
        public List<ScoreDescriptor> scoreDescriptors { get; set; }
    }
    #endregion ScoreDescriptor
    #region stream
    public class stream
    {
        public string streamId { get; set; }
        public string name { get; set; }
        public string SchoolCode { set; get; }
        public int Class { get; set; }
        public int StudentLimit { get; set; }
        public bool delete { get; set; }
        public int StatusId { get; set; }
        public List<streamList> streams { get; set; } 
    }
    public class streamList
    {
        public string streamcode { get; set; }
        public string streamname { get; set; }
        public string StreamId { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }
        public string SchoolCode { set; get; }
        public int StudentLimit { get; set; }
        public int statusid { get; set; }
        public bool delete { get; set; }
    }
    #endregion stream
    #region student_info
    public class student_info
    {
        public string studentcode { get; set; }
        public string studentregno { get; set; }
        public string studentnumber { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string othername { get; set; }
        public DateTime dateofbirth { get; set; }
        public string gendercode { get; set; }
        public int classcode { get; set; }
        public string streamecode { get; set; }
        public string sectioncode { get; set; }
        public string fundcode { get; set; }
        public string stdcode { get; set; }
        public int religioncode { get; set; }
        public bool onbursary { get; set; }
        public string bursarycode { get; set; }
        public string nationality { get; set; }
        public int districtcode { get; set; }
        public string residence { get; set; }
        public string ninnumber { get; set; }
        public string passportnumber { get; set; }
        public string bloodgroup { get; set; }
        public string homecontact1 { get; set; }
        public string homecontact1person { get; set; }
        public string homecontact2 { get; set; }
        public string homecontact2person { get; set; }
        public bool disabled { get; set; }
        public string disabilitycode { get; set; }
        public string tribe { get; set; }
        public int citizentypecode { get; set; }
        public string formerschool { get; set; }
        public string formerschaward { get; set; }
        public string formergrade { get; set; }
        public bool isstaffrelated { get; set; }
        public DateTime regdate { get; set; }
        public string yearofentry { get; set; }
        public int termofentry { get; set; }
        public DateTime entrydate { get; set; }
        public DateTime lasteditdate { get; set; }
        public bool lasteditauto { get; set; }
        public string editcomment { get; set; }
        public bool IsDisabled { get; set; }
        public int CategoryId { get; set; }
        public int DisabledReasonId { get; set; }
        public int BranchId { get; set; }
        public int statusid { get; set; }
    }
    public class StudentAnalytic
    {
        public int TotalStudents { get; set; }
    }
    #endregion student_info
    #region studenttype
    public class studenttype
    {
        public string code { get; set; }
        public string codename { get; set; }
        public int statusid { get; set; }
    }
    #endregion studenttype
    #region StudentCategory
    public class StudentCategory
    {
        public string categoryName { get; set; }
        public string categoryDesc { get; set; }
        public int categoryId { get; set; }
        public int StatusId { get; set; }
        public string SchoolCode { get; set; }
        public List<StudentCategory> studentCategories { get; set; }
    }
    #endregion StudentCategory
    #region StudentValues
    public class ValueDefinitions
    {
        public double valueId { get; set; }

        public string? SchoolCode { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? valueDescription { get; set; }

        public int StatusId { get; set; }

        public List<ValueDefinitions>? valueDefinitions { get; set; }
    }
    #endregion StudentValues
    #region subject
    public class _SchoolSubject
    {
        public string SchoolCode { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public string SubjectAbbrev { get; set; }

        public string ExamLevel { get; set; }

        public bool Compulsory { get; set; }
        public bool IsCompulsory { get; set; }
        public bool isOffered { get; set; }

        public List<SchoolSubjectPaper?>? SubjectPapers { get; set; }

        public int StatusId { get; set; }
    }
    public class SchoolSubject
    {
        public string StudentId { get; set; }

        public string Level { get; set; }

        public int Class { get; set; }

        public string Stream { get; set; }

        public string SubjectAbbrev { get; set; }

        public bool Compulsory { get; set; }
        public bool IsCompulsory { get; set; }

        public string ExamLevel { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public List<SchoolSubjectPaper?>? SubjectPapers { get; set; }

        public List<SchoolSubject> SchoolSubjects { get; set; }

        public int StatusId { get; set; }
        public bool IsOffered { get; set; }
    }
    public class SchoolSubjectPaper
    {
        public string SubjectCode { get; set; }

        public string PaperCode { get; set; }

        public int StatusId { get; set; }
    }
    public class subject
    {
        public string Subjectcode { get; set; }
        public string Subjectname { get; set; }
        public string subjectAbbrev { get; set; }
        public bool Compulsory { get; set; }
        public bool Offered { get; set; }
        public string Examlevel { get; set; }
        public int Statusid { get; set; }
        public bool delete { get; set; }
        public List<subjectList> subjectLists { get; set; }
        public Editsubject SubEdit { get; set; }
    }
    public class subjectList
    {
    public string Subject_code { get; set; }
        public string Subjectcode { get; set; }
        public string Subject_name { get; set; }
        public string Subjectname { get; set; }
        public bool Compulsory { get; set; }
        public bool Offered { get; set; }
        public string Examlevel { get; set; }
        public string SubjectAbbrev { get; set; }
    }
    public class Addsubject
    {
        public string Subjectcode { get; set; }
        public string Subjectname { get; set; }
        public bool Compulsory { get; set; }
        public bool Offered { get; set; }
        public string Examlevel { get; set; }
    }
    public class Editsubject : subjectList
    {
        public bool delete { get; set; }
    }
    #endregion subject
    #region subjectpaper
    public class subjectpaper
    {
        public string PaperCode { get; set; }
        public string SubjectCode { get; set; }
        public string PaperName { get; set; }
        public bool Compulsory { get; set; }
        public bool Offered { get; set; }
        
        public bool delete { get; set; }
        public string Level { get; set; }
        public List<subjectpaperList> subjectPaperLists { get; set; }
    }
    public class subjectpaperList
    {
        public string Paper_Code { get; set; }
        public string PaperCode { get; set; }
        public string Subject_Code { get; set; }
        public string Paper_Name { get; set; }
        public string PaperName { get; set; }
        public bool Compulsory { get; set; }
        public bool Offered { get; set; }
    }
    #endregion subjectpaper
    #region Subjecteacher
    public class Subjecteacher
    {
        public decimal Id { get; set; }
        public string year { get; set; }
        public int Class { get; set; }
        public string ClassName { get; set; }
        public string Stream { get; set; }
        public string SteamName { get; set; }
        public string SchoolCode { get; set; }
        public string subjectcode { get; set; }
        public string papercode { get; set; }
        public string tearcherid { get; set; }
        public string SubjectName { get; set; }
        public bool ApplyToAllStreams { get; set; }
        public bool delete { get; set; }
        public string Level { get; set; }
        public string FullName { get; set; }
        public List<Subjecteacher> subjecteachers { get; set; }
    }
    #endregion Subjecteacher
    #region sysconfig
    public class SysConfigVm
    {
        public string SysconfigName { get; set; }
        public string SysconfigValue { get; set; }
        public string SysconfigDesc { get; set; }
        public int Id { get; set; }
        public bool delete { get; set; }
        public List<SysConfigVm> Sysconfigs { get; set; }
    }
    #endregion sysconfig
    #region sysparam
    public class SysParamVm
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool delete { get; set; }
        public int SysParamId { get; set; }
        public int Id { get; set; }
        public List<SysParamList> ParamLists { get; set; }

    }
    public class SysParamList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        
        public string Description { get; set; }
    }
    #endregion sysparam
    #region systemuser
    public class UserAdd
    {
        public string ReferenceNo { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string UserSession { get; set; }
        public bool ChangePassword { get; set; }
        public string ProfileCode { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool PasswordExpires { get; set; }
        public bool UseDefaultPassword { get; set; }
        public string UserType { get; set; }
        public bool IsAdmin { get; set; }
        public string SchoolCode { get; set; }
    }
    public class UserEdit : UserAdd
    {
        public int StatusId { get; set; }
    }
    public class UserModel : UserEdit
    {
        public DateTime LastPasswordChange { get; set; }
        public DateTime LastLogin { get; set; }

        public UserModel()
        {
            // Set default values for properties
            LastPasswordChange = DateTime.Now;
            LastLogin = DateTime.Now;
            StatusId = 3;
        }
    }
    public class ValidUser
    {
        public string SchoolCode { get; set; }
        public int UserType { get; set; }
        public string RegistrationNo { get; set; }
    }
    public class SystemUserDetails : UserModel
    {
        public int BranchId { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string UserId { get; set; }
        public string Term { get; set; }
        public int PreviousStp { get; set; }
        public int CurrentStp { get; set; }
        public int NextStp { get; set; }
        public int MultInstitutional { get; set; }
        public string SchoolLogo { get; set; }
        public string ClientCode { get; set; }
        public string ProductCode { get; set; }
        public string LicenceCode { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class UsersDet
    {
        public string UserrefNo { get; set; }
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }
        public string SchoolLogo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoggedInSession { get; set; }
        public int StatusId { get; set; }
        public string ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public string ClientCode { get; set; }
        public string ProductCode { get; set; }
        public string LicenceCode { get; set; }
        public DateTime LastPassChange { get; set; }
        public bool ActiveSession { get; set; }
        public bool ChangePass { get; set; }
        public DateTime LastLogin { get; set; }
        public int FailedAttempts { get; set; }
        public bool Can_Add { get; set; }
        public bool Can_Update { get; set; }
        public bool Can_Delete { get; set; }
        public bool Pwd_Expires { get; set; }
        public DateTime LastActivity { get; set; }
        public string UserType { get; set; }
        public int BranchId { get; set; }
        public string ProfileCode1 { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string PreviousStp { get; set; }
        public string CurrentStp { get; set; }
        public string NextStp { get; set; }
        public string MultInstitutional { get; set; }
        public string Term { get; set; }

    }
    public class UserS
    {
        public string Userrefno { get; set; }
        public string UserName { get; set; }
        public string ReferenceNo { get; set; }
        public string Password { get; set; }
        public string LoggedinSession { get; set; }
        public bool ChangePassword { get; set; }
        public string Level { get; set; }
        public int Class { get; set; }
        public string StreamCode { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public DateTime LastLogin { get; set; }
        public string ProfileCode { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool IsAdmin { get; set; }
        public bool CanDelete { get; set; }
        public int StatusId { get; set; }
        public string  Term{ get; set; }
        public string UserType { get; set; }
        public List<UserS>  userS { get; set; }
    }
    public class UserByProfType
    {
        public string UserType { get; set; }
        public string ProfileCode { get; set; }
    }
    public class ChangePass
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string RawPassword { get; set; }
    }
    #endregion systemuser
    #region Termsession
    public class TermsessionVm
    {
        public int TermSessionId { get; set; }
        public string Year { get; set; }
        public int TermCode { get; set; }
        [Required(ErrorMessage = "Please select a start  date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Please select  end date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndDate { get; set; }
        public int StatusId { get; set; }
        public string SchoolCode { set; get; }
        public bool delete { get; set; }
        public List<TermsessionList> Termsessions { get; set; }
    }
    public class TermsessionList
    {
        public int TermSessionId { get; set; }
        public string Year { get; set; }
        public int TermCode { get; set; }
        [Required(ErrorMessage = "Please select a start  date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Please select  end date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndDate { get; set; }
        public int StatusId { get; set; }
        public string SchoolCode { set; get; }
        public bool delete { get; set; }
    }
    #endregion Termsession
    #region uceresult
    public class uceresult
    {
        public string resultcode { get; set; }
        public string resultdesc { get; set; }
        public int statusid { get; set; }
    }
    #endregion uceresult
    #region Curriculum
    public class Curriculum
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion Curriculum
    #region Tests
    public class PayslipModel
    {
        public int StudentNo { get; set; }
        public string StudentName { get; set; }
        public string SlipNo { get; set; }
        public string Branch { get; set; }
        public string Term { get; set; }
        public string PayMode { get; set; }
        public string Studyear { get; set; }
        public string PayslipImage { get; set; }
    }
    #endregion Tests
    #region Topic
    public class SubjectTopics: TopicMasterModel
    {
        public SubjectModel Subject { get; set; }
        public List<TopicMasterModel> Topics { get; set; }
    }
    public class TopicMasterModel
    {
        public decimal TopicId { get; set; }
        public string SchoolCode { get; set; }
        public string SubjectCode { get; set; }
        public string Level { get; set; }
        public string Year { get; set; }
        public int ClassId { get; set; }
        public string TopicName { get; set; }
        public int Period { get; set; }
        public int StatusId { get; set; }
    }
    public class SubjectTopicComp
    {
        public TopicMasterModel Topic { get; set; }
        public SubjectModel Subject { get; set; }
        public List<Compitence> Competencies { get; set; }
    }
    public class Compitence 
    {
        public decimal CompetencyId { get; set; }
        public string Year { get; set; }
        public string SchoolCode { get; set; }
        public decimal TopicId { get; set; }
        public string Competency { get; set; }
        public string? TopicName { get; set; }
        public string SubjectName { get; set; }
        public string SubCode { get; set; }
        public int ClassId { get; set; }
        public int StatusId { get; set; }
        public bool Delete { get; set; }
    }
    public class SubjectTopicsModel
    {
        public SubjectModel Subject { get; set; }
        public List<TopicMasterModel> Topics { get; set; }
    }
    public class TopicMaster
    {
        public decimal TopicId { get; set; }
        public string year { get; set; }
        public int ClassId { get; set; }
        public string SubjectCode { get; set; }
        public string? subjectName { get; set; }
        public string TopicName { get; set; }
        public int Period { get; set; }
        public string SchoolCode { get; set; }
        public string Level { get; set; }
        public int statusId { get; set; }
        public bool delete { get; set; }
        public List<TopicMaster> Topics { get; set; }
        public List<Compitence> CompetencyList { get; set; }
        public Compitence _compitence { get; set; }
    }       
    public class SubTopic 
    {
        public decimal TopicId { get; set; }
        public string year { get; set; }
        public int ClassId { get; set; }
        public string SubCode { get; set; }
        public string? subjectName { get; set; }
        public string TopicName { get; set; }
        public int Period { get; set; }
        public string Level { get; set; }
        public int statusId { get; set; }
        public bool delete { get; set; }
        public List<SubTopic> SubTopics { get; set; }
    }
    public class TopicsCompetencies: TopicMasterModel
    {
        public TopicMasterModel Topic { get; set; }
        public SubjectModel Subject { get; set; }
        public Compitence _compitence { get; set; }
        public List<TopicCompetencyModel> Competencies { get; set; }
    }
    public class TopicCompetencyModel
    {
        public decimal CompetencyId { get; set; }
        public string SchoolCode { get; set; }
        public decimal TopicId { get; set; }
        public string Competency { get; set; }
        public int StatusId { get; set; }
    }
    //delete after
    public class TopicCompetencyListModel : TopicMaster
    {
        public List<Compitence>? CompetencyList { get; set; }
    }
    #endregion Topic
    #region ScoreRank
    public class ScoreRank
    {
        public decimal ScoreRankId { get; set; }
        public int ClassCode { get; set; }
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }
        public string RankCode { get; set; }
        public int Points { get; set; }
        public string Level { get; set; }
        public int StatusId { get; set; }
        public CommentMaster commentmaster { get; set; }
        public List<CommentMaster> Comments { get; set; }
        public List<ScoreRank> ScoreRanks { get; set; }
    }
    public class CommentMaster
    {
        public decimal CmId { get; set; }
        public int ScoreRankId { get; set; }
        public string Comment { get; set; }
        public int StatusId { get; set; }
    }
    #endregion ScoreRank
    #region Hostel
    public class Hostel
    {
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        //public string Address { get; set; }
        //public int Intake { get; set; }
        //public bool Type { get; set; }
        public int StatusId { get; set; }
        public string SchoolCode { get; set; }
        public List<Hostel> Hostels { get; set; }

    }
    public class HostelRoom
    {
        public int RoomID { get; set; }
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public string SchoolCode { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string Gender { get; set; }
        public int Capacity { get; set; }
        public int UsedSpace { get; set; }
        public int AvailableSpace { get; set; }
        public int StatusId { get; set; }
        public List<HostelRoom> ?HostelRooms { get; set; }
    }
    public class RoomBeds
    {
        public int BedId { get; set; }
        public int RoomId { get; set; }
        public string BedName { get; set; }
    }
    #endregion Hostel
    #region Exams
    //public class MarksList
    //{
    //    public string Year { get; set; }
    //    public int Term { get; set; }
    //    public int ClassCode { get; set; }
    //    public string StreamCode { get; set; }
    //    public string StreamName { get; set; }
    //    public string ExamCode { get; set; }
    //    public List<SubjectModel> Subjects { get; set; }
    //    public List<StudentMarksList> StudentMarks { get; set; }
    //}
    public class MarksList
    {
        public string StudentId { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string ExamCode { get; set; }
        public string Score { get; set; }
        public int TermCode { get; set; }
        public int ClassCode { get; set; }
        public int Class { get; set; }
        public int Branch { get; set; }
        public string Year { get; set; }
        public string Level { get; set; }
        public string Stream { get; set; }
        public string subjectcode { get; set; }
        public string StudentName { get; set; }
        public List<StudentMarksModel> StudentMarks { get; set; }
        public List<MarksListModel> MarksLists { get; set; }
        public List<OfferedPapersModel> OfferedPapers { get; set; }
        public MarksList marksList { get; set; }
    }
    public class ExamMarksList
    {
        public string Year { get; set; }
        public int Term { get; set; }
        public int ClassCode { get; set; }
        public int Class { get; set; }
        public string Stream { get; set; }
        public string StreamCode { get; set; }
        public string StreamName { get; set; }
        public string ExamCode { get; set; }
        public int TermCode { get; set; }
        public string Level { get; set; }
        public List<SubjectModel> Subjects { get; set; }
        public List<StudentMarksList> StudentMarks { get; set; }
    }
    public class StudentMarksList
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public List<MarksListModel> MarksLists { get; set; }
    }

    public class StudentMarksModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public List<MarksListModel> MarksLists { get; set; }
    }

    public class MarksListModel
    {
        public string StudentId { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string Score { get; set; }
    }

    public class OfferedPapersModel
    {
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
    }
    public class SubmarkVm
    {
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public int TermCode { get; set; }
        public int branch { get; set; }
        public string ExamCode { get; set; }
        public int ClassCode { get; set; }
        public int Class { get; set; }
        public string Year { get; set; }
        public string Level { get; set; }
        public string Stream { get; set; }
        public List<Submarklist>  submarklists { get; set; }
    }
    public class Submarklist
    {
        public double ScoreId { get; set; }
        public string YearCode { get; set; }
        public int TermCode { get; set; }
        public int ClassCode { get; set; }
        public string Stream { get; set; }
        public string SubjectCode { get; set; }
        public string PaperCode { get; set; }
        public string ExamCode { get; set; }
        public string StudentId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public string Score { get; set; }
        public string SubjectName { get; set; }
    }
    #endregion Exams
}
