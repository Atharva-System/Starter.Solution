import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { setPageTitle } from "../../../store/themeConfigSlice";
import { pageTitle } from "../../../utils/common/route-paths";

const Profile = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle(pageTitle.profile));
  });

  return (
    <div className="pt-5">
      <form className="border border-[#ebedf2] dark:border-[#191e3a] rounded-md p-4 mb-5 bg-white dark:bg-black">
        <h6 className="text-lg font-bold mb-5">My Profile</h6>
        <div className="flex flex-col sm:flex-row">
          <div className="ltr:sm:mr-4 rtl:sm:ml-4 w-full sm:w-2/12 mb-5">
            <img
              src="/assets//images/default_profile.png"
              alt="img"
              className="w-20 h-20 md:w-32 md:h-32 rounded-full object-cover mx-auto"
            />
          </div>
          <div className="flex-1 grid grid-cols-1 sm:grid-cols-2 gap-5">
            <div>
              <label htmlFor="name">First Name</label>
              <input
                id="name"
                type="text"
                placeholder="Enter First Name"
                className="form-input"
              />
            </div>
            <div>
              <label htmlFor="profession">Last Name</label>
              <input
                id="profession"
                type="text"
                placeholder="Enter Last Name"
                className="form-input"
              />
            </div>
            <div>
              <label htmlFor="location">Email</label>
              <input
                id="location"
                type="text"
                placeholder="Enter Email"
                className="form-input"
              />
            </div>
            <div className="sm:col-span-2 mt-3">
              <button type="button" className="btn btn-primary">
                Save
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default Profile;
