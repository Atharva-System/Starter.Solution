import { Fragment, useEffect, useState } from "react";
import { Transition, Dialog } from "@headlessui/react";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { APIs } from "../../../utils/common/api-paths";
import axiosInstance from "../../../utils/api.service";
import { FieldValidation, Regex } from "../../../utils/common/constants";

interface ManageUserModalProps {
  manageUserId: string;
  isOpen: boolean;
  onClose: () => void;
  onSave: () => void;
}

const ManageUserModal: React.FC<ManageUserModalProps> = ({
  manageUserId,
  isOpen,
  onClose,
  onSave,
}) => {
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
      const response = await axiosInstance.get(APIs.getUserApi + manageUserId);
      if (response.data)
        setUserDetails({
          firstName: response.data.data.firstName,
          lastName: response.data.data.lastName,
          email: response.data.data.email,
        });
    };

    if (manageUserId) {
      fetchUserDetails();
    }
  }, [manageUserId]);

  const resetForm = () => {
    setUserDetails({
      firstName: "",
      lastName: "",
      email: "",
    });
  };

  const resetAndClose = () => {
    if (manageUserId) resetForm();
    onClose();
  };

  const submitForm = async (values: any) => {
    const response = manageUserId
      ? await axiosInstance.put(APIs.updateUserApi + manageUserId, {
          ...values,
          id: manageUserId,
        })
      : await axiosInstance.post(APIs.inviteUserApi, values);

    if (response.data) {
      messageService.showMessage(response.data.data);
      resetForm();
      onSave();
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
    <Transition appear show={isOpen} as={Fragment}>
      <Dialog
        as="div"
        open={isOpen}
        onClose={() => (isOpen = true)}
        className="relative z-50"
      >
        <Transition.Child
          as={Fragment}
          enter="ease-out duration-300"
          enterFrom="opacity-0"
          enterTo="opacity-100"
          leave="ease-in duration-200"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <div className="fixed inset-0 bg-[black]/60" />
        </Transition.Child>
        <div className="fixed inset-0 z-[999] px-4 overflow-y-auto">
          <div className="flex items-center justify-center min-h-screen">
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 scale-95"
              enterTo="opacity-100 scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 scale-100"
              leaveTo="opacity-0 scale-95"
            >
              <Dialog.Panel className="panel border-0 p-0 rounded-lg overflow-hidden w-full max-w-lg text-black dark:text-white-dark">
                <button
                  type="button"
                  onClick={resetAndClose}
                  className="absolute top-4 ltr:right-4 rtl:left-4 text-gray-400 hover:text-gray-800 dark:hover:text-gray-600 outline-none"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  >
                    <line x1="18" y1="6" x2="6" y2="18"></line>
                    <line x1="6" y1="6" x2="18" y2="18"></line>
                  </svg>
                </button>
                <div className="text-lg font-medium bg-[#fbfbfb] dark:bg-[#121c2c] ltr:pl-5 rtl:pr-5 py-3 ltr:pr-[50px] rtl:pl-[50px]">
                  {manageUserId ? "Edit User" : "Invite User"}
                </div>
                <div className="p-5">
                  <Formik
                    initialValues={{
                      firstName: userDetails.firstName,
                      lastName: userDetails.lastName,
                      email: userDetails.email,
                    }}
                    validationSchema={SubmittedForm}
                    onSubmit={() => {}}
                  >
                    {({ errors, submitCount, values }) => (
                      <Form className="space-y-5">
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.firstName
                                  ? "has-error"
                                  : ""
                                : ""
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
                                <div className="text-danger mt-1">
                                  {errors.firstName}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                          <div
                            className={
                              submitCount
                                ? errors.lastName
                                  ? "has-error"
                                  : ""
                                : ""
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
                                <div className="text-danger mt-1">
                                  {errors.lastName}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
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
                            className="form-input"
                          />
                          {submitCount ? (
                            errors.email ? (
                              <div className="text-danger mt-1">
                                {errors.email}
                              </div>
                            ) : (
                              ""
                            )
                          ) : (
                            ""
                          )}
                        </div>
                        <div className="flex justify-end items-center mt-8">
                          <button
                            type="button"
                            className="btn btn-outline-danger"
                            onClick={resetAndClose}
                          >
                            Cancel
                          </button>
                          <button
                            type="submit"
                            className="btn btn-primary ltr:ml-4 rtl:mr-4"
                            onClick={() => {
                              if (Object.keys(errors).length === 0) {
                                submitForm(values);
                              }
                            }}
                          >
                            {manageUserId ? "Update" : "Invite"}
                          </button>
                        </div>
                      </Form>
                    )}
                  </Formik>
                </div>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition>
  );
};

export default ManageUserModal;
