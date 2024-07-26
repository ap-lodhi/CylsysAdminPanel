using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCylsys.Interface
{   
    public interface ClientInterface
    {
        
        ResponseModel DeleteClient(int clientId);
        ResponseModel Addclient(clientModel cm, string imgname, string imgpath);
        List<clientModel> getClientList(int? id);
    }
}
