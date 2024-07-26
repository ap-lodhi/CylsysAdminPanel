using AdminCylsys.Models;
using System.Collections.Generic;
namespace AdminCylsys.Interface
{
    interface TestimonialInterface
    {
        ResponseModel DeleteTestimonial(int testimonialId);
        List<TestimonialModel> getTestimonialList();
        TestimonialModel GetTestimonialById(int id);
        ResponseModel Addtestimonial(TestimonialModel tm, string imgname, string imgpath);
        List<TestimonialModel> GetClientList();
        List<TestimonialModel> GetProjectList();
    }

}
