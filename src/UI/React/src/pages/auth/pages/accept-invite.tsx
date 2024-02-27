import { Link, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useEffect, useState } from "react";
import { setPageTitle } from "../../../store/themeConfigSlice";
import { authPaths, pageTitle } from "../../../utils/common/route-paths";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import { APIs } from "../../../utils/common/api-paths";
import messageService from "../../../utils/message.service";
import axiosInstance from "../../../utils/api.service";
import { useLocation } from "react-router-dom";
import { FieldValidation, Regex } from "../../../utils/common/constants";
import { HttpStatusCode } from "axios";

const AcceptInvite = () => {
  const location = useLocation();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(setPageTitle(pageTitle.forgotPassword));
  });

  const [userDetails, setUserDetails] = useState<{
    firstName: string;
    lastName: string;
    email: string;
  }>({
    firstName: "",
    lastName: "",
    email: "",
  });

  useEffect(() => {
    const fetchUserDetails = async () => {
      const queryParams = new URLSearchParams(location.search);
      const userId = queryParams.get("userId");
      const response = await axiosInstance.get(APIs.getInviteDetails + userId);
      if (response.data) {
        if (response.data.statusCode === HttpStatusCode.Conflict) {
          messageService.showMessage(
            "Invitation has already been accepted!",
            "error"
          );
          setTimeout(() => {
            navigate("/" + authPaths.signin);
          }, 1000);
        } else {
          setUserDetails({
            firstName: response.data.data.firstName,
            lastName: response.data.data.lastName,
            email: response.data.data.email,
          });
        }
      }
    };

    fetchUserDetails();
  }, []);

  const submitForm = async (values: any) => {
    const queryParams = new URLSearchParams(location.search);
    const userId = queryParams.get("userId");

    const response = await axiosInstance.post(APIs.acceptInviteUserApi, {
      userId: userId,
      password: values.password,
    });
    if (response.data) {
      messageService.showMessage(response.data.data);
      navigate("/" + authPaths.signin);
    }
  };

  const SubmittedForm = Yup.object().shape({
    password: Yup.string()
      .required("This can not be empty")
      .min(
        FieldValidation.passwordMinLength,
        `Password must be at least ${FieldValidation.passwordMinLength} characters`
      )
      .matches(
        Regex.passwordValidationPattern,
        "Password requirements: At least one uppercase letter (A-Z), one lowercase letter (a-z), and one non-alphanumeric character."
      ),
  });

  return (
    <div className="flex justify-center items-center min-h-screen bg-cover bg-center bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')]">
      <div className="panel sm:w-[480px] m-6 max-w-lg w-full">
        <h2 className="font-bold text-2xl mb-3">Create An Account</h2>
        <Formik
          initialValues={{
            password: "",
          }}
          validationSchema={SubmittedForm}
          onSubmit={() => {}}
        >
          {({ errors, submitCount, touched, values }) => (
            <Form className="space-y-5">
              <span className="text-white-dark text-xs">
                Password requirements: At least one uppercase letter (A-Z), one
                lowercase letter (a-z), and one non-alphanumeric character.
              </span>
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                  <label htmlFor="firstName">First Name </label>
                  <input
                    name="firstName"
                    type="text"
                    id="firstName"
                    placeholder="Enter First Name"
                    className="form-input disabled-textbox"
                    value={userDetails.firstName}
                    disabled
                  />
                </div>
                <div>
                  <label htmlFor="lastName">Last Name </label>
                  <input
                    name="lastName"
                    type="text"
                    id="lastName"
                    placeholder="Enter Last Name"
                    className="form-input disabled-textbox"
                    value={userDetails.lastName}
                    disabled
                  />
                </div>
              </div>
              <div>
                <label htmlFor="email">Email </label>
                <input
                  name="email"
                  type="email"
                  id="email"
                  placeholder="Enter Email"
                  className="form-input disabled-textbox"
                  value={userDetails.email}
                  disabled
                />
              </div>
              <div
                className={
                  submitCount ? (errors.password ? "has-error" : "") : ""
                }
              >
                <label htmlFor="password">Password </label>
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
              <button
                type="submit"
                className="btn btn-primary w-full"
                onClick={() => {
                  if (touched.password && !errors.password) {
                    submitForm(values);
                  }
                }}
              >
                CREATE AN ACCOUNTAT
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
          Go To
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

export default AcceptInvite;
