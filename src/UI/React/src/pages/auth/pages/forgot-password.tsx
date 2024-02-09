import { Link } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { setPageTitle } from "../../../store/themeConfigSlice";
import { authPaths, pageTitle } from "../../../utils/common/route-paths";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import { APIs } from "../../../utils/common/api-paths";
import messageService from "../../../utils/message.service";
import axiosInstance from "../../../utils/api.service";

const RecoverIdBox = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(setPageTitle(pageTitle.forgotPassword));
  });

  const submitForm = async (values: any) => {
    const response = await axiosInstance.post(APIs.forgotPasswordApi, {
      email: values.email,
    });
    if (response.data) {
      messageService.showMessage(response.data.data);
    }
  };

  const SubmittedForm = Yup.object().shape({
    email: Yup.string()
      .email("Invalid email")
      .required("This can not be empty"),
  });

  return (
    <div className="flex justify-center items-center min-h-screen bg-cover bg-center bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')]">
      <div className="panel sm:w-[480px] m-6 max-w-lg w-full">
        <h2 className="font-bold text-2xl mb-3">Password Reset</h2>
        <p className="mb-7">Enter your email to recover your ID</p>
        <Formik
          initialValues={{
            email: "",
          }}
          validationSchema={SubmittedForm}
          onSubmit={() => {}}
        >
          {({ errors, submitCount, touched, values }) => (
            <Form className="space-y-5">
              <div
                className={
                  submitCount
                    ? errors.email
                      ? "has-error"
                      : ""
                    : ""
                }
              >
                <label htmlFor="email">Email </label>
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
              <button
                type="submit"
                className="btn btn-primary w-full"
                onClick={() => {
                  if (touched.email && !errors.email) {
                    submitForm(values);
                  }
                }}
              >
                RECOVER
              </button>
            </Form>
          )}
        </Formik>
        <div className="relative my-7 h-5 text-center before:w-full before:h-[1px] before:absolute before:inset-0 before:m-auto before:bg-[#ebedf2] dark:before:bg-[#253b5c]">
          <div className="font-bold text-white-dark bg-white dark:bg-black px-2 relative z-[1] inline-block">
            <span>OR</span>
          </div>
        </div>
        <p className="text-center">
          Back To
          <Link
            to={"/" + authPaths.signin}
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
