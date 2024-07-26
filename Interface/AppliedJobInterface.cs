using AdminCylsys.Models;

using System.Collections.Generic;

namespace AdminCylsys.Interface

{

    interface AppliedJobInterface

    {

        ResponseModel appliedJobs(AppliedJobModel jobModel, string imgname, string imgpath);

        List<AppliedJobModel> get_appliedPostList(int? id);

        List<PostUser> GetPostionList();

        ResponseModel DeleteappliedPost(int postId);

    }

}
