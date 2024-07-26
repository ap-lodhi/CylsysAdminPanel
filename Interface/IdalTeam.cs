using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCylsys.Interface
{
    public interface IdalTeam
    {
        ResponseModel AddTeam(TeamModel objteam, string imgname, string imgpath);
        List<TeamModel> GetTeamList();
        ResponseModel DeleteTeam(int teamId);
        ResponseModel UpdateTeamData(TeamModel objTeam, string imgname, string imgpath);
        TeamModel GetTeamById(int id);
    }
}
