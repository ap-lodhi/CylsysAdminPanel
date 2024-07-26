using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCylsys.Interface
{
    interface RequestQuoteInterface
    {
        ResponseModel AddRequestQuote(RequestQuoteModel request);
        List<RequestQuoteModel> getRequestQuoteList();
        List<ServiceModel> GetServiceList();
        ResponseModel DeleteRequestQuote(int RequestQuoteId);
    }
}
