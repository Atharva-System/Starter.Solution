import { Link, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { setPageTitle } from "../../store/themeConfigSlice";

const LoginBoxed = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle("Sign In"));
  });
  const navigate = useNavigate();

  const submitForm = () => {
    navigate("/");
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-cover bg-center bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')]">
      <div className="panel sm:w-[480px] m-6 max-w-lg w-full">
        <h2 className="font-bold text-2xl mb-3">Sign In</h2>
        <p className="mb-7">Enter your email and password to login</p>
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
          <div>
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              className="form-input"
              placeholder="Enter Password"
            />
          </div>
          <div>
            <label className="cursor-pointer">
              <Link
                to="/forgot-password"
                className="font-bold text-primary hover:underline ltr:ml-1 rtl:mr-1"
              >
                Forgot password
              </Link>
            </label>
          </div>
          <button type="submit" className="btn btn-primary w-full">
            SIGN IN
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginBoxed;
