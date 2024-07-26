using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCylsys.Interface
{
    interface AnnouncementInterface
    {
        ResponseModel AddAnnouncement(AnnouncementModel am, string imgname, string imgpath);
        List<AnnouncementModel> getAnnouncementList(int? id);
        ResponseModel DeleteAnnouncement(int announcementId);
    }
}
