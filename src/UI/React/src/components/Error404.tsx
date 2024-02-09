import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { Link } from "react-router-dom";
import { setPageTitle } from "../store/themeConfigSlice";

const Error404 = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle("Error 404"));
  });
  return (
    <div className="flex justify-center items-center min-h-screen bg-gradient-to-t from-[#c39be3] to-[#f2eafa]">
      <div className="text-center p-5 font-semibold">
        <h2 className="text-[50px] md:text-[80px] leading-none mb-8 font-bold">
          Error 404
        </h2>
        <h4 className="mb-5 font-semibold text-xl sm:text-5xl text-primary">
          Ooops!
        </h4>
        <p className="text-base">The page you requested was not found!</p>
        <Link to="/" className="btn btn-primary mt-10 w-max mx-auto">
          Home
        </Link>
      </div>
    </div>
  );
};

export default Error404;
