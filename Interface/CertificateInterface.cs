using AdminCylsys.Models;
using System.Collections.Generic;

namespace AdminCylsys.Interface
{
    public interface CertificateInterface
    {
        ResponseModel Addcertificate(certificateModel cer, string imgname, string imgpath);

        List<certificateModel> getcertificateList(int? id);

    }

}

