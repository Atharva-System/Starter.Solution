import { Link, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { setPageTitle } from "../../store/themeConfigSlice";

const RecoverIdBox = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle("Forgot Password"));
  });
  const navigate = useNavigate();

  const submitForm = () => {
    navigate("/");
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-cover bg-center bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')]">
      <div className="panel sm:w-[480px] m-6 max-w-lg w-full">
        <h2 className="font-bold text-2xl mb-3">Password Reset</h2>
        <p className="mb-7">Enter your email to recover your ID</p>
        <form className="space-y-5" onSubmit={submitForm}>
          <div>
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              className="form-input"
              placeholder="Enter Email"
            />
          </div>
          <button type="submit" className="btn btn-primary w-full">
            RECOVER
          </button>
        </form>
        <div className="relative my-7 h-5 text-center before:w-full before:h-[1px] before:absolute before:inset-0 before:m-auto before:bg-[#ebedf2] dark:before:bg-[#253b5c]">
          <div className="font-bold text-white-dark bg-white dark:bg-black px-2 relative z-[1] inline-block">
            <span>OR</span>
          </div>
        </div>
        <p className="text-center">
          Back To
          <Link
            to="/sign-in"
            className="font-bold text-primary hover:underline ltr:ml-1 rtl:mr-1"
          >
            Sign In
          </Link>
        </p>
      </div>
    </div>
  );
};

export default RecoverIdBox;
