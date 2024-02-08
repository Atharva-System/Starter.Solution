import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { setPageTitle } from "../../../store/themeConfigSlice";
import { pageTitle } from "../../../utils/common/route-paths";

const Users = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle(pageTitle.users));
  });

  return (
    <div className="panel">
      <h5 className="font-semibold text-lg dark:text-white-light mb-5">
        Users
      </h5>
    </div>
  );
};

export default Users;
