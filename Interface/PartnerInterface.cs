using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCylsys.Interface
{
   public interface PartnerInterface
    {
        ResponseModel AddPartner(PartnerModel pm);
        List<PartnerModel> PartnerList();
        List<OrganizationList> GetOrganizationList();
        ResponseModel DisablePartner(int id, bool Active);
    }
}
