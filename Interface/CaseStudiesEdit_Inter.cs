using AdminCylsys.Models;
using System.Collections.Generic;

namespace AdminCylsys.Interface
{
     public interface CaseStudiesEdit_Inter
    {
       
        //ResponseModel DeleteCase(int caseId);
        ResponseModel AddCaseStudies(CaseStudiesEdit_Mod cm, string fileName, string filePath);

        // List<CaseStudiesEdit_Mod> getcaseStudiesDetails();

        List<CaseStudiesEdit_Mod> GetCaseStudies();

        CaseStudiesEdit_Mod GetCaseStudyById(int id);
        ResponseModel UpdateCaseStudy(CaseStudiesEdit_Mod cm, string fileName, string imgpath);

        ResponseModel DisablePost(int id, bool Active);
    }

}

