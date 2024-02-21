import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { setPageTitle } from "../../../store/themeConfigSlice";
import { pageTitle } from "../../../utils/common/route-paths";
import axiosInstance from "../../../utils/api.service";
import { APIs } from "../../../utils/common/api-paths";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { updateUserInfo } from "../../../store/userInfoSlice";
import LocalStorageService from "../../../utils/localstorage.service";
import { FieldValidation, Regex } from "../../../utils/common/constants";

const localStorageService = LocalStorageService.getService();

const Profile = () => {
  const dispatch = useDispatch();

  const [profileDetails, setProfileDetails] = useState<{
    id: string;
    firstName: string;
    lastName: string;
    email: string;
  }>({
    id: "",
    firstName: "",
    lastName: "",
    email: "",
  });
  const [originalObj, setOriginalObj] = useState<string>("");

  useEffect(() => {
    dispatch(setPageTitle(pageTitle.profile));

    const fetchProfileDetails = async () => {
      const response = await axiosInstance.get(APIs.getProfileDetails);
      if (response.data) {
        setProfileDetails({
          id: response.data.data.id,
          firstName: response.data.data.firstName,
          lastName: response.data.data.lastName,
          email: response.data.data.email,
        });
        setOriginalObj(
          JSON.stringify({
            id: response.data.data.id,
            firstName: response.data.data.firstName,
            lastName: response.data.data.lastName,
            email: response.data.data.email,
          })
        );
      }
    };

    fetchProfileDetails();
  }, [dispatch]);

  const submitForm = async (values: any) => {
    const response = await axiosInstance.put(
      APIs.updateProfile + values.id,
      values
    );

    if (response.data) {
      dispatch(
        updateUserInfo({
          fullName: values.firstName + " " + values.lastName,
          email: values.email,
        })
      );
      localStorageService.updateStorageUserInfo(
        values.firstName + " " + values.lastName,
        values.email
      );
      setOriginalObj(
        JSON.stringify({
          id: profileDetails.id,
          firstName: values.firstName,
          lastName: values.lastName,
          email: values.email,
        })
      );
      messageService.showMessage(response.data.data);
    }
  };

  const SubmittedForm = Yup.object().shape({
    firstName: Yup.string()
      .required("This can not be empty")
      .matches(Regex.noSpaceValidationPattern, "Invalid First Name")
      .max(
        FieldValidation.firstNameMaxLength,
        `First name must be at most ${FieldValidation.firstNameMaxLength} characters`
      ),
    lastName: Yup.string()
      .required("This can not be empty")
      .matches(Regex.noSpaceValidationPattern, "Invalid Last Name")
      .max(
        FieldValidation.lastNameMaxLength,
        `Last name must be at most ${FieldValidation.lastNameMaxLength} characters`
      ),
    email: Yup.string()
      .email("Invalid email")
      .required("This can not be empty")
      .max(
        FieldValidation.emailMaxLength,
        `Email must be at most ${FieldValidation.emailMaxLength} characters`
      ),
  });

  return (
    <div className="pt-5">
      <Formik
        enableReinitialize={true}
        initialValues={{
          id: profileDetails.id,
          firstName: profileDetails.firstName,
          lastName: profileDetails.lastName,
          email: profileDetails.email,
        }}
        validationSchema={SubmittedForm}
        onSubmit={() => {}}
      >
        {({ errors, submitCount, values }) => (
          <Form className="border border-[#ebedf2] dark:border-[#191e3a] rounded-md p-4 mb-5 bg-white dark:bg-black">
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
                <div
                  className={
                    submitCount ? (errors.firstName ? "has-error" : "") : ""
                  }
                >
                  <label htmlFor="firstName">First Name</label>
                  <Field
                    name="firstName"
                    type="text"
                    id="firstName"
                    placeholder="Enter First Name"
                    className="form-input"
                  />
                  {submitCount ? (
                    errors.firstName ? (
                      <div className="text-danger mt-1">{errors.firstName}</div>
                    ) : (
                      ""
                    )
                  ) : (
                    ""
                  )}
                </div>
                <div
                  className={
                    submitCount ? (errors.lastName ? "has-error" : "") : ""
                  }
                >
                  <label htmlFor="lastName">Last Name</label>
                  <Field
                    name="lastName"
                    type="text"
                    id="lastName"
                    placeholder="Enter Last Name"
                    className="form-input"
                  />
                  {submitCount ? (
                    errors.lastName ? (
                      <div className="text-danger mt-1">{errors.lastName}</div>
                    ) : (
                      ""
                    )
                  ) : (
                    ""
                  )}
                </div>
                <div
                  className={
                    submitCount ? (errors.email ? "has-error" : "") : ""
                  }
                >
                  <label htmlFor="email">Email</label>
                  <Field
                    name="email"
                    type="text"
                    id="email"
                    placeholder="Enter Email"
                    className="form-input  disabled-textbox"
                    disabled
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
                <div className="sm:col-span-2 mt-3">
                  <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={originalObj == JSON.stringify(values)}
                    onClick={() => {
                      if (Object.keys(errors).length === 0) {
                        submitForm(values);
                      }
                    }}
                  >
                    Save
                  </button>
                </div>
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </div>
  );
};

export default Profile;
