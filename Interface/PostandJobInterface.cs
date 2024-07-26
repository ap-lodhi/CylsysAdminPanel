using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCylsys.Interface
{
   public  interface PostandJobInterface
    {
        List<countrylist> GetCountryList();
        List<statelist> GetStateList(int? id);
        List<citylist> GetCityList(int? id);
        List<payrolllist> GetPayrollList();
        List<experiencelist> GetExperienceList();

        ResponseModel AddPostandJob(PostUser user);
        List<PostUser> getPostAndJobsList();
        PostUser GetPostById(int id);
        ResponseModel UpdatePostandJob(PostUser user);
        ResponseModel DisablePost(int id, bool Active);


        List<PostUser> getActiveJobsList();
    }
}
