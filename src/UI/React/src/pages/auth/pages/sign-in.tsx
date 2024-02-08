import { Link, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { setPageTitle } from "../../../store/themeConfigSlice";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import authService from "../utils/auth.service";
import { pageTitle } from "../../../utils/common/route-paths";

const SignIn = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle(pageTitle.signin));
  });
  const navigate = useNavigate();

  const submitForm = (values: any) => {
    const { email, password } = values;

    const promise = authService.login(email, password);

    promise.then(() => {
      navigate("/");
    });
  };

  const SubmittedForm = Yup.object().shape({
    email: Yup.string().required("This can not be empty"),
    password: Yup.string().required("This can not be empty"),
  });

  return (
    <div className="flex justify-center items-center min-h-screen bg-cover bg-center bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')]">
      <div className="panel sm:w-[480px] m-6 max-w-lg w-full">
        <h2 className="font-bold text-2xl mb-3">Sign In</h2>
        <p className="mb-7">Enter your email and password to login</p>
        <Formik
          initialValues={{
            email: "",
            password: "",
          }}
          validationSchema={SubmittedForm}
          onSubmit={() => {}}
        >
          {({ errors, submitCount, values }) => (
            <Form className="space-y-5">
              <div
                className={submitCount ? (errors.email ? "has-error" : "") : ""}
              >
                <label htmlFor="email">Email</label>
                <Field
                  name="email"
                  type="text"
                  id="email"
                  placeholder="Enter Email"
                  className="form-input"
                />
                {submitCount ? (
                  errors.email ? (
                    <div className="text-danger mt-1">{errors.email}</div>
                  ) : (
                    ""
                  )
                ) : (
                  ""
                )}
              </div>
              <div
                className={
                  submitCount ? (errors.password ? "has-error" : "") : ""
                }
              >
                <label htmlFor="password">Password</label>
                <Field
                  name="password"
                  type="password"
                  id="password"
                  placeholder="Enter Password"
                  className="form-input"
                />
                {submitCount ? (
                  errors.password ? (
                    <div className="text-danger mt-1">{errors.password}</div>
                  ) : (
                    ""
                  )
                ) : (
                  ""
                )}
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
              <button
                type="submit"
                className="btn btn-primary w-full"
                onClick={() => {
                  if (Object.keys(errors).length === 0) {
                    submitForm(values);
                  }
                }}
              >
                SIGN IN
              </button>
            </Form>
          )}
        </Formik>
      </div>
    </div>
  );
};

export default SignIn;
